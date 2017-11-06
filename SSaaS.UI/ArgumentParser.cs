using System;
using System.Linq;
using SSaaS.Shared;

namespace SSaaS.UI
{
	public class ArgumentParser
	{
		public ICommand Parse(string[] args, IDatabase database, IConsole console)
		{
			var arguments = args.Select(a => a.ToLower()).ToList();
			var command = arguments[0];
			switch (command)
			{
				case "queue":
					switch (arguments[1])
					{
						case "-url":
							return new QueueRequestCommand(arguments[2], database);
						case "-file":
							return new QueueBatchCommand(arguments[2], database);
						default:
							throw new Exception("Unknown argument for 'queue' command");
					}
				case "status":
					return new StatusCommand(int.Parse(arguments[1]), database, console);
				default:
					throw new Exception($"unknown command '{command}'");
			}
		}
	}
}