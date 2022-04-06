using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using Users.Dtos;

namespace Users.AsyncDataServices
{
	public class MessageBusClient : IMessageBusClient
	{
		private readonly IConfiguration _configuration;
		private readonly IConnection _connection;
		private readonly IModel _channel;

		public MessageBusClient(IConfiguration configuration)
		{
			_configuration = configuration;

			var factory = new ConnectionFactory()
			{
				HostName = _configuration["RabbitMQHost"],
				Port = int.Parse(_configuration["RabbitMQPort"])
			};

			try
			{
				_connection = factory.CreateConnection();
				_channel = _connection.CreateModel();

				_channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

				Console.WriteLine("--> Connected to MessageBus");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"--> Could not connect to the Message Bus: {ex.Message}");
			}
		}

		public void PublishUpdatedUser(UserPublishedDto userPublishedDto)
		{
			var message = JsonSerializer.Serialize(userPublishedDto);

			if (_connection.IsOpen)
			{
				Console.WriteLine("--> RabbitMQ Connection Open, sending message...");

				SendMessage(message);
			}
		}

		public void SendMessage(string message)
		{
			var body = Encoding.UTF8.GetBytes(message);

			_channel.BasicPublish(
				exchange: "trigger",
				routingKey: "",
				basicProperties: null,
				body: body
			);

			Console.WriteLine($"--> We have sent {message}");
		}
	}
}