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
			if (type.GetInterface(nameof(IOnEntryMethodBoundary)) == null)
			{
				throw new ArgumentException($"Type {type.FullName} does not implement {nameof(IOnEntryMethodBoundary)}", nameof(type));
			}

			var first = definition.Body.Instructions.First();
			var assembly = definition.Module.Assembly;
			var processor = definition.Body.GetILProcessor();

			//Add the variable current method
			VariableDefinition currentMethod = new VariableDefinition(assembly.MainModule.ImportReference(typeof(MethodBase)));
			definition.Body.Variables.Add(currentMethod);

			VariableDefinition methodArgs = new VariableDefinition(assembly.MainModule.ImportReference(typeof(MethodExecutionArgs)));
			definition.Body.Variables.Add(methodArgs);

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

		public static void AddOnEntryAspectWithParams(this MethodDefinition definition, Type type)
		{
			if (type.GetInterface(nameof(IOnEntryMethodBoundary)) == null)
			{
				throw new ArgumentException($"Type {type.FullName} does not implement {nameof(IOnEntryMethodBoundary)}", nameof(type));
			}

			var first = definition.Body.Instructions.First();
			var assembly = definition.Module.Assembly;
			var processor = definition.Body.GetILProcessor();

			//Add the variable current method
			VariableDefinition currentMethod = new VariableDefinition(assembly.MainModule.ImportReference(typeof(MethodBase)));
			definition.Body.Variables.Add(currentMethod);

			VariableDefinition methodArgs = new VariableDefinition(assembly.MainModule.ImportReference(typeof(MethodExecutionArgs)));
			definition.Body.Variables.Add(methodArgs);

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
