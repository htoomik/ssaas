using System;
using System.Collections.Generic;
using SSaaS.Shared;

namespace SSaaS.UI
{
	class Program
	{
		static void Main(string[] args)
		{
			var command = args[0].ToLower();
			switch (command)
			{
				case "queue":
					Queue(args[1]);
					break;
				case "status":
					Status(int.Parse(args[1]));
					break;
				default:
					throw new Exception($"unknown command '{command}'");
			}
		}


		private static void Queue(string url)
		{
			var batch = new Batch 
			{
				Requests = new List<Request> { new Request { Url = url }}
			};
			Database.AddBatch(batch);
			Console.WriteLine($"Batch queued. Your batch ID is {batch.Id.Value}.");
		}


		private static void Status(int batchId)
		{
			var batch = Database.GetBatch(batchId);
			var status = batch.Status;
			Console.WriteLine($"The batch with ID {batchId} is {status}.");
		}
	}
}
