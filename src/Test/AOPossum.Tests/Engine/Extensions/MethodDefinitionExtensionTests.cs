using AOPossum.Engine.Extensions;
using AOPossum.Logging;
using AOPossum.Tests.Common;
using Mono.Cecil;
using System.Reflection;
using System.Runtime.Loader;
using Xunit.Abstractions;

namespace AOPossum.Tests.Engine.Extensions
{
	public class MethodDefinitionExtensionTests : IDisposable
	{
		private AssemblyLoadContext _context;

		private ConsoleOutputUtil _output;

		private AssemblyDefinition _mock;

		public MethodDefinitionExtensionTests(ITestOutputHelper output)
		{
			_context = new CollectibleAssemblyLoadContext();

			_output = new ConsoleOutputUtil(output);
			Console.SetOut(_output);

			_mock = AssemblyDefinition.ReadAssembly(File.OpenRead(Assembly.GetExecutingAssembly().Location));
		}

		[Fact]
		public void AddOnEntryAspectNoParametersTest()
		{
			TypeDefinition t = _mock.MainModule.GetType(typeof(Mocks.ConsoleLabMock).FullName);
			MethodDefinition m = t.Methods.FirstOrDefault(m => m.FullName == "System.Void AOPossum.Tests.Mocks.ConsoleLabMock::MethodWithNoParamaters()");

			m.AddOnEntryAspectWithParams(typeof(ConsoleLogAttribute));

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

			m.AddOnEntryAspectWithParams(typeof(ConsoleLogAttribute));

			Assembly mock = reloadMockAssembly();

			dynamic instance = mock.CreateInstance(typeof(Mocks.ConsoleLabMock).FullName);
			instance.MethodWithOneParamater("test parameter");

			Assert.Equal("INFO | MethodWithOneParamater | [System.String] test parameter | START", _output.OutputLines[0]);
			Assert.Equal("test parameter", _output.OutputLines[1]);
		}

		public void Dispose()
		{
			_mock.Dispose();
			_context.Unload();
		}

		private Assembly reloadMockAssembly()
		{
			MemoryStream ms = new MemoryStream();
			_mock.Write(ms);
			ms.Seek(0, SeekOrigin.Begin);

			return _context.LoadFromStream(ms);
		}
	}
}
