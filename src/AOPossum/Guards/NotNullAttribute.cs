using AOPossum.Aspects;

namespace AOPossum.Guards
{
	internal class NotNullAttribute : Aspect
	{
		public void Validate(object parameter)
		{
			if(parameter == null)
			{
				throw new ArgumentNullException();
			}
		}
	}
}
