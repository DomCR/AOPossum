using System.Runtime.Loader;

namespace AOPossum.Tests.Common
{
	public class CollectibleAssemblyLoadContext : AssemblyLoadContext
	{
		public CollectibleAssemblyLoadContext() : base(isCollectible: true) { }
	}
}
