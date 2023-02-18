using Mono.Cecil;
using System.Reflection;

namespace AOPossum.Engine.Extensions
{
	public static class MethodInfoExtension
	{
		public static string GetFullName(this MethodInfo method)
		{
			string name = $"{method.ReturnType.FullName} {method.DeclaringType.FullName}::{method.Name}(";

			foreach (ParameterInfo p in method.GetParameters())
			{
				name += $"{p.ParameterType.FullName},";

				if (method.GetParameters().Last() == p)
				{
					name = name.Remove(name.Length - 1);
				}
			}
			name += ")";

			return name;
		}

		public static MethodDefinition ToDefinition(this MethodInfo method)
		{
			MethodDefinition methodDefinition = new MethodDefinition
				(
				method.Name,
				(Mono.Cecil.MethodAttributes)method.Attributes,
				method.ReturnType.ToDefinition()
				);

			return methodDefinition;
		}
	}
}
