using System;
using System.IO;

namespace SSaaS.Shared
{
	public class ImagePathGenerator
	{
		public static string GeneratePathFor(Request request)
		{
			var config = new ConfigLoader().LoadConfig();
			var fullPath = config.FilesFolder + $"{request.BatchId}/{request.Id}.png";
			var folder = Path.GetDirectoryName(fullPath);
			if (!Directory.Exists(folder))
				Directory.CreateDirectory(folder);
			return fullPath;
		}
	}
}