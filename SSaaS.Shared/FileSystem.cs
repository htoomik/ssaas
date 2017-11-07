using System.IO;

namespace SSaaS.Shared
{
	public class FileSystem : IFileSystem
	{
		public string[] ReadAllLines(string path)
		{
			return File.ReadAllLines(path);
		}
	}
}