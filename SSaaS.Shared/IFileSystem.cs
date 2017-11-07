namespace SSaaS.Shared
{
	public interface IFileSystem
	{
		string[] ReadAllLines(string path);
	}
}