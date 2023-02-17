namespace AOPossum.Engine.Loggers
{
	public static class ConsoleLogger
	{
		public static void LogInformation(string message)
		{
			Console.WriteLine($"INFO:	{message}");
		}

		public static void LogWarning(string message, Exception ex = null)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine($"WARN:	{message}");
			Console.ResetColor();
		}

		public static void LogError(string message, Exception ex = null)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"ERROR:	{message}");
			Console.ResetColor();
		}

		public static void LogCritical(string message, Exception ex = null)
		{
			Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.WriteLine($"CRIT:	{message}");
			Console.ResetColor();
		}
	}
}
