using AOPossum.Aspects;
using System;

namespace AOPossum.Guards
{
	public class InRangeAttribute : Aspect
	{
		public void Validate(object parameter)
		{
			if (parameter == null)
			{
				throw new ArgumentNullException();
			}
		}
	}
}
