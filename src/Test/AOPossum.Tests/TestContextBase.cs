using AOPossum.Tests.Common;
using Mono.Cecil;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
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

		protected Assembly reloadMockAssembly()
		{
			MemoryStream ms = new MemoryStream();
			_mock.Write(ms);
			ms.Seek(0, SeekOrigin.Begin);

			return _context.LoadFromStream(ms);
		}

		public void Dispose()
		{
			_mock.Dispose();
			_context.Unload();
		}
	}
}
