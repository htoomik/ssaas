using System.IO;
using Microsoft.Extensions.Configuration;

namespace SSaaS.Shared
{
	public class ConfigLoader
	{
		public Config LoadConfig()
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
			var configuration = builder.Build();

			var config = new Config();
			configuration.Bind(config);

			return config;
		}
	}
}