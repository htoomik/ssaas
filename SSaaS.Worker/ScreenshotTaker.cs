using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SSaaS.Shared;

namespace SSaaS.Worker
{
	public class ScreenshotTaker : IScreenshotTaker
	{
		public void SaveScreenshot(string url, string path)
		{
			var chromeOptions = new ChromeOptions();
			chromeOptions.BinaryLocation = new ConfigLoader().LoadConfig().ChromePath;
			using (var driver = new ChromeDriver(".", chromeOptions))
			{
				driver.Navigate().GoToUrl(url);
				var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
				screenshot.SaveAsFile(path);
			}
		}
	}
}