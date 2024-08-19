using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Interfaces
{
	/// <summary>
	/// Interface for various storages for json (memory, cloud, disk ...)
	/// </summary>
	public interface IStorage
	{
		/// <summary>
		/// Gets the document with specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public Task<Document> Get(string id);

		/// <summary>
		/// Adds the new document to storage
		/// </summary>
		/// <param name="document">The document.</param>
		/// <returns></returns>
		public Task AddNew(Document document);

		/// <summary>
		/// Updates the existing document identified by its id field.
		/// </summary>
		/// <param name="document">The document.</param>
		/// <returns></returns>
		public Task Update(Document document);
	}

	/// <summary>
	/// Interface for converting <see cref="Document"/> into json
	/// </summary>
	public interface IConverter
	{
		/// <summary>
		/// Converts the specified <see cref="Document"/>.
		/// </summary>
		/// <param name="document">The document.</param>
		/// <returns>string that can be write to output (e.g. http response)</returns>
		public string Convert(Document document);
	}
}