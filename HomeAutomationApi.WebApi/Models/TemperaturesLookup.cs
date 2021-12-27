namespace HomeAutomationApi.WebApi.Models;

public class TemperaturesLookup : Dictionary<string, short>
{
	public TemperaturesLookup() : base(StringComparer.OrdinalIgnoreCase) { }
}
