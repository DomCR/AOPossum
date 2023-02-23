using System;

namespace AOPossum.Tests.Mocks
{
	public class ConsoleLabMock
	{
		public void MethodWithNoParamaters()
		{
			Console.WriteLine(nameof(MethodWithNoParamaters));
		}

		public void MethodWithOneParamater(string param)
		{
			Console.WriteLine(param);
		}
	}
}
