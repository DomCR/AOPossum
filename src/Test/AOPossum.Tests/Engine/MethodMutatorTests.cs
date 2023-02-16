using AOPossum.Engine.Core;
using AOPossum.Logging;
using AOPossum.Tests.Common;
using Mono.Cecil;
using System.Runtime.Loader;
using Xunit.Abstractions;

namespace AOPossum.Tests.Engine
{
	public class MethodMutatorTests : IDisposable
	{
		private ConsoleOutputUtil _output;

		private AssemblyLoadContext _context;

		private AssemblyDefinition _mock;

		private ModuleDefinition _aopModule;

		public MethodMutatorTests(ITestOutputHelper output)
		{
			this._context = new CollectibleAssemblyLoadContext();

			_output = new ConsoleOutputUtil(output);
			Console.SetOut(_output);

			MemoryStream ms = new MemoryStream(File.ReadAllBytes(CommonVars.MockLibOriginal));
			_mock = AssemblyDefinition.ReadAssembly(ms);
			_aopModule = AssemblyDefinition.ReadAssembly("AOPossum.dll").MainModule;
		}

		[Fact]
		public void AddStaticLoggerTest()
		{
			TypeDefinition t = _mock.MainModule.GetType("MockLibrary.MethodAspectTest");
			MethodDefinition m = t.Methods.FirstOrDefault(m => m.FullName == "System.Void MockLibrary.MethodAspectTest::HelloWorld(System.String)");

			MethodMutator.CreateMethodArgs(m);

			// Write the module with default parameters
			_mock.Write(CommonVars.MockLibOutput);

			var assemblyBytes = new MemoryStream(System.IO.File.ReadAllBytes(CommonVars.MockLibOutput));
			var mock = _context.LoadFromStream(assemblyBytes);

			dynamic instance = mock.CreateInstance("MockLibrary.MethodAspectTest");
			instance.HelloWorld("This is my method");
		}

		[Fact]
		public void CreateMethodArgsTest()
		{
			TypeDefinition t = _mock.MainModule.GetType("MockLibrary.MethodAspectTest");
			MethodDefinition m = t.Methods.FirstOrDefault(m => m.FullName == "System.Void MockLibrary.MethodAspectTest::HelloWorld(System.String)");
			MethodMutator.CreateMethodArgs<ConsoleLogAttribute>(m);

			_mock.Write(CommonVars.MockLibOutput);

			var assemblyBytes = new MemoryStream(System.IO.File.ReadAllBytes(CommonVars.MockLibOutput));
			var mock = _context.LoadFromStream(assemblyBytes);

			dynamic instance = mock.CreateInstance("MockLibrary.MethodAspectTest");
			instance.HelloWorld("This is my method");
		}

		public void Dispose()
		{
			this._aopModule.Assembly.Dispose();
			this._mock.Dispose();
			this._context.Unload();
		}
	}
}
