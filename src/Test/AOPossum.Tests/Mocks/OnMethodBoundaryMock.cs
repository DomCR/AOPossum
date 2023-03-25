using AOPossum.Aspects;
using System;

namespace AOPossum.Tests.Mocks
{
	public class OnMethodBoundaryMock : Aspect, IOnEntryMethodBoundary, IOnExitMethodBoundary
	{
		public static bool HasExecutedOnEntry = false;

		public static bool HasExecutedOnExit = false;

		public static int ParameterCount = 0;

		public static MethodExecutionArgs OnEntryArgs = null;

		public void OnEntry(MethodExecutionArgs args)
		{
			HasExecutedOnEntry = true;

			OnEntryArgs = args;

			Console.WriteLine("On entry mock");
		}

		public void OnExit(MethodExecutionArgs args)
		{
			HasExecutedOnExit = true;

			OnEntryArgs = args;

			Console.WriteLine("On exit mock");
		}
	}
}
