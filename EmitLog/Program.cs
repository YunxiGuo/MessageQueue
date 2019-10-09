using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;

namespace EmitLog
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory {HostName = "localhost"};
            using (var connention = factory.CreateConnection())
            {
                using (var channel = connention.CreateModel())
                {
                    channel.ExchangeDeclare("log", "fanout", true, false);
                    while (true)
                    {
                        var message = GetMessage();
                        var messageBytes = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(
                            "log",
                            "",
                            null,
                            messageBytes
                        );
                        Console.WriteLine($"the sended message is :{message}");
                        Thread.Sleep(4000);
                    }
                }
            }
        }

        static string GetMessage()
        {
            return $"hello world --- {DateTime.Now}";
        }
    }
}
