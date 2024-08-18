using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Interfaces
{
	public class Document
	{
		[JsonPropertyName("Id")]
		public string Id { get; set; }
		public string[] Tags { get; set; }
		public dynamic? Data { get; set; }
	}
}
