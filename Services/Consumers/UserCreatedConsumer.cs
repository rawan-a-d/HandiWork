using MassTransit;
using MessagingModels;

namespace Services.Consumers
{
	internal class UserCreatedConsumer : IConsumer<UserCreated>
	{
		private readonly ILogger<UserCreated> _logger;

		public UserCreatedConsumer(ILogger<UserCreated> logger)
		{
			_logger = logger;
		}

		public async Task Consume(ConsumeContext<UserCreated> context)
		{
			await Console.Out.WriteLineAsync(context.Message.Name);
			_logger.LogInformation($"Got new message {context.Message.Name}");
		}
	}
}