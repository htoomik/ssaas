using System;

namespace SSaaS.Shared
{
	public class ConsoleWrapper : IConsole
	{
		public void WriteLine(string line)
		{
			Console.WriteLine(line);
		}
	}
}