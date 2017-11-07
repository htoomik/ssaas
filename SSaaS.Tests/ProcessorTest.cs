using Moq;
using SSaaS.Shared;
using SSaaS.Worker;
using Xunit;

namespace SSaaS.Tests
{
	public class ProcessorTest
	{
		[Fact]
		public void When_NoRequests_NoProblem()
		{
			var database = new Mock<IDatabase>();
			var screenshotTaker = new Mock<IScreenshotTaker>();

			database.Setup(d => d.GetNextRequest()).Returns<Request>(null);

			new Processor(database.Object, screenshotTaker.Object).Process();
		}


		[Fact]
		public void When_ValidUrl_Should_TakeScreenshot()
		{
			const string url = "https://www.google.com/";
			var request = new Request { Url = url };

			var database = new Mock<IDatabase>();
			var screenshotTaker = new Mock<IScreenshotTaker>();

			database.Setup(d => d.GetNextRequest()).Returns(request);

			// Assert
			screenshotTaker.Setup(s => s.SaveScreenshot(url, It.IsAny<string>()));

			new Processor(database.Object, screenshotTaker.Object).Process();

			screenshotTaker.VerifyAll();
		}


		[Fact]
		public void When_ValidUrl_Should_SetStatusToDone()
		{
			const string url = "https://a.test.domain.com/";
			var request = new Request { Url = url };

			var database = new Mock<IDatabase>();
			var screenshotTaker = new Mock<IScreenshotTaker>();

			database.Setup(d => d.GetNextRequest()).Returns(request);

			// Assert
			database.Setup(d => d.SetStatus(request, RequestStatus.Done, null));
			
			new Processor(database.Object, screenshotTaker.Object).Process();

			database.VerifyAll();
		}


		[Fact]
		public void When_InvalidUrl_Should_NotTakeScreenshot()
		{
			const string url = "invalid";
			var request = new Request { Url = url };

			var database = new Mock<IDatabase>();
			var screenshotTaker = new Mock<IScreenshotTaker>();

			database.Setup(d => d.GetNextRequest()).Returns(request);

			new Processor(database.Object, screenshotTaker.Object).Process();

			screenshotTaker.Verify(s => s.SaveScreenshot(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
		}


		[Fact]
		public void When_InvalidUrl_Should_SetStatusToFailed()
		{
			const string url = "invalid";
			var request = new Request { Url = url };

			var database = new Mock<IDatabase>();
			var screenshotTaker = new Mock<IScreenshotTaker>();

			database.Setup(d => d.GetNextRequest()).Returns(request);

			// Assert
			database.Setup(d => d.SetStatus(request, RequestStatus.Failed, It.IsAny<string>()));
			
			new Processor(database.Object, screenshotTaker.Object).Process();

			database.VerifyAll();
		}
	}
}