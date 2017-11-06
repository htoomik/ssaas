using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SSaaS.Shared;

namespace SSaaS.UI
{
	public class QueueBatchCommand : ICommand
	{
		private IDatabase database;
		
		public string FilePath { get; }


		public QueueBatchCommand(string filePath, IDatabase database)
		{
			FilePath = filePath;
			this.database = database;
		}
		

		public void Execute()
		{
			var urls = GetUrlsFromFile(FilePath);
			var requests = urls.Select(url => new Request { Url = url }).ToList();
			var batch = new Batch { Requests = requests };
			database.AddBatch(batch);
			Console.WriteLine($"Batch queued. Your batch ID is {batch.Id.Value}.");
		}


		private static List<string> GetUrlsFromFile(string arg2)
		{
			var fileContent = File.ReadAllLines(arg2);
			return fileContent.ToList();
		}
	}
}