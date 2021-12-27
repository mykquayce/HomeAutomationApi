using System.Net.NetworkInformation;

namespace HomeAutomationApi.WebApi.Models;

public class AliasesLookup : Dictionary<string, PhysicalAddress> 
{
	public AliasesLookup() : base(StringComparer.OrdinalIgnoreCase) { }
}
