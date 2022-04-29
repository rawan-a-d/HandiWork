using MassTransit;
using MessagingModels;

namespace Services
{
    internal class UserConsumer : IConsumer<UserUpdated>
    {
        private readonly ILogger<UserUpdated> _logger;

        public UserConsumer(ILogger<UserUpdated> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<UserUpdated> context)
        {
            await Console.Out.WriteLineAsync(context.Message.Name);
            _logger.LogInformation($"Got new message {context.Message.Name}");
        }
    }
}