using System;
using System.Threading;
using SSaaS.Shared;

namespace SSaaS.Worker
{
	class Program
	{
		static void Main(string[] args)
		{
			while (true)
			{
				var request = Database.GetNextRequest();
				if (request != null)
				{
					var path = ImagePathGenerator.GeneratePathFor(request);
					new ScreenshotTaker().SaveScreenshot(request.Url, path);
					Database.SetStatus(request, RequestStatus.Done);
				}
				else
				{
					Thread.Sleep(1000);
				}
			}
		}
	}
}
