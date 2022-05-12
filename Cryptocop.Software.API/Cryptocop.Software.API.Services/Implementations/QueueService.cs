using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using Newtonsoft.Json.Serialization;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class QueueService : IQueueService, IDisposable
    {
        private IConfiguration Configuration;
        private IModel _channel;

        public QueueService(IConfiguration configuration)
        {
            Configuration = configuration;
            try
            {
                var messageBrokerSection = configuration.GetSection("MessageBroker");
                var factory = new ConnectionFactory();
                factory.Uri = new Uri(messageBrokerSection.GetSection("ConnectionString").Value);
                var connection = factory.CreateConnection();
                _channel = connection.CreateModel();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void PublishMessage(string routingKey, object payload)
        {
            var messageBrokerSection = Configuration.GetSection("MessageBroker");
            _channel.BasicPublish(
                exchange: messageBrokerSection.GetSection("ExchangeName").Value,
                routingKey,
                mandatory: true,
                basicProperties: null,
                body: ConvertJsonToBytes(payload));
        }

        private byte[] ConvertJsonToBytes(object obj) => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));

        public void Dispose()
        {
            // TODO: Dispose the connection and channel
            throw new NotImplementedException();
        }
    }
}