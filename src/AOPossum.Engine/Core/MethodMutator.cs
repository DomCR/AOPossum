using AOPossum.Aspects;
using AOPossum.Engine.Extensions;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Reflection;

namespace AOPossum.Engine.Core
{
	[Obsolete("Move to MethodDefinitionExtensions")]
	public class MethodMutator
	{
		public MethodDefinition Definition { get; }

		public MethodMutator(MethodDefinition definition)
		{
			Definition = definition;
		}

		public void AddOnEntryAspect(Type type)
		{
			var first = Definition.Body.Instructions.First();
			var assembly = Definition.Module.Assembly;
			var processor = Definition.Body.GetILProcessor();

			//Add the variable current method
			VariableDefinition currentMethod = new VariableDefinition(assembly.MainModule
				.ImportReference(typeof(MethodBase)));
			Definition.Body.Variables.Add(currentMethod);

			VariableDefinition methodArgs = new VariableDefinition(assembly.MainModule
				.ImportReference(typeof(MethodExecutionArgs)));
			Definition.Body.Variables.Add(methodArgs);

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

		public void AddOnEntryAspect<T>()
			where T : IOnEntryMethodBoundary
		{
			var first = Definition.Body.Instructions.First();
			var assembly = Definition.Module.Assembly;
			var processor = Definition.Body.GetILProcessor();

			//Add the variable current method
			VariableDefinition currentMethod = new VariableDefinition(assembly.MainModule
				.ImportReference(typeof(MethodBase)));
			Definition.Body.Variables.Add(currentMethod);

			VariableDefinition methodArgs = new VariableDefinition(assembly.MainModule
				.ImportReference(typeof(MethodExecutionArgs)));
			Definition.Body.Variables.Add(methodArgs);

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
			TypeReference loggerRef = assembly.MainModule.ImportReference(typeof(T));
			MethodReference loggerConstructor = assembly.MainModule.ImportReference(typeof(T).GetConstructor(new Type[0]));

			processor.InsertBefore(first, Instruction.Create(OpCodes.Newobj, loggerConstructor));

			//Method : OnEntry
			MethodReference onEntryRef = assembly.MainModule.ImportReference(
					SymbolExtensions.GetMethodInfo<T>(l => l.OnEntry(null)));

			processor.InsertBefore(first, Instruction.Create(OpCodes.Ldloc, methodArgs));
			processor.InsertBefore(first, Instruction.Create(OpCodes.Callvirt, onEntryRef));
		}
	}
}
