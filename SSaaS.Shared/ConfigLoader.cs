using System.IO;
using Microsoft.Extensions.Configuration;

namespace SSaaS.Shared
{
	public class ConfigLoader
	{
		public Config LoadConfig()
		{
			const string configFile = "appsettings.json";
			var basePath = Directory.GetCurrentDirectory();
			if (!File.Exists(Path.Combine(basePath, configFile)))
				basePath = Directory.GetParent(basePath).FullName;

			var builder = new ConfigurationBuilder()
				.SetBasePath(basePath)
				.AddJsonFile(configFile, optional: false, reloadOnChange: true);
			var configuration = builder.Build();

			var config = new Config();
			configuration.Bind(config);

			return config;
		}
	}
}