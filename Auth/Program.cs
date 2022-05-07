using System.Text;
using Auth.Data;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// ----------------------
if (builder.Environment.IsProduction())
{
	Console.WriteLine("--> Using SqlServer Db");
	// Database context - SQL server
	builder.Services.AddDbContext<AppDbContext>(opt =>
		// specify database type and name
		opt.UseSqlServer(builder.Configuration.GetConnectionString("IdentityDB"))
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

// Authentication
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	// in case first one fails
	options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
// JWT token configuration
.AddJwtBearer(jwt =>
{
	// how it should be encoded
	var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Secret"]);

	// jwt token settings
	jwt.SaveToken = true;
	jwt.TokenValidationParameters = new TokenValidationParameters
	{
		// validate third part of the token using the secret and check if it was configured and encrypted by us
		ValidateIssuerSigningKey = true,
		// define signing key (responsible for encrypting)
		IssuerSigningKey = new SymmetricSecurityKey(key),
		ValidateIssuer = false,
		ValidateAudience = false,
		ValidateLifetime = true,
		// should be true in production
		RequireExpirationTime = false,
	};
});

// Identity configuration
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddEntityFrameworkStores<AppDbContext>();

// Configuration
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// MassTransit
builder.Services.AddMassTransit(config =>
{
	config.UsingRabbitMq((ctx, cfg) =>
	{
		//cfg.Host("amqp://guest:guest@localhost:5672");
		Console.WriteLine($"amqp://guest:guest@{builder.Configuration["RabbitMQHost"]}:{builder.Configuration["RabbitMQPort"]}");
		cfg.Host($"amqp://guest:guest@{builder.Configuration["RabbitMQHost"]}:{builder.Configuration["RabbitMQPort"]}");
	});
});
// ----------------------


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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Prep data
PrepDb.PrepPopulation(app, app.Environment.IsDevelopment());

app.Run();
