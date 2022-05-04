using System.Text;
using Auth.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// ----------------------
// DB
builder.Services.AddDbContext<AppDbContext>(opt =>
		// specify database type and name
		//opt.UseSqlServer(
		//	builder.Configuration.GetConnectionString("DefaultConnection")
		//)
		opt.UseInMemoryDatabase("InMem")
);

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
PrepDb.PrepPopulation(app);

app.Run();
