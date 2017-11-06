using System.Linq;
using SSaaS.Shared;

namespace SSaaS.UI
{
	public class StatusCommand : ICommand
	{
		private readonly IDatabase database;
		private readonly IConsole console;

		public int BatchId { get; }


		public StatusCommand(int batchId, IDatabase database, IConsole console)
		{
			this.database = database;
			this.console = console;
			
			BatchId = batchId;
		}


		public void Execute()
		{
			var batch = database.GetBatch(BatchId);
			console.WriteLine($"The status for batch {BatchId} is {batch.Status}.");
			var messages = batch.Requests.Select(r => r.Message).Where(m => !string.IsNullOrEmpty(m));
			if (messages.Any())
			{
				console.WriteLine("Messages:");
				foreach (var message in messages)
					console.WriteLine(message);
			}
		}
	}
}