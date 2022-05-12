import pika
import requests
import json
from time import sleep
from os import environ

def get_connection_string():
    with open('./config/mb.%s.json' % environ.get('PYTHON_ENV'), 'r') as f:
        return json.load(f)

def connect_to_mb():
    error = False
    connection_string = get_connection_string()
    while not error:
        try:
            credentials = pika.PlainCredentials(connection_string['user'], connection_string['password'])
            connection = pika.BlockingConnection(pika.ConnectionParameters(connection_string['host'], 5672, connection_string['virtualhost'], credentials))
            channel = connection.channel()
            return channel
        except:
            sleep(5)
            continue

channel = connect_to_mb()

# Configuration
exchange_name = 'cryptocop_exchange'
create_order_routing_key = 'create-order'
order_email_queue_name = 'order_email_queue'
order_template = '<h2>Thank you for ordering @ Cryptocop! </h2><p>We hope you will enjoy our lovely product and don\'t hesitate to contact us if there are any questions.</p><table><thead><tr style="background-color: rgba(155, 155, 155, .2)"><th>Description</th><th>Unit price</th><th>Quantity</th><th>Row price</th></tr></thead><tbody>%s</tbody></table>'
address_template = '<table><thead><tr style="background-color: rgba(155, 155, 155, .2)"><th>Name</th><th>Address</th><th>City</th><th>Zip</th><th>Country</th><th>Date</th></tr></thead><tr><td>%s</td><td>%s</td><td>%s</td><td>%s</td><td>%s</td><td>%s</td></tr></table>'
def setup_queue(exchange_name, queue_name, routing_key):
    # Declare the queue, if it doesn't exist
    channel.queue_declare(queue=queue_name, durable=True)
    # Bind the queue to a specific exchange with a routing key
    channel.queue_bind(exchange=exchange_name, queue=queue_name, routing_key=routing_key)

# Declare the exchange, if it doesn't exist
channel.exchange_declare(exchange=exchange_name, exchange_type='direct', durable=True)

setup_queue(exchange_name, order_email_queue_name, create_order_routing_key)

def send_simple_message(to, subject, body):
    return requests.post(
        "https://api.mailgun.net/v3/sandbox114292bd9b3846d4bc1eab20c4346bb8.mailgun.org/messages",
        auth=("api", ""),
        data={"from": "New Order! <postmaster@sandbox114292bd9b3846d4bc1eab20c4346bb8.mailgun.org>",
              "to": to,
              "subject": subject,
              "html": body})

def send_ack(ch, delivery_tag, success):
    if success:
        ch.basic_ack(delivery_tag)

def send_order_email(ch, method, properties, data):
    parsed_msg = json.loads(data)
    email = parsed_msg['Email']
    items = parsed_msg['OrderItems']
    tbl = address_template % (parsed_msg['FullName'], parsed_msg['StreetName'] + " " + parsed_msg['HouseNumber'], parsed_msg['City'], parsed_msg['ZipCode'], parsed_msg['Country'], parsed_msg['OrderDate'])
    items_html = ''
    if (items != None):
        items_html = ''.join([ '<tr><td>%s</td><td>%.2f</td><td>%d</td><td>%.2f</td></tr>' % (item['ProductIdentifier'], float(item['UnitPrice']), int(item['Quantity']), int(item['Quantity']) * float(item['UnitPrice'])) for item in items ])
    representation = order_template % items_html
    representation += tbl
    representation += '<h2> Total Price: %s</h2>' % parsed_msg['TotalPrice']
    response = send_simple_message(email, 'Successful order!', representation)
    send_ack(ch, method.delivery_tag, response.ok)

channel.basic_consume(order_email_queue_name,
                      send_order_email)


channel.start_consuming()
connection.close()
