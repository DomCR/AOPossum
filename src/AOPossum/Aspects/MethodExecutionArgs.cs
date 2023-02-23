using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace AOPossum.Aspects
{
	public class MethodExecutionArgs
	{
		public MethodBase MethodBase { get; }

		public IEnumerable<object> Parameters { get; } = new List<object>();

		public MethodExecutionArgs(MethodBase method)
		{
			MethodBase = method;
		}

		public MethodExecutionArgs(MethodBase method, params object[] args)
		{
			MethodBase = method;
			Parameters = new List<object>(args);
		}

		public static MethodExecutionArgs Create()
		{
			// TODO: MethodExecutionArgs Create() is it needed??

			StackTrace stackTrace = new StackTrace();
			StackFrame frame = stackTrace.GetFrame(1);
			
			MethodBase m = frame.GetMethod();
			return new MethodExecutionArgs(m);
		}
	}
}