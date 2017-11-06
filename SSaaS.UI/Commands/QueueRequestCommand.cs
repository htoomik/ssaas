using System;
using System.Collections.Generic;
using SSaaS.Shared;

namespace SSaaS.UI
{
	public class QueueRequestCommand : ICommand
	{
		private string url;

		public QueueRequestCommand(string url)
		{
			this.url = url;
		}

		public void Execute()
		{
			var requests = new List<Request> { new Request { Url = this.url } };
			var batch = new Batch { Requests = requests };
			Database.AddBatch(batch);
			Console.WriteLine($"Batch queued. Your batch ID is {batch.Id.Value}.");
		}
	}
}