using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory{HostName = "localhost"};
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

int counter = 1;

for(int i = 0; i < 1000000; i++){
  string message = $"Hello World! {counter}";
  var body = Encoding.UTF8.GetBytes(message);
  
  await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "hello", body: body);
  Console.WriteLine($"[x] Sent {message}");
  counter++;
}

Console.WriteLine("Press [enter] to exit.");
Console.ReadLine();