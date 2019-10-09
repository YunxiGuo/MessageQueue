using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DirectTypeConsumer1
{
    class Program
    {
        private static string _queueName = "queue.direct.cn.q1";

        static void Main(string[] args)
        {
            Console.WriteLine("DirectTypeConsumer1");
            var factory = new ConnectionFactory {HostName = "localhost"};
            using(var con = factory.CreateConnection())
            using (var channel = con.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_ex",
                    type: ExchangeType.Direct,
                    durable: true,
                    autoDelete: false,
                    arguments: null);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (o, e) =>
                {
                    var message = Encoding.UTF8.GetString(e.Body);
                    Console.WriteLine($"received message is :{message}");
                };
                channel.BasicConsume(queue: _queueName,
                    autoAck: true,
                    consumer: consumer);
                Console.ReadLine();
            }
        }
    }
}
