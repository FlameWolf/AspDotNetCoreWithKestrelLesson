namespace AspDotNetCoreWithKestrelLesson.Extensions
{
	public static class StringExtensions
	{
		public static string ToCamel(this string param) => $"{char.ToLowerInvariant(param[0])}{param[1..]}";
	}
}