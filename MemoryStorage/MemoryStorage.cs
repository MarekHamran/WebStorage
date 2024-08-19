using Interfaces;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace MemoryStorage
{
	/// <summary>
	/// In-memory storage of json documents
	/// </summary>
	/// <seealso cref="Interfaces.IStorage" />
	public class MemoryStorage : IStorage
	{
		private static readonly Dictionary<string, Document> _storage = new Dictionary<string, Document>();

		/// <summary>
		/// Gets the document with specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		/// <exception cref="Interfaces.DocNotFoundException"></exception>
		public async Task<Document> Get(string id)
		{
			lock (_storage)
			{
				if (_storage.ContainsKey(id))
					return _storage[id];
				throw new DocNotFoundException(id);
			}
		}

		/// <summary>
		/// Adds the new document to storage
		/// </summary>
		/// <param name="document">The document.</param>
		/// <exception cref="Interfaces.DocExistsException"></exception>
		public async Task AddNew(Document document)
		{
			lock (_storage)
			{
				if (_storage.ContainsKey(document.Id))
					throw new DocExistsException(document.Id);
				
				_storage[document.Id] = document;
			}
		}

		/// <summary>
		/// Updates the existing document identified by its id field.
		/// </summary>
		/// <param name="document">The document.</param>
		/// <exception cref="Interfaces.DocNotFoundException"></exception>
		public async Task Update(Document document)
		{
			lock (_storage)
			{
				if (!_storage.ContainsKey(document.Id))
					throw new DocNotFoundException(document.Id);

				_storage[document.Id] = document;
			}
		}
	}
}