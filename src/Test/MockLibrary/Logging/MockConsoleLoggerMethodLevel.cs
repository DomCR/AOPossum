using AOPossum.Logging;

namespace MockLibrary.Logging
{
	public class MockConsoleLoggerMethodLevel
	{
		[ConsoleLog]
		public void MethodToLog1() { }

		[ConsoleLog]
		public void MethodToLog2() { }
	}
}
