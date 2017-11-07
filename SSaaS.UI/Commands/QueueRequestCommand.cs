using System;
using System.Collections.Generic;
using SSaaS.Shared;

namespace SSaaS.UI
{
	public class QueueRequestCommand : ICommand
	{
		private readonly IDatabase database;
		private readonly IConsole console;

		public string Url { get; }


		public QueueRequestCommand(string url, IDatabase database, IConsole console)
		{
			this.database = database;
			this.console = console;

			Url = url;			
		}


		public void Execute()
		{
			var requests = new List<Request> { new Request { Url = Url } };
			var batch = new Batch { Requests = requests };
			database.AddBatch(batch);
			console.WriteLine($"Batch queued. Your batch ID is {batch.Id.Value}.");
		}
	}
}