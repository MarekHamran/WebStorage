namespace Interfaces
{
	public class DocNotFoundException : Exception
	{
		public DocNotFoundException(string id) : base($"Document with id '{id}' was not found") { }
	}

	public class DocExistsException : Exception
	{
		public DocExistsException(string id) : base($"Document with id '{id}' already exists") { }
	}
}