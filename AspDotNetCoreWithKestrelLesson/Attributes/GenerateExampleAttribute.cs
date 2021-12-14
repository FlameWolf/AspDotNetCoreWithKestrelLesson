namespace AspDotNetCoreWithKestrelLesson.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class GenerateExampleAttribute : Attribute
{
	public string[] Properties { set; get; }
	public object[] Values { init; get; }

	public GenerateExampleAttribute()
	{
		Properties = Array.Empty<string>();
		Values = Array.Empty<object>();
	}

	public GenerateExampleAttribute(params object[] values) : this()
	{
		Values = values;
	}
}