using AOPossum.Aspects;
using System.Text;

namespace AOPossum.Logging
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
	public class ConsoleLogAttribute : Aspect, IOnEntryMethodBoundary
	{
		public void LogEntry()
		{
			Console.WriteLine("Hello I'm at the entry of a method");
		}

		public void OnEntry(MethodExecutionArgs args)
		{
			StringBuilder str = new StringBuilder();
			str.Append("TRACE");

			Console.WriteLine($"OnEntry executed in : {args.MethodBase.Name}");
		}

		public void LogExit()
		{
			Console.WriteLine("Goodbye I'm at the exit of a method");
		}
	}
}
