using System;
using System.Diagnostics;
using Leleko.CSharp;
using System.Collections.Generic;

namespace UTest
{
	public class SpeedTest
	{

		public static void Main(params string[] args)
		{
			Stopwatch sw = new Stopwatch();
			
			string s;
			const int repeatCount = (int)1e+7;

			bool success = default(bool);
			bool boolValue = default(bool);
			int intValue = default(int);
			long longValue = default(long);
			double doubleValue = default(double);
			TimeSpan timeSpan = default(TimeSpan);
			DateTime dateTime = default(DateTime);


			Console.WriteLine(DateTime.MaxValue);
			return;

			Console.WriteLine("Speed Testing");

			s = "True";

			sw.Reset();
			sw.Start();
			for (int i=0;i<repeatCount;i++)
			{
				success = bool.TryParse(s,out boolValue);
			}
			sw.Stop();
			Console.WriteLine("bool.TryParse(out bool) sucess = {0} timeMilliseconds = {1}",success,sw.ElapsedMilliseconds);

			sw.Reset();
			sw.Start();
			for (int i=0;i<repeatCount;i++)
			{
				success = NumericParser.TryParse(s,out boolValue);
			}
			sw.Stop();
			Console.WriteLine("NumericParser.TryParse(out bool) sucess = {0} timeMilliseconds = {1}",success,sw.ElapsedMilliseconds);

			s = "123456789";
			
			sw.Reset();
			sw.Start();
			for (int i=0;i<repeatCount;i++)
			{
				success = int.TryParse(s,out intValue);
			}
			sw.Stop();
			Console.WriteLine("int.TryParse(out int) sucess = {0} timeMilliseconds = {1}",success,sw.ElapsedMilliseconds);
			
			sw.Reset();
			sw.Start();
			for (int i=0;i<repeatCount;i++)
			{
				success = NumericParser.TryParse(s,out intValue);
			}
			sw.Stop();
			Console.WriteLine("NumericParser.TryParse(out int) sucess = {0} timeMilliseconds = {1}",success,sw.ElapsedMilliseconds);



			s = "123456789123456";
			
			sw.Reset();
			sw.Start();
			for (int i=0;i<repeatCount;i++)
			{
				success = long.TryParse(s,out longValue);
			}
			sw.Stop();
			Console.WriteLine("long.TryParse(out long) sucess = {0} timeMilliseconds = {1}",success,sw.ElapsedMilliseconds);
			
			sw.Reset();
			sw.Start();
			for (int i=0;i<repeatCount;i++)
			{
				success = NumericParser.TryParse(s,out longValue);
			}
			sw.Stop();
			Console.WriteLine("NumericParser.TryParse(out long) sucess = {0} timeMilliseconds = {1}",success,sw.ElapsedMilliseconds);

			s = "12345.56789e-23";

			sw.Reset();
			sw.Start();
			for (int i=0;i<repeatCount;i++)
			{
				success = double.TryParse(s,out doubleValue);
			}
			sw.Stop();
			Console.WriteLine("double.TryParse(out double) sucess = {0} timeMilliseconds = {1}, value = {2}",success,sw.ElapsedMilliseconds,doubleValue);

			sw.Reset();
			sw.Start();
			for (int i=0;i<repeatCount;i++)
			{
				success = NumericParser.TryParse(s,out doubleValue, NumericParser.NumberSeparator.Point);
			}
			sw.Stop();
			Console.WriteLine("NumericParser.TryParse(out double) sucess = {0} timeMilliseconds = {1}, value = {2}",success,sw.ElapsedMilliseconds,doubleValue);

			s = "11.11:11:11.11";
			
			sw.Reset();
			sw.Start();
			for (int i=0;i<repeatCount;i++)
			{
				success = TimeSpan.TryParse(s,out timeSpan);
			}
			sw.Stop();
			Console.WriteLine("TimeSpan.TryParse(out bool) sucess = {0} timeMilliseconds = {1}",success,sw.ElapsedMilliseconds);
			
			sw.Reset();
			sw.Start();
			for (int i=0;i<repeatCount;i++)
			{
				success = NumericParser.TryParse(s,out timeSpan);
			}
			sw.Stop();
			Console.WriteLine("NumericParser.TryParse(out TimeSpan) sucess = {0} timeMilliseconds = {1}",success,sw.ElapsedMilliseconds);

			Console.ReadKey();
		}
	}
}

