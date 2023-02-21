using AOPossum.Logging;

namespace AOPossum.Samples.ConsoleLogging.Default.Logging
{
	public class ConsoleLoggerMethodLevel
	{
		[ConsoleLog]
		public void SayHelloWorld()
		{
			Console.WriteLine("Hello World!!!");
		}

		[ConsoleLog]
		public void SayGoodbyeWorld()
		{
			Console.WriteLine("Goodbye World!!!");
		}

		[ConsoleLog]
		public void SayHelloTo(string name)
		{
			Console.WriteLine($"Hello {name}");
		}
	}
}
