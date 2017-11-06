using System;
using System.Linq;
using SSaaS.Shared;

namespace SSaaS.UI
{
	public class StatusCommand : ICommand
	{
		private IDatabase database;

		public int BatchId { get; }


		public StatusCommand(int batchId, IDatabase database)
		{
			this.database = database;
			BatchId = batchId;
		}


		public void Execute()
		{
			var batch = database.GetBatch(BatchId);
			Console.WriteLine($"The status for batch {BatchId} is {batch.Status}.");
			var messages = batch.Requests.Select(r => r.Message).Where(m => !string.IsNullOrEmpty(m));
			if (messages.Any())
			{
				Console.WriteLine("Messages:");
				foreach (var message in messages)
					Console.WriteLine(message);
			}
		}
	}
}