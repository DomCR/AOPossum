using AOPossum.Logging;

namespace MockLibrary.Logging
{
	[ConsoleLog]
	public class MockConsoleLoggerClassLevel
	{
		public void MethodToLog1() { }

		public void MethodToLog2() { }
	}
}
