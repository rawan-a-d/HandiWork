using Microsoft.EntityFrameworkCore;
using Users.AsyncDataServices;
using Users.Data;
using MassTransit;
using Users.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// ----------------
// Database context
if (builder.Environment.IsProduction())
{
	Console.WriteLine("--> Using SqlServer Db");
	// Database context - SQL server
	builder.Services.AddDbContext<AppDbContext>(opt =>
		// specify database type and name
		opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
	);
}
else
{
	Console.WriteLine("--> Using InMem Db");
	// Database context - In memory
	builder.Services.AddDbContext<AppDbContext>(opt =>
		// specify database type and name
		opt.UseInMemoryDatabase("InMem")
	);
}

// User Repo
builder.Services.AddScoped<IUserRepo, UserRepo>();

// RabbitMQ
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

// Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// MassTransit
builder.Services.AddMassTransit(config =>
{
	// register consumer
	config.AddConsumer<UserConsumer>();

	config.UsingRabbitMq((ctx, cfg) =>
	{
		Console.WriteLine($"amqp://guest:guest@{builder.Configuration["RabbitMQHost"]}:{builder.Configuration["RabbitMQPort"]}");
		cfg.Host($"amqp://guest:guest@{builder.Configuration["RabbitMQHost"]}:{builder.Configuration["RabbitMQPort"]}");

		// This is the consumer
		// creates exchange and queue with this name
		cfg.ReceiveEndpoint("Users_user-endpoint", c =>
		{
			// define the consumer class
			c.ConfigureConsumer<UserConsumer>(ctx);
		});
	});
});

// ----------------

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
PrepDb.PrepPopulation(app, app.Environment.IsProduction());


app.Run();
