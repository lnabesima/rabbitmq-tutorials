using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory{HostName = "localhost"};
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync("hello", false, false, false, null);

Console.WriteLine("[*] Waiting for messages...");
await Task.Delay(TimeSpan.FromSeconds(1));

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += (model, ea) => {
  var body = ea.Body.ToArray();
  var message = Encoding.UTF8.GetString(body);
  Console.WriteLine($"[x] Received {message}");
  Task.Delay(TimeSpan.FromSeconds(0.5));
  return Task.CompletedTask;
};

await channel.BasicConsumeAsync("hello", autoAck: true,consumer: consumer);

Console.WriteLine("Press [enter] to exit.");
Console.ReadLine();