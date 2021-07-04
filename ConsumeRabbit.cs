using System;
using System.Text;
using Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace angularDotnet
{
    public class ConsumeRabbit
    {
        public static void Consume(IModel channel)
        {
            channel.QueueDeclare("demo-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
            };

            channel.BasicConsume("demo-queue", true, consumer);
            Console.WriteLine("Consumer started");
            //   Console.ReadLine();
        }

        public static void Publish(IModel channel)
        {
           channel.ExchangeDeclare("pramesh-exchange",ExchangeType.Direct);
            //channel.QueueDeclare("data-one", true, false, false, null);
            var data = new UserViewModel()
            {
                Country = "Nepal",
                City = "Kathmandu",
                Gender = "Male",
                UserName = "Pramesh",
                Email = "pramesh@gmail.com "
            };

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            channel.BasicPublish("pramesh-exchange", "pramesh-init", false, null, body);

        }
    }
}