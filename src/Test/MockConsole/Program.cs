using Mock.Shared.Logging;
using System;

namespace MockConsole
{
	internal class Program
	{
		static void Main(string[] args)
		{
			MockConsoleLogMethodLevel mock = new MockConsoleLogMethodLevel();
			mock.MethodToLog1();
			mock.MethodToLog2();
			mock.MethodToLogWithParam("this is a parameter");
			mock.MethodToLogWithMultipleParam("this is a parameter", "this is another parameter");
			mock.MethodWithoutLogging();

			MockConsoleLogClassLevel levelClass = new MockConsoleLogClassLevel();
			levelClass.MethodToLog1();
			levelClass.MethodToLog2();
			levelClass.MethodToLogWithParam("this is a parameter");
			levelClass.MethodToLogWithMultipleParam("this is a parameter", "this is another parameter");

			Console.ReadKey();
		}
	}
}