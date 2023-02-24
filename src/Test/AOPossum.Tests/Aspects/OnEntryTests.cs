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

		[Fact]
		public void OnEntryWithParamater()
		{
			TypeDefinition t = _mock.MainModule.GetType(typeof(Mock.Shared.Logging.MockConsoleLogMethodLevel).FullName);
			MethodDefinition m = t.Methods.FirstOrDefault(m => m.Name == nameof(Mock.Shared.Logging.MockConsoleLogMethodLevel.MethodToLogWithParam));

			m.AddOnEntryAspectWithParams(typeof(OnMethodBoundaryMock));

			Assembly mock = reloadMockAssembly();

			string param = "hellow this is a parameter";
			dynamic instance = mock.CreateInstance(typeof(Mock.Shared.Logging.MockConsoleLogMethodLevel).FullName);
			instance.MethodToLogWithParam(param);

			Type boundary = mock.DefinedTypes.FirstOrDefault(o => o.FullName == typeof(OnMethodBoundaryMock).FullName);
			object hasEnteredValue = boundary.GetField(nameof(OnMethodBoundaryMock.HasExecutedOnEntry)).GetValue(null);
			Assert.True((bool)hasEnteredValue);

			MethodExecutionArgs args = (MethodExecutionArgs)boundary.GetField(nameof(OnMethodBoundaryMock.OnEntryArgs)).GetValue(null);
			Assert.NotNull(args);
			Assert.NotEmpty(args.Parameters);
			Assert.Equal(param, args.Parameters.FirstOrDefault());
		}

		[Fact]
		public void OnEntryWithMultipleParamaters()
		{
			TypeDefinition t = _mock.MainModule.GetType(typeof(Mock.Shared.Logging.MockConsoleLogMethodLevel).FullName);
			MethodDefinition m = t.Methods.FirstOrDefault(m => m.Name == nameof(Mock.Shared.Logging.MockConsoleLogMethodLevel.MethodToLogWithMultipleParam));

			m.AddOnEntryAspectWithParams(typeof(OnMethodBoundaryMock));

			Assembly mock = reloadMockAssembly();

			string param1 = "parameter 1";
			string param2 = "parameter 2";
			dynamic instance = mock.CreateInstance(typeof(Mock.Shared.Logging.MockConsoleLogMethodLevel).FullName);
			instance.MethodToLogWithMultipleParam(param1, param2);

			Type boundary = mock.DefinedTypes.FirstOrDefault(o => o.FullName == typeof(OnMethodBoundaryMock).FullName);
			object hasEnteredValue = boundary.GetField(nameof(OnMethodBoundaryMock.HasExecutedOnEntry)).GetValue(null);
			Assert.True((bool)hasEnteredValue);

			MethodExecutionArgs args = (MethodExecutionArgs)boundary.GetField(nameof(OnMethodBoundaryMock.OnEntryArgs)).GetValue(null);
			Assert.NotNull(args);
			Assert.NotEmpty(args.Parameters);
			Assert.Equal(param1, args.Parameters.ElementAt(0));
			Assert.Equal(param2, args.Parameters.ElementAt(1));
		}
	}
}
