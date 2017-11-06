using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SSaaS.Shared;

namespace SSaaS.UI
{
	class Program
	{
		static void Main(string[] args)
		{
			var arguments = args.Select(a => a.ToLower()).ToList();
			var command = arguments[0];
			switch (command)
			{
				case "queue":
					Queue(arguments[1], arguments[2]);
					break;
				case "status":
					Status(int.Parse(arguments[1]));
					break;
				default:
					throw new Exception($"unknown command '{command}'");
			}
		}


		private static void Queue(string queueSourceType, string queueSource)
		{
			List<Request> requests;
			switch (queueSourceType)
			{
				case "-url":
					requests = new List<Request> { new Request { Url = queueSource } };
					break;
				case "-file":
					var urls = GetUrlsFromFile(queueSource);
					requests = urls.Select(url => new Request { Url = url }).ToList();
					break;
				default:
					throw new Exception("Unknown queue source " + queueSourceType);
			}
			var batch = new Batch { Requests = requests };
			Database.AddBatch(batch);
			Console.WriteLine($"Batch queued. Your batch ID is {batch.Id.Value}.");
		}


		private static List<string> GetUrlsFromFile(string arg2)
		{
			var fileContent = File.ReadAllLines(arg2);
			return fileContent.ToList();
		}


		private static void Status(int batchId)
		{
			var batch = Database.GetBatch(batchId);
			var status = batch.Status;
			Console.WriteLine($"The batch with ID {batchId} is {status}.");
		}
	}
}
