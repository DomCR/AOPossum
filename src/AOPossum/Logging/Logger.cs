using AOPossum.Aspects;

namespace AOPossum.Logging
{
	public class Logger
	{
		public static Logger Instance = new Logger();

		public void OnEntry(MethodExecutionArgs methodArgs)
		{
			Console.WriteLine($"Hello Entering in: {methodArgs.MethodBase.Name}");
		}
	}
}
