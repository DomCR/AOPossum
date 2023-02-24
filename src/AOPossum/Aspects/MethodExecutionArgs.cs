using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace AOPossum.Aspects
{
	public class MethodExecutionArgs
	{
		public MethodBase MethodBase { get; }

		public IEnumerable<object> Parameters { get; } = new List<object>();

		public object ReturnValue { get; }

		public MethodExecutionArgs(MethodBase method)
		{
			this.MethodBase = method;
		}

		public MethodExecutionArgs(MethodBase method, object[] args)
		{
			this.MethodBase = method;
			this.Parameters = new List<object>(args);
		}

		public MethodExecutionArgs(MethodBase method, object[] args, object retValue) : this(method, args)
		{
			this.MethodBase = method;
			this.Parameters = new List<object>(args);
			this.ReturnValue = retValue;
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