using AOPossum.Aspects;
using System;

namespace AOPossum.Tests.Mocks
{
	public class OnMethodBoundaryMock : Aspect, IOnEntryMethodBoundary
	{
		public static bool HasExecutedOnEntry = false;

		public static int ParameterCount = 0;

		public static MethodExecutionArgs OnEntryArgs = null;

		public void OnEntry(MethodExecutionArgs args)
		{
			HasExecutedOnEntry = true;

			OnEntryArgs = args;

			Console.WriteLine("On entry mock");
		}
	}
}
