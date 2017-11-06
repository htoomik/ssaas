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
			var command = new ArgumentParser().Parse(args, new Database(), new ConsoleWrapper());
			command.Execute();
		}
	}
}
