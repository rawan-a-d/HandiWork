using Microsoft.EntityFrameworkCore;
using Users.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ----------------
// Database context
builder.Services.AddDbContext<AppDbContext>(opt =>
	// specify database type and name
	opt.UseInMemoryDatabase("InMem")
);

// User Repo
builder.Services.AddScoped<IUserRepo, UserRepo>();

// Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// ----------------

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
