using Microsoft.EntityFrameworkCore;
using Users.Data;
using MassTransit;
using Users.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// ----------------
string jwtConfig;
string rabbitMQ;
string connectionString;

if (builder.Environment.IsProduction())
{
	jwtConfig = Environment.GetEnvironmentVariable("JWT");
	rabbitMQ = Environment.GetEnvironmentVariable("RABBIT_MQ");
	connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

	// Database context - SQL server
	Console.WriteLine("--> Using SqlServer Db");
	builder.Services.AddDbContext<AppDbContext>(opt =>
		// specify database type and name
		opt.UseSqlServer(connectionString)
	);
}
else
{
	jwtConfig = builder.Configuration["JwtConfig:Secret"];
	rabbitMQ = $"amqp://guest:guest@{builder.Configuration["RabbitMQHost"]}:{builder.Configuration["RabbitMQPort"]}";
	connectionString = builder.Configuration.GetConnectionString("UsersDB");

	// Database context - In memory
	Console.WriteLine("--> Using InMem Db");
	builder.Services.AddDbContext<AppDbContext>(opt =>
		// specify database type and name
		opt.UseInMemoryDatabase("InMem")
	);
}

// User Repo
builder.Services.AddScoped<IUserRepo, UserRepo>();

// Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// MassTransit
builder.Services.AddMassTransit(config =>
{
	// register consumer
	config.AddConsumer<UserConsumer>();

	config.UsingRabbitMq((ctx, cfg) =>
	{
		Console.WriteLine(rabbitMQ);
		cfg.Host(rabbitMQ);

		// This is the consumer
		// creates exchange and queue with this name
		cfg.ReceiveEndpoint("Users_user-endpoint", c =>
		{
			// define the consumer class
			c.ConfigureConsumer<UserConsumer>(ctx);
		});
	});
});

// Cors
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
	options.AddPolicy(name: MyAllowSpecificOrigins,
		policy =>
		{
			policy.WithOrigins(
				"http://localhost:4200",
				"http://localhost:80"
			)
			.AllowAnyMethod()
			.AllowAnyHeader();
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

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

// Prep data
PrepDb.PrepPopulation(app, app.Environment.IsDevelopment());

app.Run();
