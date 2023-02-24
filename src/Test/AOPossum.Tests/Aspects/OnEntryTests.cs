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
	public class OnEntryTests : TestContextBase
	{
		public OnEntryTests(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public void OnEntryNoParamaters()
		{
			TypeDefinition t = _mock.MainModule.GetType(typeof(Mock.Shared.Logging.MockConsoleLogMethodLevel).FullName);
			MethodDefinition m = t.Methods.FirstOrDefault(m => m.Name == nameof(Mock.Shared.Logging.MockConsoleLogMethodLevel.MethodToLog1));

			m.AddOnEntryAspectWithParams(typeof(OnMethodBoundaryMock));

			Assembly mock = reloadMockAssembly();

			dynamic instance = mock.CreateInstance(typeof(Mock.Shared.Logging.MockConsoleLogMethodLevel).FullName);
			instance.MethodToLog1();

			Type boundary = mock.DefinedTypes.FirstOrDefault(o => o.FullName == typeof(OnMethodBoundaryMock).FullName);
			object hasEnteredValue = boundary.GetField(nameof(OnMethodBoundaryMock.HasExecutedOnEntry)).GetValue(null);
			Assert.True((bool)hasEnteredValue);

			MethodExecutionArgs args = (MethodExecutionArgs)boundary.GetField(nameof(OnMethodBoundaryMock.OnEntryArgs)).GetValue(null);
			Assert.NotNull(args);
			Assert.Empty(args.Parameters);
		}
	}
}
