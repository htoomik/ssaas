using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SSaaS.Worker
{
	public class ScreenshotTaker
	{
		public void SaveScreenshot(string url, string path)
		{
			var chromeOptions = new ChromeOptions();
			chromeOptions.BinaryLocation = "/Applications/Chrome.app/Contents/MacOS/Google Chrome";
			using (var driver = new ChromeDriver(".", chromeOptions))
			{
				driver.Navigate().GoToUrl(url);
				var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
				screenshot.SaveAsFile(path);
			}
		}
	}
}