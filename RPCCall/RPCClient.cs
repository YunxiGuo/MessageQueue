using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RPCCall
{
    public class RPCClient
    {
        private IConnection _connection;

        public RPCClient()
        {
            var factory = new ConnectionFactory() {HostName = "localhost"};
            _connection = factory.CreateConnection();
            var channel = _connection.CreateModel();
            var replyQueueName = channel.QueueDeclare().QueueName;
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (o, e) => { };

        }

        public string Call(string str)
        {
            return "";
        }

        public void Close()
        {

        }

    }
}
