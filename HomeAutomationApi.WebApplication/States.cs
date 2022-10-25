namespace HomeAutomationApi.WebApplication;

[Flags]
public enum States : byte
{
	None = 0,
	Off = 1,
	On = 2,
	Toggle = 4,
}
