using Mono.Cecil;
using System.Reflection;

namespace AOPossum.Engine.Extensions
{
	public static class TypeDefinitionExtensions
	{
		public static MethodDefinition GetMethod(this TypeDefinition type, MethodInfo info)
		{
			return type.Methods.FirstOrDefault(m => m.FullName == info.GetFullName());
		}
	}
}
