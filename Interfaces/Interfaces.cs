using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Interfaces
{
	public interface IStorage
	{
		public Task<Document> Get(string id);

		public Task AddNew(Document document);

		public Task Update(Document document);
	}

	public interface IConverter
	{
		public string Convert(Document document);
	}
}