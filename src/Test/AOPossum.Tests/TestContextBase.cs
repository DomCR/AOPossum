using AOPossum.Aspects;
using AOPossum.Tests.Common;
using AOPossum.Tests.Mocks;
using Mono.Cecil;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Xunit;
using Xunit.Abstractions;

namespace AOPossum.Tests
{
	public abstract class TestContextBase : IDisposable
	{
		protected ConsoleOutputUtil _output;

		protected AssemblyDefinition _mock;

		private AssemblyLoadContext _context;

		public TestContextBase(ITestOutputHelper output)
		{
			_context = new CollectibleAssemblyLoadContext();

			_output = new ConsoleOutputUtil(output);
			Console.SetOut(_output);

			_mock = AssemblyDefinition.ReadAssembly(File.OpenRead(Assembly.GetExecutingAssembly().Location));
		}

		public void Dispose()
		{
			_mock.Dispose();
			_context.Unload();
		}

		protected Assembly reloadMockAssembly()
		{
			MemoryStream ms = new MemoryStream();
			_mock.Write(ms);
			ms.Seek(0, SeekOrigin.Begin);

			return _context.LoadFromStream(ms);
		}

		protected MethodDefinition getMethodDefinition<T>(string methodName)
		{
			TypeDefinition t = this._mock.MainModule.GetType(typeof(T).FullName);
			return t.Methods.FirstOrDefault(m => m.Name == methodName);
		}

		protected void assertExecution(Type boundary, params object[] pars)
		{
			MethodExecutionArgs args = (MethodExecutionArgs)boundary.GetField(nameof(OnMethodBoundaryMock.OnEntryArgs)).GetValue(null);
			Assert.NotNull(args);

			if (pars == null)
				return;

			Assert.Equal(pars.Length, args.Parameters.Count());
			for (int i = 0; i < pars.Length; i++)
			{
				Assert.Equal(pars[i], args.Parameters.ElementAt(i));
			}
		}
	}
}
