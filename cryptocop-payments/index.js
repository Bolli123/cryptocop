const config = require('./config/mb.json')
const amqp = require('amqplib/callback_api');
var valid = require("card-validator");


const messageBrokerInfo = {
    exchanges: {
        order: config.exchangeName
    },
    routingKeys: {
        createOrder: config.routingKey
    },
    queues: {
        orderQueue: 'order_queue'
    }
}

const createMessageBrokerConnection = () => new Promise((resolve, reject) => {

    amqp.connect(process.env.AMQP_RECIEVE_URL, (err, conn) => {
        if (err) {
            reject(err);
        }
        resolve(conn);
    });
});

const createChannel = connection => new Promise((resolve, reject) => {
    connection.createChannel((err, channel) => {
        if (err) { reject(err); }
        resolve(channel);
    });
});

const configureMessageBroker = channel => {
    const { order } = messageBrokerInfo.exchanges;
    const { queues } = messageBrokerInfo.queues;
    const { createOrder } = messageBrokerInfo.routingKeys;
    channel.assertExchange(order, 'direct', {durable:true})
    channel.assertQueue(queues, {durable:true})
    channel.bindQueue(queues, order, createOrder)
};

(async () => {
    var messageBrokerConnection = undefined;
    while(messageBrokerConnection == undefined)
    {
        await new Promise(r => setTimeout(r, 2000));
        try
        {   
            messageBrokerConnection = await createMessageBrokerConnection();
        }
        catch(err)
        {
            console.log('Connection failed, retrying...')
            messageBrokerConnection = undefined
        }
    }
    const channel = await createChannel(messageBrokerConnection);
    configureMessageBroker(channel);
    const { queues } = messageBrokerInfo.queues;
    channel.consume(queues, message => {
        var validNumber = (valid.number(message.content.CreditCard));
        if (validNumber.isValid)
        {
          console.log("The given card number is valid")
        }
        else
        {
          console.log("The given card number is invalid")
        }
        
    })
    
})().catch(e => console.error(e));
