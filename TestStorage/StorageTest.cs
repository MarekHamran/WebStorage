using Moq;
using Interfaces;

namespace StorageTests
{
	[TestClass]
	public class StorageTest
	{
		private Mock<IStorage> _storageMock;
		private Document _dummy;

		public StorageTest() 
		{
			_storageMock = new Mock<IStorage>();
			_dummy = new Document { Id = "abc", Tags = new string[] { "tag1", "tag2" }, Data = "data" };
		}

		[TestMethod]
		public void TestGet()
		{
			_storageMock.Setup(s => s.Get(It.IsAny<string>())).ReturnsAsync(_dummy);

			Document doc = _storageMock.Object.Get("abc").Result;

			Assert.AreEqual(doc.Id, "abc");
			Assert.AreEqual(doc.Tags[0], "tag1");
			Assert.AreEqual(doc.Tags[1], "tag2");
		}

		[TestMethod]
		public void TestGetNotExisting()
		{
			_storageMock.Setup(s => s.Get(It.IsAny<string>())).ThrowsAsync(new DocNotFoundException("abc"));

			Assert.ThrowsExceptionAsync<DocNotFoundException>(() => _storageMock.Object.Get("abc")).Wait();
		}


		[TestMethod]
		public void TestPost()
		{
			_storageMock.Setup(s => s.AddNew(It.IsAny<Document>()));

			_storageMock.Object.AddNew(_dummy).Wait();
			Assert.IsTrue(true);
		}

		[TestMethod]
		public void TestPostExisting()
		{
			_storageMock.Setup(s => s.AddNew(It.IsAny<Document>())).ThrowsAsync(new DocExistsException("abc"));

			Assert.ThrowsExceptionAsync<DocExistsException>(() => _storageMock.Object.AddNew(_dummy)).Wait();
		}
	}
}