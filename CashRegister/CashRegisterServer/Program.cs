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

// statically set the port, will generate a error dialog that means nothing
builder.WebHost.ConfigureKestrel(options => options.ListenLocalhost(49157));

builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseDeveloperExceptionPage();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CashRegister V1"));
}

app.UseAuthorization();

app.MapControllers();

app.Run();
