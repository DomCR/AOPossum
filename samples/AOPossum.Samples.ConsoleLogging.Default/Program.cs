using AOPossum.Samples.ConsoleLogging.Default.Logging;

namespace AOPossum.Samples.ConsoleLogging.Default
{
	public class Program
	{
		public static void Main(params string[] args)
		{
			ConsoleLoggerMethodLevel methodLevel = new ConsoleLoggerMethodLevel();

			methodLevel.SayHelloWorld();
			methodLevel.SayHelloTo("a possum");
			methodLevel.SayGoodbyeWorld();

			Console.ReadKey();
		}
	}
}