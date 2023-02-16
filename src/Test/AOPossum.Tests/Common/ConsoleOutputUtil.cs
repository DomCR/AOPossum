using System.Text;
using Xunit.Abstractions;

namespace AOPossum.Tests.Common
{
	public class ConsoleOutputUtil : TextWriter
	{
		public override Encoding Encoding { get; } = Encoding.Default;

		private ITestOutputHelper _output;

		public ConsoleOutputUtil(ITestOutputHelper helper)
		{
			_output = helper;
		}

		public override void WriteLine(string? value)
		{
			_output.WriteLine(value);
		}
	}
}