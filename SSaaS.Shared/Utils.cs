using System;

namespace SSaaS.Shared
{
	public static class Utils
	{
		public static T ParseAs<T>(this string value) where T : struct
		{
			return (T)Enum.Parse(typeof(T), value);
		}
	}
}