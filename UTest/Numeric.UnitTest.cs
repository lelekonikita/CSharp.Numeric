using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using Leleko.CSharp;
using System.Reflection;

namespace UTest
{

	[TestFixture]
	public abstract class UTest_Parser<T>
	{
		/// <summary>
		/// Stream parser fucntion delegate
		/// </summary>
		protected ParseFunc<T> tryParseFn;
		
		/// <summary>
		/// Initializate delegates fot test
		/// </summary>
		[TestFixtureSetUp]
		public virtual void Init()
		{
			// Get methos NumericParser.TryParse(strin s, ref int index, out T value)
			MethodInfo methodInfo = typeof(NumericParser).GetMethod("TryParse", new Type[]
			                                                        {
				typeof(string),
				typeof(int).MakeByRefType(),
				typeof(T).MakeByRefType()
			});
			Assert.That(methodInfo, Is.Not.Null, "Method TryParse(string,ref int,out {0}) not found", typeof(T));
			this.tryParseFn = Delegate.CreateDelegate(typeof(ParseFunc<T>), methodInfo) as ParseFunc<T>;
			Assert.That(this.tryParseFn, Is.Not.Null, "Delegate {0} not created", typeof(ParseFunc<T>));
		}
	}

	/// <summary>
	/// Template of deletegate to function NumericParser.TryParse[TOuput](string s, out TOutput value)
	/// </summary>
	public delegate bool ParseFuncSingle<TOutput>(string s,out TOutput value);

	public abstract class UTest_NumericParser<T> : UTest_Parser<T>
	{
		/// <summary>
		/// Parser of single value in string
		/// </summary>
		protected ParseFuncSingle<T> tryParseWithIndexFn;

		public override void Init()
		{
			base.Init();

			// Get methos NumericParser.TryParse(strin s, ref int index, out T value)
			MethodInfo methodInfo = typeof(NumericParser).GetMethod("TryParse", new Type[]
			{
				typeof(string),
				typeof(T).MakeByRefType()
			});
			Assert.That(methodInfo, Is.Not.Null, "Method TryParse(string,out {0}) not found", typeof(T));
			this.tryParseWithIndexFn = Delegate.CreateDelegate(typeof(ParseFuncSingle<T>), methodInfo) as ParseFuncSingle<T>;
			Assert.That(this.tryParseWithIndexFn, Is.Not.Null, "Delegate {0} not created", typeof(ParseFuncSingle<T>));

		}

		[Test]
		[TestCaseSource("Source_TryParse_WithIndex")]
		public virtual void TryParse_WithIndex(bool testCheckSuccess, string testString, T testCheckValue, int testIndex, int testCheckIndex)
		{
			T value;
			int oldTestIndex = testIndex;
			bool success = this.tryParseFn(testString as String, ref testIndex, out value);
			Assert.That(success, Is.EqualTo(testCheckSuccess), "Succes is not checked '{0}'!='{1}", success, testCheckSuccess);
			if (success)
			{
				Assert.That(value, Is.EqualTo(testCheckValue).Within(0.0001).Percent, "Parse value not checked '{0}'!='{1}", value, testCheckValue);
				Assert.That(testIndex, Is.EqualTo(testCheckIndex), "Index of cursor not checked '{0}'!='{1}'", testIndex, testCheckIndex);
			}
			else
			{
				Assert.That(testIndex, Is.EqualTo(oldTestIndex), "Index of cursor not checked '{0}'!='{1}'", testIndex, oldTestIndex);
			}
		}

		/// <summary>
		/// source for test
		/// </summary>
		/// <value>The index of the source_ try parse_ with.</value>
		public virtual IEnumerable<ITestCaseData> Source_TryParse_WithIndex
		{
			get
			{
				yield return null;
			}
		}

		[Test]
		[TestCaseSource("Source_TryParse")]
		public virtual void TryParse(bool testCheckSuccess, string testString, T testCheckValue)
		{
			T value;
			bool success = this.tryParseWithIndexFn(testString as String, out value);
			Assert.That(success, Is.EqualTo(testCheckSuccess), "Succes is not checked '{0}'!='{1}", success, testCheckSuccess);
			if (success)
			{
				Assert.That(value, Is.EqualTo(testCheckValue).Within(0.0001).Percent, "Parse value not checked '{0}'!='{1}", value, testCheckValue);
			}
		}

