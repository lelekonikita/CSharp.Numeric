using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using Leleko.CSharp;
using System.Reflection;

/// <summary>
/// Template of deletegate to function NumericParser.TryParse[TOuput](string s, out TOutput value)
/// </summary>

/// <summary>
/// Base Test class
/// </summary>
[TestFixture]
public abstract class NumericParserTest<T>
{
	/// <summary>
	/// Stream parser fucntion delegate
	/// </summary>
	protected ParseFunc<T> tryParseStreamFn;

	/// <summary>
	/// Parser of single value in string
	/// </summary>
	protected ParseFuncSingle<T> tryParseSingleFn;

	/// <summary>
	/// Initializate delegates fot test
	/// </summary>
	[TestFixtureSetUp]
	public virtual void Init()
	{
		MethodInfo methodInfo = null;

		// Get methos NumericParser.TryParse(strin s, ref int index, out T value)
		methodInfo = typeof(NumericParser).GetMethod("TryParse",new Type[]{typeof(string),typeof(int).MakeByRefType(),typeof(T).MakeByRefType()});
		Assert.That(methodInfo,Is.Not.Null,"Method TryParse(string,ref int,out {0}) not found",typeof(T));
		this.tryParseStreamFn = Delegate.CreateDelegate(typeof(ParseFunc<T>),methodInfo) as ParseFunc<T>;
		Assert.That(this.tryParseStreamFn,Is.Not.Null,"Delegate {0} not created",typeof(ParseFunc<T>));

		// Get methos NumericParser.TryParse(strin s, ref int index, out T value)
		methodInfo = typeof(NumericParser).GetMethod("TryParse",new Type[]{typeof(string),typeof(T).MakeByRefType()});
		Assert.That(methodInfo,Is.Not.Null,"Method TryParse(string,out {0}) not found",typeof(T));
		this.tryParseSingleFn = Delegate.CreateDelegate(typeof(ParseFuncSingle<T>),methodInfo) as ParseFuncSingle<T>;
		Assert.That(this.tryParseStreamFn,Is.Not.Null,"Delegate {0} not created",typeof(ParseFuncSingle<T>));
	}

	[Test]
	public virtual void TryParseStream(bool testCheckSuccess, object testString, T testCheckValue, int testIndex, int testCheckIndex)
	{
		if (!(testString is String)) testString = testString.ToString();

		T value;
		int oldTestIndex = testIndex;
		bool success = this.tryParseStreamFn(testString as String, ref testIndex, out value);
		Assert.That(success,Is.EqualTo(testCheckSuccess),"Succes is not checked '{0}'!='{1}",success,testCheckSuccess);
		if (success)
		{
			Assert.That(value,Is.EqualTo(testCheckValue).Within(0.0001).Percent,"Parse value not checked '{0}'!='{1}",value,testCheckValue);
			Assert.That(testIndex,Is.EqualTo(testCheckIndex),"Index of cursor not checked '{0}'!='{1}'",testIndex,testCheckIndex);
		}
		else
		{
			Assert.That(testIndex,Is.EqualTo(oldTestIndex),"Index of cursor not checked '{0}'!='{1}'",testIndex,oldTestIndex);
		}
	}

	[Test]
	public virtual void TryParseSingle(bool testCheckSuccess, object testString, T testCheckValue)
	{
		if (!(testString is String)) testString = testString.ToString();

		T value;
		bool success = this.tryParseSingleFn(testString as String, out value);
		Assert.That(success,Is.EqualTo(testCheckSuccess),"Succes is not checked '{0}'!='{1}",success,testCheckSuccess);
		if (success)
		{
			Assert.That(value,Is.EqualTo(testCheckValue).Within(0.0001).Percent,"Parse value not checked '{0}'!='{1}",value,testCheckValue);
		}
	}
}
