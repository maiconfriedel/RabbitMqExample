using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost", UserName = "admin", Password = "admin" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

int count = 0;

while (true)
{
    var message = $"info: Hello World! {DateTime.Now}";
    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: "logs",
                         routingKey: count == 5 ? "error" : string.Empty,
                         basicProperties: null,
                         body: body);

    Console.WriteLine($" [x] Sent {message}");
    
    if (count == 5) count = 0;
    count++;

    await Task.Delay(TimeSpan.FromSeconds(2));
}
