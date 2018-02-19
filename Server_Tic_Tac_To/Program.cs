/*
 * Created by SharpDevelop.
 * User: luis_nikolai
 * Date: 19.02.2018
 * Time: 18:44
 * 
 */
using System;

namespace Server_Tic_Tac_To
{
	class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			
			// TODO: Implement Functionality Here
			TServer server = new TServer();
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}