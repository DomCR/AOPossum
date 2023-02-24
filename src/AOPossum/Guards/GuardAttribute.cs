using AOPossum.Aspects;
using System;

namespace AOPossum.Guards
{
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = true)]
	internal abstract class GuardAttribute : Aspect, IGuard
	{
		public abstract void Validate(GuardArgs args);
	}

	public interface IGuard
	{
		void Validate(GuardArgs args);
	}

	public class GuardArgs
	{
		public object Value { get; }
	}
}
