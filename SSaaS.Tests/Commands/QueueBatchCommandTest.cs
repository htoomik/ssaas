using Moq;
using SSaaS.Shared;
using SSaaS.UI;
using Xunit;

namespace SSaaS.Tests
{
	public class QueueBatchCommandTest
	{
		[Fact]
		public void Should_GetUrlsFromFile()
		{
			const string path = "some\\path.txt";
			const string url1 = "some/url.com";
			const string url2 = "some/other/url.com";
			var database = new Mock<IDatabase>();
			var console = new Mock<IConsole>();
			var fileSystem = new Mock<IFileSystem>();
			
			fileSystem
				.Setup(fs => fs.ReadAllLines(path))
				.Returns(new string[] { url1, url2 });

			database
				// Assert
				.Setup(d => d.AddBatch(It.Is<Batch>(b => 
					b.Requests[0].Url == url1 &&
					b.Requests[1].Url == url2)))
				// and set value to avoid NullReferenceException
				.Callback((Batch b) => b.Id = 1);

			new QueueBatchCommand(path, database.Object, console.Object, fileSystem.Object).Execute();

			database.VerifyAll();
		}
	}
}