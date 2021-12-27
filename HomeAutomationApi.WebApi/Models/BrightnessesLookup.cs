namespace HomeAutomationApi.WebApi.Models;

public class BrightnessesLookup : Dictionary<string, double>
{
	public BrightnessesLookup() : base(StringComparer.OrdinalIgnoreCase) { }
}
