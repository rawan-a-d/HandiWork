using MassTransit;
using Microsoft.EntityFrameworkCore;
using Services.Consumers;
using Services.Data;
using Services.Helpers;
using Users.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -------------------
// Database
if (builder.Environment.IsProduction())
{
	Console.WriteLine("--> Using SqlServer Db");
	// Database context - SQL server
	builder.Services.AddDbContext<AppDbContext>(opt =>
		// specify database type and name
		opt.UseSqlServer(builder.Configuration.GetConnectionString("ServicesDB"))
	);
}
else
{
	Console.WriteLine("--> Using InMem Db");
	builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
}

// Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Repo
builder.Services.AddScoped<IServiceRepo, ServiceRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IServiceCategoryRepo, ServiceCategoryRepo>();
builder.Services.AddScoped<IPhotoRepo, PhotoRepo>();

// Cloudinary
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddScoped<IPhotoService, PhotoService>();

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
// -------------------

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
PrepDb.PrepPopulation(app);

app.Run();
