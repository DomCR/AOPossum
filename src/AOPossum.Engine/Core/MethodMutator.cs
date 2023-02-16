using AOPossum.Aspects;
using AOPossum.Engine.Extensions;
using AOPossum.Logging;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Reflection;

namespace AOPossum.Engine.Core
{
	public class MethodMutator
	{
		public MethodDefinition Definition { get; }

		public MethodMutator(MethodDefinition definition)
		{
			Definition = definition;
		}

		public void AddOnEntry(MethodDefinition pre)
		{

		}

		public static void CreateMethodArgs(MethodDefinition original)
		{
			var first = original.Body.Instructions.First();
			var assembly = original.Module.Assembly;
			var processor = original.Body.GetILProcessor();

			//Add the variable current method
			VariableDefinition currentMethod = new VariableDefinition(assembly.MainModule
				.ImportReference(typeof(MethodBase)));
			original.Body.Variables.Add(currentMethod);

			VariableDefinition methodArgs = new VariableDefinition(assembly.MainModule
				.ImportReference(typeof(MethodExecutionArgs)));
			original.Body.Variables.Add(methodArgs);

			//Get the static method GetCurrentMethod
			processor.InsertBefore(first, Instruction.Create(OpCodes.Call,
				assembly.MainModule.ImportReference(SymbolExtensions.GetMethodInfo(
					() => MethodBase.GetCurrentMethod()))));
			processor.InsertBefore(first, Instruction.Create(OpCodes.Stloc, currentMethod));

			ConstructorInfo constructorInfo = typeof(MethodExecutionArgs).GetConstructor(new Type[] { typeof(MethodBase) });
			MethodReference constructorRef = assembly.MainModule.ImportReference(constructorInfo);
			processor.InsertBefore(first, Instruction.Create(OpCodes.Ldloc, currentMethod));
			processor.InsertBefore(first, Instruction.Create(OpCodes.Newobj, constructorRef));
			processor.InsertBefore(first, Instruction.Create(OpCodes.Stloc, methodArgs));

			//Class : Logger
			var loggerRef = assembly.MainModule.ImportReference(typeof(Logger));
			var loggerInstance = assembly.MainModule.ImportReference(typeof(Logger).GetField("Instance"));

			processor.InsertBefore(first, Instruction.Create(OpCodes.Ldsfld, loggerInstance));

			//Method : OnEntry
			MethodReference onEntryRef = assembly.MainModule.ImportReference(
					SymbolExtensions.GetMethodInfo<Logger>(l => l.OnEntry(null))
				);
			processor.InsertBefore(first, Instruction.Create(OpCodes.Ldloc, methodArgs));
			processor.InsertBefore(first, Instruction.Create(OpCodes.Callvirt, onEntryRef));
		}

		public static void CreateMethodArgs<T>(MethodDefinition original)
			where T : IOnMethodBoundary
		{
			var first = original.Body.Instructions.First();
			var assembly = original.Module.Assembly;
			var processor = original.Body.GetILProcessor();

			//Add the variable current method
			VariableDefinition currentMethod = new VariableDefinition(assembly.MainModule
				.ImportReference(typeof(MethodBase)));
			original.Body.Variables.Add(currentMethod);

			VariableDefinition methodArgs = new VariableDefinition(assembly.MainModule
				.ImportReference(typeof(MethodExecutionArgs)));
			original.Body.Variables.Add(methodArgs);

			//Get the static method GetCurrentMethod
			processor.InsertBefore(first, Instruction.Create(OpCodes.Call,
				assembly.MainModule.ImportReference(SymbolExtensions.GetMethodInfo(
					() => MethodBase.GetCurrentMethod()))));
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
