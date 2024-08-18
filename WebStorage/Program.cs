using Interfaces;
using MongoDB.Bson.Serialization;
using WebStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<IStorage, MongoStorage.MongoStorage>();
//builder.Services.AddSingleton<IStorage, MemoryStorage.MemoryStorage>();

builder.Services.AddControllers(o => o.RespectBrowserAcceptHeader = true);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o => o.ResolveConflictingActions( descs => descs.First() ));

var app = builder.Build();

BsonClassMap.RegisterClassMap<Document>(cm =>
{
	cm.MapProperty(c => c.Id);
	cm.MapProperty(c => c.Tags);
	cm.MapProperty(c => c.Data);
});

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
