using System;
using System.Linq;
using SSaaS.Shared;

namespace SSaaS.UI
{
	public class StatusCommand : ICommand
	{
		private int batchId;

		public StatusCommand(int batchId)
		{
			this.batchId = batchId;
		}


		public void Execute()
		{
			var batch = Database.GetBatch(batchId);
			Console.WriteLine($"The status for batch {batchId} is {batch.Status}.");
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