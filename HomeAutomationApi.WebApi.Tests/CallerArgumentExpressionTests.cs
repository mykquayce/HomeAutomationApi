using System.Runtime.CompilerServices;
using Xunit;

namespace HomeAutomationApi.WebApi.Tests;

public class CallerArgumentExpressionTests
{
	[Theory]
	[InlineData("1")]
	public void Test1(string s)
	{
		var actual = GetCallerArgumentName(s);
		Assert.NotNull(actual);
		Assert.NotEqual(s, actual);
		Assert.Equal(nameof(s), actual);
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
	private static string? GetCallerArgumentName(string argument, [CallerArgumentExpression("argument")] string? message = default)
		=> message;
}
