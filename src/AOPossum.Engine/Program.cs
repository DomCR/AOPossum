using AOPossum.Engine.Core;
using AOPossum.Engine.Loggers;

namespace AOPossum.Engine
{
	public class Program
	{
		public static void Main(params string[] args)
		{
			ConsoleLogger.LogInformation("AOPossum.Engine Start");

			try
			{
#if TEST
				string path = @"..\..\..\..\Test\MockLibrary\bin\Test\net6.0\MockLibrary.dll";
#else
				string path = args.FirstOrDefault();
#endif
				if (string.IsNullOrEmpty(path) || !File.Exists(path))
				{
					throw new ArgumentException("No dll Assembly found", nameof(path));
				}

				string original = saveOriginalAssembly(path);

				Injector bypass = new Injector(path, original);
				bypass.Resolve();

				bypass.Save();
			}
			catch (Exception ex)
			{
				ConsoleLogger.LogCritical("An error ocurred", ex);
			}

			ConsoleLogger.LogInformation("AOPossum.Engine End");
		}

		private static string saveOriginalAssembly(string path)
		{
			ConsoleLogger.LogInformation("Save the original assembly");

			string folder = Path.GetDirectoryName(path);
			string original = Path.Combine(folder, $"{Path.GetFileNameWithoutExtension(path)}.original.dll");
			System.IO.File.Copy(path, original, true);

			ConsoleLogger.LogInformation($"Saved as {original}");

			return original;
		}
	}
}