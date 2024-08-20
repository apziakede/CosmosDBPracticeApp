using LearnCosmosDB.Models;
using LearnCosmosDB.Repositories;
using LearnCosmosDB.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configure Cosmos DB settings
builder.Services.Configure<CosmosSettings>(builder.Configuration.GetSection("CosmosDb"));
builder.Services.AddSingleton<GlobalSettings>();
builder.Services.AddHostedService<DatabaseInitializerService>();
builder.Services.AddScoped<IFamilyService, FamilyService>();
builder.Services.AddScoped<IChildService, ChildService>();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

