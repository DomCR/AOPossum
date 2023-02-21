using AOPossum.Aspects;
using AOPossum.Engine.Extensions;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using System.IO;
using System.Reflection;

namespace AOPossum.Engine.Core
{
	public class Injector
	{
		private readonly static ModuleDefinition _possum = AssemblyDefinition.ReadAssembly("AOPossum.dll").MainModule;
		private string _path;
		private Assembly _assembly;
		private AssemblyDefinition _assemblyDefinition;

		public Injector(string assemblyPath, string original)
		{
			this._path = assemblyPath;

			this._assembly = Assembly.LoadFile(Path.GetFullPath(original));
			this._assemblyDefinition = AssemblyDefinition.ReadAssembly(new MemoryStream(File.ReadAllBytes(assemblyPath)));
		}

		public void Resolve()
		{
			resolveAssembly();

			foreach (TypeInfo ti in this._assembly.DefinedTypes)
			{
				resolveObject(ti);
			}

			foreach (TypeDefinition td in this._assemblyDefinition.MainModule.Types)
			{
				foreach (var aspect in td.CustomAttributes)
				{

				}
			}
		}

		public void Save()
		{
			this._assemblyDefinition.Write(this._path);
		}

		private void resolveAssembly()
		{
			IEnumerable<Aspect> assemblyAspects = this._assembly.GetCustomAttributes<Aspect>();

			Mono.Collections.Generic.Collection<CustomAttribute> customAtts = this._assemblyDefinition.CustomAttributes;
		}

		private void resolveObject(TypeInfo typeInfo)
		{
			TypeDefinition tdefinition = this._assemblyDefinition.MainModule.GetType(typeInfo.FullName);

			List<Aspect> typeAspects = new List<Aspect>();
			typeAspects.AddRange(typeInfo.GetCustomAttributes<Aspect>());

			foreach (MethodInfo m in typeInfo.GetMethods())
			{
				List<Aspect> methodAspects = new List<Aspect>(m.GetCustomAttributes<Aspect>());
				if (!methodAspects.Any())
					continue;

				MethodDefinition mdef = tdefinition.GetMethod(m);

				foreach (var item in methodAspects)
				{
					if (item is IOnEntryMethodBoundary)
					{
						mdef.AddOnEntryAspect(item.GetType());
					}
				}
			}
		}

		private void resolveMethod(MethodInfo m)
		{

		}
	}
}
