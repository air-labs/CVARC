using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CVARC.V2
{

    partial class Loader
    {
        
		/// <summary>
		/// The method that creates the world, according to command line arguments.
		/// </summary>
		/// <param name="arguments"></param>
		/// <returns></returns>
        public IWorld Load(string[] arguments)
        {
            var cmdLineData = CommandLineData.Parse(arguments);
            
            if (cmdLineData.Unnamed.Count==0)
                throw new Exception("CVARC required parameters to run. See manual");

            if (cmdLineData.Unnamed[0] == "Debug")
                return CreateSoloNetwork(cmdLineData);
            else if (cmdLineData.Unnamed.Count == 1)
                return CreateLogPlayer(cmdLineData);
            else if (cmdLineData.Unnamed.Count == 4 && cmdLineData.Unnamed[2] == "SelfTest")
                return CreateSelfTestInCommandLineContext(cmdLineData, new EmptyAsserter());
            else
                return CreateSimpleMode(cmdLineData);
        }

        public static string Help = @"

";


    }
}
