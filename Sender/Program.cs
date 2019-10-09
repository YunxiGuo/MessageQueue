using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory {HostName = "localhost", Port = 5672};
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: "work_queue",
                        durable: true,
                        exclusive: false,
                        autoDelete: true,
                        arguments: null
                    );
                    var messages = "hello rabbitmq. DateTime:";
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    while (true)
                    {
                        var str = messages + DateTime.Now;
                        var body = Encoding.UTF8.GetBytes(str);
                        channel.BasicPublish(
                            exchange: "",
                            routingKey: "work_queue",
                            basicProperties: properties,
                            body: body);
                        Console.WriteLine($"message to send :{str}");
                        Thread.Sleep(2000);
                    }
                }
            }
        }

        static string GetMessages(string[] args)
        {
            return args.Length > 0 ? string.Join(",", args) : "..." + " hello world!";
        }
    }
}
