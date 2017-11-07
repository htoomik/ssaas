using System;
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
			console.WriteLine($"The overall status for batch {BatchId} is {batch.Status}.");

			foreach (var request in batch.Requests)
			{
				switch (request.Status)
				{
					case RequestStatus.New:
						console.WriteLine($"The request for {request.Url} has not been processed yet.");
						break;
					case RequestStatus.Processing:
						console.WriteLine($"The request for {request.Url} is being processed.");
						break;
					case RequestStatus.Done:
						console.WriteLine($"A screenshot for {request.Url} has been saved at {request.Path}");
						break;
					case RequestStatus.Failed:
						console.WriteLine($"The request for {request.Url} failed: {request.Message}");
						break;
					default:
						throw new Exception("Unexpected RequestStatus " + request.Status);
				}
			}
		}
	}
}