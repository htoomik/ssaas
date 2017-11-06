using SSaaS.UI;
using Xunit;

namespace SSaaS.Tests
{
	public class ArgumentParserTest
	{
		[Fact]
		public void Should_ParseStatusCommand()
		{
			const int batchId = 10;

			var command = new ArgumentParser().Parse(new [] { "status", batchId.ToString() });
			Assert.IsType<StatusCommand>(command);

			var statusCommand = (StatusCommand)command;
			Assert.Equal(batchId, statusCommand.BatchId);
		}


		[Fact]
		public void Should_ParseQueueBatchCommand()
		{
			const string filePath = "path\\to\\some\\file.txt";

			var command = new ArgumentParser().Parse(new [] { "queue", "-file", filePath });
			Assert.IsType<QueueBatchCommand>(command);

			var queueCommand = (QueueBatchCommand)command;
			Assert.Equal(filePath, queueCommand.FilePath);
		}


		[Fact]
		public void Should_ParseQueueRequestCommand()
		{
			const string url = "https://some.url.com/";

			var command = new ArgumentParser().Parse(new [] { "queue", "-url", url });
			Assert.IsType<QueueRequestCommand>(command);

			var queueCommand = (QueueRequestCommand)command;
			Assert.Equal(url, queueCommand.Url);
		}
	}
}