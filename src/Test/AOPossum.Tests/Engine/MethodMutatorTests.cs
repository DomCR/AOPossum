using AOPossum.Engine.Core;
using AOPossum.Engine.Extensions;
using AOPossum.Logging;
using AOPossum.Tests.Common;
using Mono.Cecil;
using System.Reflection;
using System.Runtime.Loader;
using Xunit.Abstractions;

namespace AOPossum.Tests.Engine
{
	public class MethodMutatorTests : IDisposable
	{
		private AssemblyLoadContext _context;

		private ConsoleOutputUtil _output;

		private AssemblyDefinition _mock;

		public MethodMutatorTests(ITestOutputHelper output)
		{
			this._context = new CollectibleAssemblyLoadContext();

			this._output = new ConsoleOutputUtil(output);
			Console.SetOut(_output);

			this._mock = AssemblyDefinition.ReadAssembly(File.OpenRead(Assembly.GetExecutingAssembly().Location));
		}

		[Fact]
		public void AddOnEntryAspectNoParametersTest()
		{
			TypeDefinition t = _mock.MainModule.GetType(typeof(Mocks.ConsoleLabMock).FullName);
			MethodDefinition m = t.Methods.FirstOrDefault(m => m.FullName == "System.Void AOPossum.Tests.Mocks.ConsoleLabMock::MethodWithNoParamaters()");

			m.AddOnEntryAspect(typeof(ConsoleLogAttribute));

			Assembly mock = reloadMockAssembly();

			dynamic instance = mock.CreateInstance(typeof(Mocks.ConsoleLabMock).FullName);
			instance.MethodWithNoParamaters();

			Assert.Equal("OnEntry executed in : MethodWithNoParamaters", this._output.OutputLines[0]);
			Assert.Equal("MethodWithNoParamaters", this._output.OutputLines[1]);
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

			Assert.Equal("OnEntry executed in : MethodWithOneParamater", this._output.OutputLines[0]);
			Assert.Equal("test parameter", this._output.OutputLines[1]);
		}

		[Fact]
		public void AddOnEntryAspectOneParameterTypeTest()
		{
			TypeDefinition t = _mock.MainModule.GetType(typeof(Mocks.ConsoleLabMock).FullName);
			MethodDefinition m = t.Methods.FirstOrDefault(m => m.FullName == "System.Void AOPossum.Tests.Mocks.ConsoleLabMock::MethodWithOneParamater(System.String)");

			m.AddOnEntryAspect(typeof(ConsoleLogAttribute));

			Assembly mock = reloadMockAssembly();

			dynamic instance = mock.CreateInstance(typeof(Mocks.ConsoleLabMock).FullName);
			instance.MethodWithOneParamater("test parameter");

			Assert.Equal("OnEntry executed in : MethodWithOneParamater", this._output.OutputLines[0]);
			Assert.Equal("test parameter", this._output.OutputLines[1]);
		}

		public void Dispose()
		{
			this._mock.Dispose();
			this._context.Unload();
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
