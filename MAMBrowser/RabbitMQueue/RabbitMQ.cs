using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAMBrowser.RabbitMQueue
{
    public class RabbitMQ
    {
        public void Enqueue(object value, byte priority)
        {
            var Id = Startup.AppSetting.RabbitMQInfo["Id"] as string;
            var Pass = Startup.AppSetting.RabbitMQInfo["Pass"] as string;
            var Ip = Startup.AppSetting.RabbitMQInfo["Ip"] as string;
            var Port = Startup.AppSetting.RabbitMQInfo["Port"] as string;
            var QueueName = Startup.AppSetting.RabbitMQInfo["QueueName"] as string;
            

            var factory = new ConnectionFactory
            {
                Uri = new Uri($"amqp://{Id}:{Pass}@{Ip}:{Port}")
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            Dictionary<string, object> props = new Dictionary<string, object>();
            props.Add("x-max-priority", 10);
            channel.QueueDeclare(QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: props);

            var properties = channel.CreateBasicProperties();
            properties.Priority = priority;

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));
            channel.BasicPublish("", QueueName, properties, body);

        }
    }
}
