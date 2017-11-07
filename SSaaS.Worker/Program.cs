using System;
using System.Threading;
using SSaaS.Shared;

namespace SSaaS.Worker
{
	class Program
	{
		static void Main(string[] args)
		{
			var database = new Database();
			var screenshotTaker = new ScreenshotTaker();
			var processor = new Processor(database, screenshotTaker);

			while (true)
			{
				processor.Process();
			}
		}
	}
}
