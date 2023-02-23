using AOPossum.Aspects;
using Xunit;

namespace AOPossum.Tests.Aspects
{
	public class MethodExecutionArgsTests
	{
		[Fact]
		public void CreateTest()
		{
			MethodExecutionArgs args = MethodExecutionArgs.Create();
			Assert.Equal(nameof(CreateTest), args.MethodBase.Name);
		}
	}
}
