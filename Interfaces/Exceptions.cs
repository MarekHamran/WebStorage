namespace Interfaces
{
	/// <summary>
	/// Thrown when document with id specified in id field was not found
	/// </summary>
	/// <seealso cref="System.Exception" />
	public class DocNotFoundException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DocNotFoundException"/> class.
		/// </summary>
		/// <param name="id">The identifier.</param>
		public DocNotFoundException(string id) : base($"Document with id '{id}' was not found") { }
	}

	/// <summary>
	/// Thrown when there was attempt to post document with id and document with such id already exists
	/// </summary>
	/// <seealso cref="System.Exception" />
	public class DocExistsException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DocExistsException"/> class.
		/// </summary>
		/// <param name="id">The identifier.</param>
		public DocExistsException(string id) : base($"Document with id '{id}' already exists") { }
	}
}