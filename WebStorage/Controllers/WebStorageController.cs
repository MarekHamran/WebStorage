using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Xml;
using XmlConverter;

namespace WebStorage.Controllers
{
	/// <summary>
	/// Controller for web storage of json documents
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
	[ApiController]
	[Route("documents")]
	public class WebStorageController : ControllerBase
	{

		private readonly ILogger<WebStorageController> _logger;
		private readonly IStorage _storage;

		/// <summary>
		/// Initializes a new instance of the <see cref="WebStorageController"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="storage">The storage.</param>
		public WebStorageController(ILogger<WebStorageController> logger, IConfiguration configuration, IStorage storage)
		{
			_logger = logger;
			_storage = storage;
		}

		/// <summary>
		/// Gets document with specified id as json
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		[HttpGet]
		[AcceptHeader("application/json")]
		[Route("{id}")]
		public async Task<ActionResult<Document>> Get([FromRoute] string id)
		{
			try
			{
				Document document = await _storage.Get(id);
				return document;
			}
			catch (DocNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Gets document with specified id as XML
		/// </summary>
		/// <param name="id">The identifier.</param>
		[HttpGet]
		[AcceptHeader("application/xml")]
		[Route("{id}")]
		public async Task GetXml([FromRoute] string id)
		{
			try
			{
				Document doc = await _storage.Get(id);

				Response.ContentType = "application/xml";

				IConverter converter = new XmlConverter.XmlConverter();
				await Response.WriteAsync(converter.Convert(doc));
			}
			catch (DocNotFoundException ex)
			{
				Response.StatusCode = 404;
				await Response.WriteAsync(ex.Message);
			}
		}

		/// <summary>
		/// Posts document
		/// Document with specified id must not exists
		/// </summary>
		/// <param name="content">The content.</param>
		/// <returns></returns>
		[HttpPost]
		public async Task<ActionResult> Post([FromBody] Document content)
		{
			if (!ValidateInputDoc(content, out string error))
			{
				return BadRequest(error);
			}

			try
			{
				await _storage.AddNew(content);
			}
			catch (DocExistsException ex)
			{
				return Problem(ex.Message);
			}

			return Accepted();
		}

		/// <summary>
		/// Update document with specified id
		/// </summary>
		/// <param name="content">The content.</param>
		/// <returns></returns>
		[HttpPut]
		public async Task<ActionResult> Put([FromBody] Document content)
		{
			if ( ! ValidateInputDoc(content, out string error))
			{
				return BadRequest(error);
			}

			try
			{
				await _storage.Update(content);
			}
			catch (DocNotFoundException ex)
			{
				return BadRequest(ex.Message);
			}

			return Accepted();
		}

		private bool ValidateInputDoc (Document doc, out string error)
		{
			if (string.IsNullOrEmpty(doc.Id))
			{
				error = "Missing or invalid Id field";
				return false;
			}
			if (doc.Data is null)
			{
				error = "Missing or invalid Data field";
				return false;
			}
			error = string.Empty;
			return true;
		}
	}
}