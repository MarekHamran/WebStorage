using Interfaces;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace MemoryStorage
{
	public class MemoryStorage : IStorage
	{
		private static readonly Dictionary<string, Document> _storage = new Dictionary<string, Document>();

		public async Task<Document> Get(string id)
		{
			lock (_storage)
			{
				if (_storage.ContainsKey(id))
					return _storage[id];
				throw new DocNotFoundException(id);
			}
		}

		public async Task AddNew(Document document)
		{
			lock (_storage)
			{
				if (_storage.ContainsKey(document.Id))
					throw new DocExistsException(document.Id);
				
				_storage[document.Id] = document;
			}
		}

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