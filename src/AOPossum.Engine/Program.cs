namespace AOPossum.Engine
{
	public class Program
	{
		public static void Main(params string[] args)
		{
			Console.WriteLine("OtterSharp.Engine Start");

			try
			{
				string path = "";
				saveOriginalAssembly(path);

			}
			catch (Exception ex)
			{
				Console.Error.WriteLine(ex);
			}
			finally
			{
				Console.ResetColor();
			}

			Console.WriteLine("OtterSharp.Engine End");
		}

		private static string saveOriginalAssembly(string path)
		{
			//Save the original assembly
			string folder = Path.GetDirectoryName(path);
			string old = Path.Combine(folder, $"{Path.GetFileNameWithoutExtension(path)}.original.dll");
			System.IO.File.Copy(path, old, true);

			return old;
		}
	}
}