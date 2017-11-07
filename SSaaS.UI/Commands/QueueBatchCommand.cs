using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SSaaS.Shared;

namespace SSaaS.UI
{
	public class QueueBatchCommand : ICommand
	{
		private readonly IDatabase database;
		private readonly IConsole console;
		private readonly IFileSystem fileSystem;
		
		public string FilePath { get; }


		public QueueBatchCommand(string filePath, IDatabase database, IConsole console, IFileSystem fileSystem)
		{
			this.database = database;
			this.console = console;
			this.fileSystem = fileSystem;

			FilePath = filePath;
		}
		

		public void Execute()
		{
			var urls = GetUrlsFromFile(FilePath);
			var requests = urls.Select(url => new Request { Url = url }).ToList();
			var batch = new Batch { Requests = requests };
			database.AddBatch(batch);
			console.WriteLine($"Batch queued. Your batch ID is {batch.Id.Value}.");
		}


		private List<string> GetUrlsFromFile(string arg2)
		{
			var fileContent = fileSystem.ReadAllLines(arg2);
			return fileContent.ToList();
		}
	}
}