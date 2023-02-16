using AOPossum.Aspects;

namespace AOPossum.Logging
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
	public class ConsoleLogAttribute : Aspect
	{
		public void LogEntry()
		{
			Console.WriteLine("Hello I'm at the entry of a method");
		}

		public void LogExit()
		{
			Console.WriteLine("Goodbye I'm at the exit of a method");
		}
	}
}
