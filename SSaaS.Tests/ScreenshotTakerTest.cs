using System.IO;
using SSaaS.Worker;
using Xunit;

namespace SSaaS.Tests
{
	public class ScreenshotTakerTest
	{
		[Fact]
		public void Expect_SavesSomething()
		{
			// Ideally we'd want to test that the saved image is a valid PNG file,
			// but the System.Drawing library is not available in .NET Core.
			// TODO: see if there is an alternative library that can be used.
			
			var path = "ScreenshotTakerTest.png";
			new ScreenshotTaker().SaveScreenshot("https://www.google.com/", path);

			Assert.True(File.Exists(path));

			File.Delete(path);
		}
	}
}