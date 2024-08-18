using Interfaces;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml;

namespace XmlConverter
{
	public class XmlConverter : IConverter
	{
		public string Convert(Document document)
		{
			StringBuilder sb = new StringBuilder();
			using (TextWriter textWriter = new StringWriter(sb))
			{
				using (XmlWriter writer = new XmlTextWriter(textWriter))
				{

					if (!(document.Data is JsonElement))
					{
						string serialized = JsonSerializer.Serialize(document.Data);
						document.Data = JsonSerializer.Deserialize<dynamic>(serialized);
					}

					JsonElement element = document.Data;

					writer.WriteStartDocument();
					writer.WriteStartElement("document");

					writer.WriteElementString("id", document.Id);
					writer.WriteElementString("tags", string.Join(';', document.Tags));
					writer.WriteStartElement("data");
					foreach (JsonProperty o in element.EnumerateObject())
						WriteProperty(o);
					writer.WriteEndElement();

					writer.WriteEndElement();
					writer.WriteEndDocument();

					void WriteProperty(JsonProperty prop)
					{
						switch (prop.Value.ValueKind)
						{
							case JsonValueKind.Object:
								writer.WriteStartElement(prop.Name);
								foreach (JsonProperty property in prop.Value.EnumerateObject())
									WriteProperty(property);
								writer.WriteEndElement();
								break;
							case JsonValueKind.Array:
								writer.WriteStartElement(prop.Name);
								WriteArray(prop.Value);
								writer.WriteEndElement();
								break;
							default:
								writer.WriteElementString(prop.Name, prop.Value.ToString());
								break;

						}
					}

					void WriteArray(JsonElement array)
					{
						int i = 0;
						foreach (JsonElement element in array.EnumerateArray())
						{
							switch (element.ValueKind)
							{
								case JsonValueKind.Array:
									WriteArray(element);
									break;
								case JsonValueKind.Object:
									foreach (JsonProperty o in element.EnumerateObject())
										WriteProperty(o);
									break;
								default:
									writer.WriteElementString($"item{i++}", element.GetRawText());
									break;
							}
						}

					}
				}
				textWriter.Flush();
			}
			return sb.ToString();
		}

	}
}