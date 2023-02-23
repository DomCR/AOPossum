using AOPossum.Logging;
using System;

namespace Mock.Shared.Logging
{
	[ConsoleLog]
	public class MockConsoleLogClassLevel
	{
		public void MethodToLog1()
		{
			Console.WriteLine($"Body of {nameof(MethodToLog1)}");
		}

		public void MethodToLog2()
		{
			Console.WriteLine($"Body of {nameof(MethodToLog2)}");
		}

		public void MethodToLogWithParam(string param)
		{
			Console.WriteLine($"Body of {nameof(MethodToLog2)} with param {param}");
		}

		public void MethodToLogWithMultipleParam(string param1, string param2)
		{
			Console.WriteLine($"Body of {nameof(MethodToLog2)} with param {param1} | {param2}");
		}
	}
}
