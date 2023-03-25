using AOPossum.Aspects;
using AOPossum.Engine.Extensions;
using AOPossum.Tests.Mocks;
using Mono.Cecil;
using System;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace AOPossum.Tests.Aspects
{
	public class OnExitTests : TestContextBase
	{
		public OnExitTests(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public void OnExitNoParamaters()
		{
			TypeDefinition t = _mock.MainModule.GetType(typeof(Mock.Shared.Logging.MockConsoleLogMethodLevel).FullName);
			MethodDefinition m = t.Methods.FirstOrDefault(m => m.Name == nameof(Mock.Shared.Logging.MockConsoleLogMethodLevel.MethodToLog1));

			m.AddOnExitAspect(typeof(OnMethodBoundaryMock));

			Assembly mock = reloadMockAssembly();

			dynamic instance = mock.CreateInstance(typeof(Mock.Shared.Logging.MockConsoleLogMethodLevel).FullName);
			instance.MethodToLog1();

			Type boundary = mock.DefinedTypes.FirstOrDefault(o => o.FullName == typeof(OnMethodBoundaryMock).FullName);
			object hasExitedValue = boundary.GetField(nameof(OnMethodBoundaryMock.HasExecutedOnExit)).GetValue(null);
			Assert.True((bool)hasExitedValue);

			MethodExecutionArgs args = (MethodExecutionArgs)boundary.GetField(nameof(OnMethodBoundaryMock.OnEntryArgs)).GetValue(null);
			Assert.NotNull(args);
			Assert.Empty(args.Parameters);
		}
	}
}
