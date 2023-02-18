using AOPossum.Aspects;
using Mono.Cecil.Cil;
using Mono.Cecil;
using System.Reflection;

namespace AOPossum.Engine.Extensions
{
	public static class MethodDefinitionExtensions
	{
		public static void AddOnEntryAspect(this MethodDefinition mdefinition, Type type)
		{
			var first = mdefinition.Body.Instructions.First();
			var assembly = mdefinition.Module.Assembly;
			var processor = mdefinition.Body.GetILProcessor();

			//Add the variable current method
			VariableDefinition currentMethod = new VariableDefinition(assembly.MainModule
				.ImportReference(typeof(MethodBase)));
			mdefinition.Body.Variables.Add(currentMethod);

			VariableDefinition methodArgs = new VariableDefinition(assembly.MainModule
				.ImportReference(typeof(MethodExecutionArgs)));
			mdefinition.Body.Variables.Add(methodArgs);

			//Get the static method GetCurrentMethod
			processor.InsertBefore(first, Instruction.Create(OpCodes.Call,
				assembly.MainModule.ImportReference(SymbolExtensions.GetMethodInfo(() => MethodBase.GetCurrentMethod()))));
			processor.InsertBefore(first, Instruction.Create(OpCodes.Stloc, currentMethod));

			ConstructorInfo constructorInfo = typeof(MethodExecutionArgs).GetConstructor(new Type[] { typeof(MethodBase) });
			MethodReference constructorRef = assembly.MainModule.ImportReference(constructorInfo);
			processor.InsertBefore(first, Instruction.Create(OpCodes.Ldloc, currentMethod));
			processor.InsertBefore(first, Instruction.Create(OpCodes.Newobj, constructorRef));
			processor.InsertBefore(first, Instruction.Create(OpCodes.Stloc, methodArgs));

			//Class : logging
			TypeReference loggerRef = assembly.MainModule.ImportReference(type);
			MethodReference loggerConstructor = assembly.MainModule.ImportReference(type.GetConstructor(new Type[0]));

			processor.InsertBefore(first, Instruction.Create(OpCodes.Newobj, loggerConstructor));

			//Method : OnEntry
			MethodReference onEntryRef = assembly.MainModule.ImportReference(SymbolExtensions.GetMethodInfo<IOnEntryMethodBoundary>(l => l.OnEntry(null)));

			processor.InsertBefore(first, Instruction.Create(OpCodes.Ldloc, methodArgs));
			processor.InsertBefore(first, Instruction.Create(OpCodes.Callvirt, onEntryRef));
		}
	}
}
