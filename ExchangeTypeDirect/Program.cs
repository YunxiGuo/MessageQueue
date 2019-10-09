//*******************************************************************
//  版权所有：珠海优特电力科技股份有限公司
//  版 本 号：1.00.00
//  功能说明：测试RabbitMQ在ExchangeType=Direct时,其消息分发时routingkey必须完全匹配
//  创建日期：2019/10/9 11:25:12
//  作者：郭云喜
//  CLR版本: 4.0.30319.42000
//  修改者：
//  修改日期：
//  修改说明：
// ******************************************************************

using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;

namespace DirectTypeProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("DirectTypeProducer");
            var factory = new ConnectionFactory() {HostName = "localhost"};
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //定义exchange,设置其type为direct
                    channel.ExchangeDeclare(exchange: "direct_ex",
                        type: ExchangeType.Direct,
                        durable: true,
                        autoDelete: false,
                        arguments: null);
                    //定义队列queue.direct.cn.q1
                    channel.QueueDeclare(queue: "queue.direct.cn.q1",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
                    //定义队列queue.direct.cn.q2
                    channel.QueueDeclare(queue: "queue.direct.cn.q2",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
                    //将队列queue.direct.cn.q1绑定到direct_ex,并设置routingkey为queue_direct_q1
                    channel.QueueBind(queue: "queue.direct.cn.q1",
                        exchange: "direct_ex",
                        routingKey: "queue_direct_q1",
                        arguments: null);
                    //将队列queue.direct.cn.q2绑定到direct_ex,并设置routingkey为queue_direct_q2
                    channel.QueueBind(queue: "queue.direct.cn.q2",
                        exchange: "direct_ex",
                        routingKey: "queue_direct_q2",
                        arguments: null);
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    while (true)
                    {
                        var message = $"message send to routingkey queue_direct_q1,{DateTime.Now}";
                        var body = Encoding.UTF8.GetBytes(message);
                        //发送消息到exchange: "direct_ex",routingKey: "queue_direct_q1"
                        channel.BasicPublish(exchange: "direct_ex",
                            routingKey: "queue_direct_q1",
                            basicProperties: properties,
                            body: body);
                        var message1 = $"message send to routingkey queue_direct_q2,{DateTime.Now}";
                        var body1 = Encoding.UTF8.GetBytes(message1);
                        //发送消息到exchange: "direct_ex",routingKey: "queue_direct_q2"
                        channel.BasicPublish(exchange: "direct_ex",
                            routingKey: "queue_direct_q2",
                            basicProperties: properties,
                            body: body1);
                        Console.WriteLine(message);
                        Console.WriteLine(message1);
                        Thread.Sleep(5000);
                    }
                }
            }
        }
    }
}
