using AOPossum.Aspects;
using Mono.Cecil.Cil;
using Mono.Cecil;
using System.Reflection;

namespace AOPossum.Engine.Extensions
{
	public static class MethodDefinitionExtension
	{
		public static void AddOnEntryAspect(this MethodDefinition definition, Type type)
		{
			typeCheck<IOnEntryMethodBoundary>(type);
			addBefore(definition.Body.Instructions.First(), definition, type, SymbolExtensions.GetMethodInfo<IOnEntryMethodBoundary>(l => l.OnEntry(null)));
		}


		public static void AddOnExitAspect(this MethodDefinition definition, Type type)
		{
			typeCheck<IOnExitMethodBoundary>(type);

			IEnumerable<Instruction> arr = definition.Body.Instructions.Where(i => i.OpCode == OpCodes.Ret).ToArray();
			foreach (Instruction ins in arr)
			{
				addBefore(ins, definition, type, SymbolExtensions.GetMethodInfo<IOnExitMethodBoundary>(l => l.OnExit(null)));
			}
		}

		private static void addBefore(Instruction first, MethodDefinition definition, Type type, MethodInfo action)
		{
			AssemblyDefinition assembly = definition.Module.Assembly;
			ILProcessor processor = definition.Body.GetILProcessor();

			//Capture the method arguments
			VariableDefinition methodArgs = addMethodArgs(first, definition, processor);

			//Aspect constructor
			TypeReference aspectReference = assembly.MainModule.ImportReference(type);
			MethodReference aspectContructor = assembly.MainModule.ImportReference(type.GetConstructor(new Type[0]));

			processor.InsertBefore(first, Instruction.Create(OpCodes.Newobj, aspectContructor));

			//Aspect method 
			MethodReference actionReference = assembly.MainModule.ImportReference(action);

			processor.InsertBefore(first, Instruction.Create(OpCodes.Ldloc, methodArgs));
			processor.InsertBefore(first, Instruction.Create(OpCodes.Callvirt, actionReference));
		}

		private static VariableDefinition addMethodArgs(Instruction first, MethodDefinition definition, ILProcessor processor)
		{
			AssemblyDefinition assembly = definition.Module.Assembly;

			//Add the variables to the current method
			VariableDefinition methodArgs = new VariableDefinition(assembly.MainModule.ImportReference(typeof(MethodExecutionArgs)));
			definition.Body.Variables.Add(methodArgs);

			VariableDefinition currentMethod = new VariableDefinition(assembly.MainModule.ImportReference(typeof(MethodBase)));
			definition.Body.Variables.Add(currentMethod);

			VariableDefinition arrayDef = new VariableDefinition(new ArrayType(definition.Module.TypeSystem.Object));
			definition.Body.Variables.Add(arrayDef);

			//Get the static method GetCurrentMethod
			processor.InsertBefore(first, Instruction.Create(OpCodes.Call,
				assembly.MainModule.ImportReference(SymbolExtensions.GetMethodInfo(() => MethodBase.GetCurrentMethod()))));
			processor.InsertBefore(first, Instruction.Create(OpCodes.Stloc, currentMethod));

			ConstructorInfo constructorInfo = typeof(MethodExecutionArgs).GetConstructor(new Type[] { typeof(MethodBase), typeof(object[]) });
			MethodReference constructorRef = assembly.MainModule.ImportReference(constructorInfo);

			processor.InsertBefore(first, Instruction.Create(OpCodes.Ldc_I4, definition.Parameters.Count));
			processor.InsertBefore(first, Instruction.Create(OpCodes.Newarr, definition.Module.TypeSystem.Object));
			processor.InsertBefore(first, Instruction.Create(OpCodes.Stloc, arrayDef));

			// loop through the parameters of the method to run
			for (int i = 0; i < definition.Parameters.Count; i++)
			{
				processor.InsertBefore(first, processor.Create(OpCodes.Ldloc, arrayDef));
				processor.InsertBefore(first, processor.Create(OpCodes.Ldc_I4, i));
				processor.InsertBefore(first, processor.Create(OpCodes.Ldarg, i + 1));

				if (definition.Parameters[i].ParameterType.IsValueType)
				{
					processor.InsertBefore(first, processor.Create(OpCodes.Box, definition.Parameters[i].ParameterType));
				}
				else
				{
					processor.InsertBefore(first, processor.Create(OpCodes.Castclass, definition.Module.TypeSystem.Object));
				}
				processor.InsertBefore(first, processor.Create(OpCodes.Stelem_Ref)); // store in the array
			}

			processor.InsertBefore(first, Instruction.Create(OpCodes.Ldloc, currentMethod));
			processor.InsertBefore(first, Instruction.Create(OpCodes.Ldloc, arrayDef));
			processor.InsertBefore(first, Instruction.Create(OpCodes.Newobj, constructorRef));
			processor.InsertBefore(first, Instruction.Create(OpCodes.Stloc, methodArgs));

			return methodArgs;
		}

		private static void typeCheck<T>(Type type)
		{
			if (type.GetInterface(typeof(T).FullName) == null)
			{
				throw new ArgumentException($"Type {type.FullName} does not implement {typeof(T).FullName}", nameof(type));
			}
		}
	}
}
