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

	public static void RunDemo(string testName)
	{
		var loader = new Loader();
		loader.AddLevel("Demo", "Demo", () => new Demo.DemoCompetitions());
		loader.RunSelfTestInVSContext("Demo", "Demo", testName, new NUnitAsserter());
	}
    public static void RunDWM(string testName)
    {
        var loader = new Loader();
        loader.AddLevel("Demo", "DWM", () => new Demo.DWMCompetitions());
        loader.RunSelfTestInVSContext("Demo", "DWM", testName, new NUnitAsserter());
    }
	public static void Main()
	{
		new Movement.Round().Forward();
	}
}

