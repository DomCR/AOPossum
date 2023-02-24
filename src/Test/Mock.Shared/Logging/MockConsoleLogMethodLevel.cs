using AOPossum.Logging;
using System;

namespace Mock.Shared.Logging
{
	public class MockConsoleLogMethodLevel
	{
		[ConsoleLog]
		public void MethodToLog1()
		{
			Console.WriteLine($"Body of {nameof(MethodToLog1)}");
		}

		[ConsoleLog]
		public void MethodToLog2()
		{
			Console.WriteLine($"Body of {nameof(MethodToLog2)}");
		}

		public void MethodWithoutLogging()
		{
			Console.WriteLine("this method should not have any extra logs");
		}

		[ConsoleLog]
		public void MethodToLogWithParam(string param)
		{
			Console.WriteLine($"Body of {nameof(MethodToLog2)} with param {param}");
		}

		[ConsoleLog]
		public void MethodToLogWithMultipleParam(string param1, string param2)
		{
			Console.WriteLine($"Body of {nameof(MethodToLog2)} with param {param1} | {param2}");
		}
	}
}
