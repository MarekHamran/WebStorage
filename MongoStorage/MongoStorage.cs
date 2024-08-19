using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System.Text.Json;

namespace MongoStorage
{
	/// <summary>
	/// MongoDB storage for json documents
	/// </summary>
	/// <seealso cref="Interfaces.IStorage" />
	public class MongoStorage : IStorage
	{
		private static string connectionString = string.Empty;

		private static readonly Lazy<MongoClient> mongoClient = new Lazy<MongoClient>(() => new MongoClient(MongoClientSettings.FromConnectionString(connectionString))) ;

		public MongoStorage() { }

		public MongoStorage(IConfiguration config) 
		{
			connectionString = config.GetConnectionString("mongo") ?? throw new Exception("Missing mongo db connection string in appsettings.json");
		}

		/// <summary>
		/// Gets the document with specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		/// <exception cref="Interfaces.DocNotFoundException"></exception>
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

		/// <summary>
		/// Adds the new document to storage
		/// </summary>
		/// <param name="document">The document.</param>
		/// <exception cref="Interfaces.DocExistsException"></exception>
		public async Task AddNew(Document document)
		{
			var db = mongoClient.Value.GetDatabase("DocStorage");
			var collection = db.GetCollection<Document>("DocCollection");

			var filter = Builders<Document>.Filter.Eq(d => d.Id, document.Id);

			// Document.Data property contains JsonElement (System.Text.Json put it to dynamic type)
			// Workaround is to serialize it and deserialize using mongoDB BSON deserializer
			var serialized = JsonSerializer.Serialize(document);
			var bson = BsonSerializer.Deserialize<Document>(serialized);

			if (0 == await collection.Find(filter).CountDocumentsAsync())
				await collection.InsertOneAsync(bson);
			else
				throw new DocExistsException(document.Id);
		}

		/// <summary>
		/// Updates the existing document identified by its id field.
		/// </summary>
		/// <param name="document">The document.</param>
		/// <exception cref="Interfaces.DocNotFoundException"></exception>
		public async Task Update(Document document)
		{
			var db = mongoClient.Value.GetDatabase("DocStorage");
			var collection = db.GetCollection<Document>("DocCollection");

			var filter = Builders<Document>.Filter.Eq(d => d.Id, document.Id);

			// Document.Data property contains JsonElement (System.Text.Json put it to dynamic type)
			// Workaround is to serialize it and deserialize using mongoDB BSON deserializer
			var serialized = JsonSerializer.Serialize(document);
			var bson = BsonSerializer.Deserialize<Document>(serialized);

			if (0 == await collection.Find(filter).CountDocumentsAsync())
				throw new DocNotFoundException(document.Id);
			else
				await collection.ReplaceOneAsync(filter, bson);
		}
	}
}