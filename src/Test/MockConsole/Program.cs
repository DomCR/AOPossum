using MockLibrary.Logging;

namespace MockConsole
{
	internal class Program
	{
		static void Main(string[] args)
		{
			MockConsoleLoggerMethodLevel mock = new MockConsoleLoggerMethodLevel();
			mock.MethodToLog1();
			mock.MethodToLog2();
		}
	}
}