		/// <summary>
		/// source for test
		/// </summary>
		/// <value>The index of the source_ try parse_ with.</value>
		public virtual IEnumerable<ITestCaseData> Source_TryParse
		{
			get
			{
				yield return null;
			}
		}
	}

	public class UTest_NumericParserOfBoolean: UTest_NumericParser<bool>
	{
		string trueString = true.ToString();
		string falseString = false.ToString();
	
		public override IEnumerable<ITestCaseData> Source_TryParse_WithIndex
		{
			get
			{
				yield return new TestCaseData(true, "0", false, 0, 1);
				yield return new TestCaseData(true, "false", false, 0, 5);
				yield return new TestCaseData(true, "False", false, 0, 5);
				yield return new TestCaseData(true, "FALSE", false, 0, 5);
				yield return new TestCaseData(true, "xxfalsexx", false, 2, 7);
				yield return new TestCaseData(true, "1", true, 0, 1);
				yield return new TestCaseData(true, "true", true, 0, 4);
				yield return new TestCaseData(true, "  true  ", true, 0, 6);
				yield return new TestCaseData(true, "True", true, 0, 4);
				yield return new TestCaseData(true, "TRUE", true, 0, 4);
				yield return new TestCaseData(false, "TRuE", true, 0, 0);
				yield return new TestCaseData(true, "4444TRUE", true, 4, 8);

				yield return new TestCaseData(true, trueString, true, 0, trueString.Length);
				yield return new TestCaseData(true, falseString, false, 0, falseString.Length);
			}
		}

		public override IEnumerable<ITestCaseData> Source_TryParse
		{
			get
			{
				yield return new TestCaseData(true, "0", false);
				yield return new TestCaseData(true, "false", false);
				yield return new TestCaseData(true, "False", false);
				yield return new TestCaseData(true, "FALSE", false);
				yield return new TestCaseData(true, "1", true);
				yield return new TestCaseData(true, "true", true);
				yield return new TestCaseData(true, "  True  ", true);
				yield return new TestCaseData(true, "TRUE", true);

				yield return new TestCaseData(true, trueString, true);
				yield return new TestCaseData(true, falseString, false);
			}
		}
	}

	public class UTest_NumericParserOfInteger: UTest_NumericParser<int>
	{
		public override IEnumerable<ITestCaseData> Source_TryParse
		{
			get
			{
				yield return new TestCaseData(true, "0", 0);
				yield return new TestCaseData(true, "-9752", -9752);
				yield return new TestCaseData(true, "   +2223  ", 2223);
				yield return new TestCaseData(true, "-000012", -12);

				yield return new TestCaseData(true, int.MaxValue.ToString(), int.MaxValue);
				yield return new TestCaseData(true, int.MinValue.ToString(), int.MinValue);
			}
		}

		public override IEnumerable<ITestCaseData> Source_TryParse_WithIndex
		{
			get
			{
				yield return new TestCaseData(true, "xx123xxx", 23, 3, 5);
			}
		}
	}

	public class UTest_NumericParserOfLong: UTest_NumericParser<long>
	{
		public override IEnumerable<ITestCaseData> Source_TryParse
		{
			get
			{
				yield return new TestCaseData(true, "0", 0);
				yield return new TestCaseData(true, "-9752", -9752L);
				yield return new TestCaseData(true, "   +2223   ", 2223L);
				yield return new TestCaseData(true, "-000012", -12L);
			
				yield return new TestCaseData(true, long.MaxValue.ToString(), long.MaxValue);
				yield return new TestCaseData(true, long.MinValue.ToString(), long.MinValue);
			}
		}
	
		public override IEnumerable<ITestCaseData> Source_TryParse_WithIndex
		{
			get
			{
				yield return new TestCaseData(true, "xx123xxx", 23L, 3, 5);
			}
		}
	}

	public class UTest_NumericParserOfDouble: UTest_NumericParser<double>
	{
		/// <summary>
		/// Special initialization for use NumberSeparator.All
		/// </summary>
		public override void Init()
		{
			this.tryParseFn = delegate (string s, ref int index, out double value)
			{
				return NumericParser.TryParse(s,ref index, out value, NumericParser.NumberSeparator.All);
			};
		
			this.tryParseWithIndexFn = delegate (string s, out double value)
			{
				return NumericParser.TryParse(s, out value, NumericParser.NumberSeparator.All);
			};
		}

