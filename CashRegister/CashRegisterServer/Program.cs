// Load the DotEnv file
var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, ".env");
CashRegisterServer.DotEnv.Load(dotenv);
// dunno what it is, works also without
var config =
    new ConfigurationBuilder()
        .AddEnvironmentVariables()
        .Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseDeveloperExceptionPage();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
