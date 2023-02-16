namespace AOPossum.Aspects
{
	/// <summary>
	/// Represents a class that can wrap itself around any given method call.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public abstract class OnMethodBoundaryAttribute : Aspect
	{
		public virtual void OnEntry() { }

		public virtual void OnEntry(MethodExecutionArgs args) { }

		public virtual void OnExit() { }

		public virtual void OnExit(MethodExecutionArgs args) { }
	}

	public interface IOnMethodBoundary
	{
		void OnEntry(MethodExecutionArgs args);
	}
}