		public override IEnumerable<ITestCaseData> Source_TryParse_WithIndex
		{
			get
			{
				yield return new TestCaseData(true," +1.2e-23dddd",1.2e-23,1,9);
				yield return new TestCaseData(true," NAN",double.NaN,0,4);
				yield return new TestCaseData(true," NaN",double.NaN,0,4);
				yield return new TestCaseData(true," nan",double.NaN,0,4);
			}
		}

		public override IEnumerable<ITestCaseData> Source_TryParse
		{
			get
			{
				yield return new TestCaseData(true,"123.123",123.123);
				yield return new TestCaseData(true,".2323423e-22",.2323423e-22);
				yield return new TestCaseData(true,"   ,2323423e-22   ",.2323423e-22);

				yield return new TestCaseData(true,"   infinity   ",double.PositiveInfinity);
				yield return new TestCaseData(true,"infinity",double.PositiveInfinity);
				yield return new TestCaseData(true,"Infinity",double.PositiveInfinity);
				yield return new TestCaseData(true,"INFINITY",double.PositiveInfinity);
				yield return new TestCaseData(true,"-infinity",double.NegativeInfinity);
				yield return new TestCaseData(true,"-Infinity",double.NegativeInfinity);
				yield return new TestCaseData(true,"-INFINITY",double.NegativeInfinity);

				yield return new TestCaseData(true,double.PositiveInfinity.ToString(),double.PositiveInfinity);
				yield return new TestCaseData(true,double.NegativeInfinity.ToString(),double.NegativeInfinity);
				yield return new TestCaseData(true,double.NaN.ToString(),double.NaN);
			}
		}
	}

	public class UTestNumericParserOfTimeSpan: UTest_NumericParser<TimeSpan>
	{
		public override IEnumerable<ITestCaseData> Source_TryParse
		{
			get
			{
				yield return new TestCaseData(true, "11.12:13:14.15", new TimeSpan(11,12,13,14,15));
				yield return new TestCaseData(true, "11.12:13:14", new TimeSpan(11,12,13,14));
				yield return new TestCaseData(true, "12:13:14", new TimeSpan(12,13,14));
				yield return new TestCaseData(true, TimeSpan.MinValue.ToString(), TimeSpan.MinValue); 
				yield return new TestCaseData(true, TimeSpan.MaxValue.ToString(), TimeSpan.MaxValue);
				yield return new TestCaseData(true, "12345678", new TimeSpan(12345678));
			}
		}
	}
	
	public abstract class UTest_CollectionParser<T>: UTest_Parser<T>
	{
		protected virtual IEnumerable<ITestCaseData> Source
		{
			get
			{
				yield return null;
			}
		}

		[Test]
		[TestCaseSource("Source")]
		public void TryParseCollection(bool testCheckSuccess, string testS, char testSeparator, int testCheckElementsCount, IEnumerable<T> testCheckCollection)
		{
			List<T> outCollection = new List<T>();

			int outElementsCount;
			bool success = CollectionParser.TryParse<T>(testS,testSeparator,this.tryParseFn,out outElementsCount,outCollection);
			Assert.That(success, Is.EqualTo(testCheckSuccess), "Succes is not checked '{0}'!='{1}", success, testCheckSuccess);
			if (success)
			{
				Assert.That(outCollection, Is.EquivalentTo(testCheckCollection), "Parse collection not checked '{0}'!='{1}", outCollection, testCheckCollection);
				Assert.That(outElementsCount, Is.EqualTo(testCheckElementsCount), "Elements count not checked '{0}'!='{1}'", outElementsCount, testCheckElementsCount);
			}
			else
			{
				Assert.That(outElementsCount, Is.EqualTo(testCheckElementsCount), "Elements count not checked '{0}'!='{1}'", outElementsCount, testCheckElementsCount);
			}
		}
	}

	public class UTest_CollectionParserOfBoolean: UTest_CollectionParser<bool>
	{
		protected override IEnumerable<ITestCaseData> Source
		{
			get
			{
				yield return new TestCaseData(false,"1 , 0,1,sdtrue  ",',',3,new bool[]{true,false,true,true}); 
				yield return new TestCaseData(true,"1 , 0,1,true  ",',',4,new bool[]{true,false,true,true}); 
			}
		}
	}

	public class UTest_CollectionParserOfInteger: UTest_CollectionParser<int>
	{
		protected override IEnumerable<ITestCaseData> Source
		{
			get
			{
				yield return new TestCaseData(true," 12 ; 13 ; 55",';',3,new int[]{12,13,55}); 
			}
		}
	}
}
