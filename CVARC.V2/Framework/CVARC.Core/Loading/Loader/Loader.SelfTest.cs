using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CVARC.V2
{
	/// <summary>
	/// This part of the class run self-test in various mode: as an application with GUI, or as a NUnit test.
	/// 
	/// The main problem is to run and correctly handle two threads: the server thread with CVARC competitions, and the client thread that sends commands and receives sensors data.
	/// 
	/// What deepens the problem is that in different modes these threads must be run in different ways:
	/// in Unit Test, client thread must be main and the server must be background, or the asserts won't be displayed correctly.
	/// in an application with GUI, server thread must be main and client must be background, otherwise the drawing won't work.
	/// </summary>
	partial class Loader
	{

		/// <summary>
		/// Gets the test with the specific name
		/// </summary>
		/// <param name="data"></param>
		/// <param name="testName"></param>
		/// <returns></returns>
		public ICvarcTest GetTest(LoadingData data, string testName)
		{
			var assemblyName = data.AssemblyName;
			var level = data.Level;
			Competitions competitions;
			try
			{
				competitions = GetCompetitions(assemblyName, level);
			}
			catch
			{
				throw new Exception(string.Format("The competition '{0}'.'{1}' were not found", assemblyName, level));
			}
			ICvarcTest test;
			try
			{
				test = competitions.Logic.Tests[testName];
			}
			catch
			{
				throw new Exception(string.Format("The test with name '{0}' was not found in competitions {1}.{2}", testName, assemblyName, level));
			}
			return test;
		}

		/// <summary>
		/// The entry point for a client that performs a test
		/// </summary>
		/// <param name="test"></param>
		/// <param name="asserter"></param>
		/// <param name="holder"></param>
		void SelfTestClientThread(ICvarcTest test, IAsserter asserter, NetworkServerData holder)
		{
			holder.WaitForServer();
			test.Run(holder, asserter);
		}

		
		/// <summary>
		/// The entry point for a server
		/// </summary>
		/// <param name="holder"></param>
		/// <param name="proposal"></param>
		public void CreateSelfTestServer(NetworkServerData holder, SettingsProposal proposal)
		{
			RunServer(holder);
			ReceiveConfiguration(holder);
			proposal.Push(holder.Settings, true);
			InstantiateWorld(holder);
		}

		/// <summary>
		/// Runs the test as an application with GUI
		/// </summary>
		/// <param name="data"></param>
		/// <param name="asserter"></param>
		/// <returns></returns>
		public IWorld CreateSelfTestInCommandLineContext(CommandLineData data, IAsserter asserter)
		{
			var holder = new NetworkServerData();
			holder.Port = DefaultPort;
			holder.LoadingData = new LoadingData { AssemblyName = data.Unnamed[0], Level = data.Unnamed[1] };
			var test = GetTest(holder.LoadingData, data.Unnamed[3]);
			new Action<ICvarcTest, IAsserter, NetworkServerData>(SelfTestClientThread).BeginInvoke(test, asserter, holder, null, null);
			var proposal = SettingsProposal.FromCommandLineData(data);
			CreateSelfTestServer(holder, proposal);
			return holder.World;
		}

		static Random rnd = new Random();

		/// <summary>
		/// Runs the test as a Unit test.
		/// </summary>
		/// <param name="assemblyName"></param>
		/// <param name="level"></param>
		/// <param name="testName"></param>
		/// <param name="asserter"></param>
		public void RunSelfTestInVSContext(string assemblyName, string level, string testName, IAsserter asserter)
		{
			var holder = new NetworkServerData();
			holder.Port = DefaultPort;
			holder.LoadingData = new LoadingData { AssemblyName = assemblyName, Level = level };
			var test = GetTest(holder.LoadingData, testName);

			var thread = new Thread(() =>
			{
				var proposal = new SettingsProposal { SpeedUp = true };
				CreateSelfTestServer(holder, proposal);
				holder.World.RunActively(1);
				holder.Close();
			}) { IsBackground = true };
			thread.Start();

			try
			{
				SelfTestClientThread(test, asserter, holder);
				
			}
			catch (Exception e)
			{
				throw e;
			}
			finally
			{
				holder.Close();
			}
		}
	}
}
