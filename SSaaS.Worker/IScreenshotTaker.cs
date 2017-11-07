namespace SSaaS.Worker
{
	public interface IScreenshotTaker
	{
		void SaveScreenshot(string url, string path);
	}
}