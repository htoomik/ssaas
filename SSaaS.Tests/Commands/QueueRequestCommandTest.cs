using Moq;
using SSaaS.Shared;
using SSaaS.UI;
using Xunit;

namespace SSaaS.Tests
{
	public class QueueRequestCommandTest
	{
		[Fact]
		public void Should_SaveRequestWithUrl()
		{
			const string url = "some/url.com";
			var database = new Mock<IDatabase>();
			var console = new Mock<IConsole>();
			
			database
				// Assert
				.Setup(d => d.AddBatch(It.Is<Batch>(b => b.Requests[0].Url == url)))
				// and set value to avoid NullReferenceException
				.Callback((Batch b) => b.Id = 1);

			new QueueRequestCommand(url, database.Object, console.Object).Execute();

			database.VerifyAll();
		}
	}
}