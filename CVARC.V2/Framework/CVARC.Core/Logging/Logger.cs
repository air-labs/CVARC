using AIRLab.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace CVARC.V2
{
    [Serializable]
     public class PositionLogItem
    {
        public double Time { get; set; }
        public Frame3D Location { get; set; }
        public bool Exists { get; set; }
    }



    public class Logger
    {
        public double LoggingDeltaTime = 0.1;
        public string LogFileName { get; set; }
        public bool SaveLog { get; set; }
        public Log Log { get; private set; }
        IWorld world;
        public Logger(IWorld world)
        {
            this.world = world;
            Log=new V2.Log();
            //world.Clocks.AddTrigger(new TimerTrigger(UpdatePositions, LoggingDeltaTime));
            world.Exit += world_Exit;
        }

        void world_Exit()
        {
            if (!SaveLog) return;
            string filename=world.Configuration.Settings.LogFile;
            if (filename == null)
            {
                for (int i = 0; ; i++)
                {
                    filename = "log" + i + ".cvarclog";
                    if (!File.Exists(filename))
                        break;
                }
            }
            Log.Save(filename);
        }

        public void AccountCommand(string controllerId, ICommand command)
        {
            if (!Log.Commands.ContainsKey(controllerId))
                Log.Commands[controllerId] = new List<ICommand>();
            Log.Commands[controllerId].Add(command);
        }

        void UpdatePositions(double tick)
        {
            foreach (var e in world.IdGenerator.GetAllId())
            {
                if (!Log.Positions.ContainsKey(e))
                    Log.Positions[e] = new List<PositionLogItem>();
                var item = new PositionLogItem { Time = tick };
                if (!world.Engine.ContainBody(e))
                    item.Exists = false;
                else
                {
                    item.Exists = true;
                    item.Location = world.Engine.GetAbsoluteLocation(e);
                }
                Log.Positions[e].Add(item);
            }
        }


    }
}
