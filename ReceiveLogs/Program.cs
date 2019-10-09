using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ReceiveLogs
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory {HostName = "localhost"};
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare("log", "fanout", true, false);
                    //获取自动生成的队列的名称
                    var queueName = channel.QueueDeclare().QueueName;
                    channel.QueueBind(queue: queueName, exchange: "log", routingKey: "");
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (o, e) =>
                    {
                        var msg = Encoding.UTF8.GetString(e.Body);
                        Console.WriteLine($"the receive message is :{msg}");
                    };
                    channel.BasicConsume(queue: queueName, autoAck: true, consumer);
                    Console.ReadLine();
                }
            }
        }
    }
}
