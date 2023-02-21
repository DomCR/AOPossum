using AOPossum.Aspects;
using System.Text;

namespace AOPossum.Logging
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
	public class ConsoleLogAttribute : Aspect, IOnEntryMethodBoundary
	{
		public void OnEntry(MethodExecutionArgs args)
		{
			StringBuilder str = new StringBuilder();
			str.Append("INFO");
			str.Append(" | ");
			str.Append(args.MethodBase.Name);

			if (args.Parameters.Any())
			{
				for (int i = 0; i < args.Parameters.Count(); i++)
				{
					str.Append(" | ");
					str.Append($"[{args.MethodBase.GetParameters()[i].ParameterType.FullName}] {args.Parameters.ElementAt(i)}");
				}
			}

			str.Append(" | ");
			str.Append("START");

			Console.WriteLine(str);
		}

		public void LogExit()
		{
			Console.WriteLine("Goodbye I'm at the exit of a method");
		}
	}
}
