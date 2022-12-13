using DataAccessLayer;
using DataAccessLayer.Interfaces;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (connectionString is null)
{
    // TODO: Log an error when retrieving the connection string
    throw new ArgumentNullException(nameof(connectionString), "Failed to retrieve connection string. Is it defined in the configuration?");
}
// Add services to the container.

builder.Services.AddControllers();
// Configuring Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped((dataAccess) => DataAccessFactory.GetDataAccess<ISignalDataAccess>(connectionString));
builder.Services.AddScoped((dataAccess) => DataAccessFactory.GetDataAccess<IBidDataAccess>(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();