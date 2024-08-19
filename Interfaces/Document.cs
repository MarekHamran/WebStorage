using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Interfaces
{
	/// <summary>
	/// Represent json document
	/// </summary>
	public class Document
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		[JsonPropertyName("Id")]
		public string Id { get; set; }
		/// <summary>
		/// Gets or sets the tags.
		/// </summary>
		/// <value>
		/// The tags.
		/// </value>
		public string[] Tags { get; set; }
		/// <summary>
		/// Gets or sets the data.
		/// </summary>
		/// <value>
		/// The data. Can has various structures.
		/// </value>
		public dynamic? Data { get; set; }
	}
}
