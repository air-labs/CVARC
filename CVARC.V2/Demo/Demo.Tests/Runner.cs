using CVARC.V2;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NUnitAsserter : IAsserter
{
	public void IsEqual(double expected, double actual, double delta)
	{
		Assert.AreEqual(expected, actual, delta);
	}

	public void IsEqual(bool expected, bool actual)
	{
		Assert.AreEqual(expected, actual);
	}
}


public static class TestRunner
{

	public static void Run(string testName)
	{
		var loader = new Loader();
		loader.AddLevel("Demo", "Demo", () => new Demo.KroR.DemoCompetitions());
		loader.RunSelfTestInVSContext("Demo", "Demo", testName, new NUnitAsserter());
	}
	public static void Main()
	{
		new Interaction.Rect().Alignment();
	}
}