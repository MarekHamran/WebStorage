using Interfaces;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System.Text.Json;

namespace MongoStorage
{
	public class MongoStorage : IStorage
	{
		private static readonly string connectionString = "mongodb+srv://admin:1234@cluster0.5my4n.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";

		private static readonly Lazy<MongoClient> mongoClient = new Lazy<MongoClient>(() => new MongoClient(MongoClientSettings.FromConnectionString(connectionString))) ;

		public async Task<Document> Get(string id)
		{
			var db = mongoClient.Value.GetDatabase("DocStorage");
			var collection = db.GetCollection<Document>("DocCollection");
			
			Document document = await collection.Find(d => d.Id == id).FirstOrDefaultAsync();
			if (document == null)
			{
				throw new DocNotFoundException(id);
			}
			return document;
		}

		public async Task AddNew(Document document)
		{
			var db = mongoClient.Value.GetDatabase("DocStorage");
			var collection = db.GetCollection<Document>("DocCollection");

			var filter = Builders<Document>.Filter.Eq(d => d.Id, document.Id);

			var serialized = JsonSerializer.Serialize(document);
			var bson = BsonSerializer.Deserialize<Document>(serialized);

			if (0 == await collection.Find(filter).CountDocumentsAsync())
				await collection.InsertOneAsync(bson);
			else
				throw new DocExistsException(document.Id);
		}

		public async Task Update(Document document)
		{
			var db = mongoClient.Value.GetDatabase("DocStorage");
			var collection = db.GetCollection<Document>("DocCollection");

			var filter = Builders<Document>.Filter.Eq(d => d.Id, document.Id);

			var serialized = JsonSerializer.Serialize(document);
			var bson = BsonSerializer.Deserialize<Document>(serialized);

			if (0 == await collection.Find(filter).CountDocumentsAsync())
				throw new DocNotFoundException(document.Id);
			else
				await collection.ReplaceOneAsync(filter, bson);
		}
	}
}