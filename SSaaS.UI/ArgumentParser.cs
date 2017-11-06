using System;
using System.Linq;

namespace SSaaS.UI
{
	public class ArgumentParser
	{
		public ICommand Parse(string[] args)
		{
			var arguments = args.Select(a => a.ToLower()).ToList();
			var command = arguments[0];
			switch (command)
			{
				case "queue":
					switch (arguments[1])
					{
						case "-url":
							return new QueueRequestCommand(arguments[2]);
						case "-file":
							return new QueueBatchCommand(arguments[2]);
						default:
							throw new Exception("Unknown argument for 'queue' command");
					}
				case "status":
					return new StatusCommand(int.Parse(arguments[1]));
				default:
					throw new Exception($"unknown command '{command}'");
			}
		}
	}
}