using Moq;
using SSaaS.Shared;
using SSaaS.UI;
using Xunit;

namespace SSaaS.Tests
{
	public class StatusCommandTest
	{
		[Fact]
		public void Should_GetBatch()
		{
			const int batchId = 8;
			var database = new Mock<IDatabase>();
			var console = new Mock<IConsole>();

			// Assert
			database.Setup(d => d.GetBatch(batchId))
				.Returns(new Batch());

			new StatusCommand(batchId, database.Object, console.Object).Execute();

			database.VerifyAll();
		}


		[Fact]
		public void Should_PrintStatusMessage()
		{
			const int batchId = 8;
			var database = new Mock<IDatabase>();
			var console = new Mock<IConsole>();

			database.Setup(d => d.GetBatch(batchId))
				.Returns(new Batch());

			// Assert
			console.Setup(c => c.WriteLine(It.IsAny<string>()));

			new StatusCommand(batchId, database.Object, console.Object).Execute();

			console.VerifyAll();
		}
	}
}