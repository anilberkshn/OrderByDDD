using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Application.Common.MessageQ
{
    public class RabbitMqProducer : IMessageProducer
    {
        public void SendMessage<T>(T message)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = factory.CreateConnection(); // todo : her seferinde connection oluşturmamalı
            using var channel = connection.CreateModel();
            
            channel.QueueDeclare(queue: "rabbitOrders",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(
                exchange: "",
                routingKey: "rabbitOrders",
                body: body);
            
            
            Console.WriteLine($" Sent Message :  {message}");
         
        }
    }
}