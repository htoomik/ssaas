﻿using System;
using System.Threading;
using SSaaS.Shared;

namespace SSaaS.Worker
{
	class Program
	{
		static void Main(string[] args)
		{
			var database = new Database();
			while (true)
			{
				var request = database.GetNextRequest();
				if (request != null)
				{
					Logger.Log($"Found request {request.Id} for url {request.Url}");
					RequestStatus status;
					string message = null;

					if (!IsValidUrl(request.Url))
					{
						status = RequestStatus.Failed;
						message = "Invalid URL";
						Logger.Log($"'{request.Url}' is an invalid url; skipping");
					}
					else
					{
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
					}
					database.SetStatus(request, status, message);
				}
				else
				{
					Thread.Sleep(1000);
				}
			}
		}


		private static bool IsValidUrl(string urlString)
		{
			try
			{
				var urlObject = new Uri(urlString);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
