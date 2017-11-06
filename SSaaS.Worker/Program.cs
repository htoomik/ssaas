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
					Logger.Log($"Found request {request.Id} for url {request.Url}");
					RequestStatus status;
					string message = null;
					try
					{
						var path = ImagePathGenerator.GeneratePathFor(request);
						Logger.Log($"Saving screenshot at {path}");
						new ScreenshotTaker().SaveScreenshot(request.Url, path);
						status = RequestStatus.Done;
					}
					catch (Exception ex)
					{
						Logger.Log(ex.Message);
						status = RequestStatus.Failed;
						message = ex.Message;
					}
					Database.SetStatus(request, status, message);
				}
				else
				{
					Thread.Sleep(1000);
				}
			}
		}
	}
}
