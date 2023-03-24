using AOPossum.Engine.Extensions;
using AOPossum.Logging;
using AOPossum.Tests.Common;
using Mono.Cecil;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Xunit;
using Xunit.Abstractions;

namespace AOPossum.Tests.Engine.Extensions
{
	public class MethodDefinitionExtensionTests : TestContextBase
	{
		public MethodDefinitionExtensionTests(ITestOutputHelper output) : base(output) { }

		[Fact]
		public void AddOnEntryAspectNoParametersTest()
		{
			TypeDefinition t = _mock.MainModule.GetType(typeof(Mocks.ConsoleLabMock).FullName);
			MethodDefinition m = t.Methods.FirstOrDefault(m => m.FullName == "System.Void AOPossum.Tests.Mocks.ConsoleLabMock::MethodWithNoParamaters()");

			m.AddOnEntryAspect(typeof(ConsoleLogAttribute));

			Assembly mock = reloadMockAssembly();

			dynamic instance = mock.CreateInstance(typeof(Mocks.ConsoleLabMock).FullName);
			instance.MethodWithNoParamaters();

			Assert.Equal("INFO | MethodWithNoParamaters | START", _output.OutputLines[0]);
			Assert.Equal("MethodWithNoParamaters", _output.OutputLines[1]);
		}

		[Fact]
		public void AddOnEntryAspectOneParameterTest()
		{
			TypeDefinition t = _mock.MainModule.GetType(typeof(Mocks.ConsoleLabMock).FullName);
			MethodDefinition m = t.Methods.FirstOrDefault(m => m.FullName == "System.Void AOPossum.Tests.Mocks.ConsoleLabMock::MethodWithOneParamater(System.String)");

			m.AddOnEntryAspect(typeof(ConsoleLogAttribute));

			Assembly mock = reloadMockAssembly();

			dynamic instance = mock.CreateInstance(typeof(Mocks.ConsoleLabMock).FullName);
			instance.MethodWithOneParamater("test parameter");

			Assert.Equal("INFO | MethodWithOneParamater | [System.String] test parameter | START", _output.OutputLines[0]);
			Assert.Equal("test parameter", _output.OutputLines[1]);
		}
	}
}
