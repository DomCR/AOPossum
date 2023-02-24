using AOPossum.Aspects;
using System;
using System.Linq;
using System.Text;

namespace AOPossum.Logging
{
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public class ConsoleLogAttribute : Aspect, IOnEntryMethodBoundary, IOnExitMethodBoundary
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

		public void OnExit(MethodExecutionArgs args)
		{
			throw new NotImplementedException();
		}
	}
}
