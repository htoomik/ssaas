using System;
using System.Collections.Generic;
using SSaaS.Shared;

namespace SSaaS.UI
{
	public class QueueRequestCommand : ICommand
	{
		private readonly IDatabase database;

		public string Url { get; }


		public QueueRequestCommand(string url, IDatabase database)
		{
			Url = url;
			this.database = database;
		}


		public void Execute()
		{
			var requests = new List<Request> { new Request { Url = Url } };
			var batch = new Batch { Requests = requests };
			database.AddBatch(batch);
			Console.WriteLine($"Batch queued. Your batch ID is {batch.Id.Value}.");
		}
	}
}