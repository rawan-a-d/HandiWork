using MassTransit;
using Microsoft.EntityFrameworkCore;
using Services.Consumers;
using Services.Data;
using Users.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -------------------
// Database
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));

// Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Repo
builder.Services.AddScoped<IServiceRepo, ServiceRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IServiceCategoryRepo, ServiceCategoryRepo>();

// Event Processor
//builder.Services.AddSingleton<IEventProcessor, EventProcessor>();

// Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// MassTransit
builder.Services.AddMassTransit(config =>
{
	// register consumers
	config.AddConsumer<UserUpdatedConsumer>();
	config.AddConsumer<UserCreatedConsumer>();

	config.UsingRabbitMq((ctx, cfg) =>
	{
		cfg.Host($"amqp://guest:guest@{builder.Configuration["RabbitMQHost"]}:{builder.Configuration["RabbitMQPort"]}");

		// These are the consumers
		// creates exchange and queue with this name
		cfg.ReceiveEndpoint("Services_user-update-endpoint", c =>
		{
			// define the consumer class
			c.ConfigureConsumer<UserUpdatedConsumer>(ctx);
		});
		cfg.ReceiveEndpoint("Services_user-create-endpoint", c =>
		{
			// define the consumer class
			c.ConfigureConsumer<UserCreatedConsumer>(ctx);
		});
	});
});
// -------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

// Prep data
PrepDb.PrepPopulation(app);

app.Run();
