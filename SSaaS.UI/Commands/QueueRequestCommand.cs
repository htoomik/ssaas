using System;
using System.Collections.Generic;
using SSaaS.Shared;

namespace SSaaS.UI
{
	public class QueueRequestCommand : ICommand
	{
		public string Url { get; }


		public QueueRequestCommand(string url)
		{
			Url = url;
		}


		public void Execute()
		{
			var requests = new List<Request> { new Request { Url = Url } };
			var batch = new Batch { Requests = requests };
			Database.AddBatch(batch);
			Console.WriteLine($"Batch queued. Your batch ID is {batch.Id.Value}.");
		}
	}
}