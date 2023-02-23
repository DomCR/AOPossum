using System;

namespace MockLibrary
{
	public class MethodAspectTest
	{
		public void HelloWorld()
		{
			Console.WriteLine("Hello World!!");
		}

		public void HelloWorld(string text)
		{
			Console.WriteLine(text);
		}
	}
}