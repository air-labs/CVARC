using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using AIRLab.Mathematics;
using CVARC.Basic;
using CVARC.Core;
using Gems;

namespace StarshipRepair
{
    public class GemsWorldV0 : GemsWorld
    {
        public override ISceneSettings ParseSettings(CVARC.Network.HelloPackage helloPackage)
        {
            return SceneSettings.GetRandomMap(-1);
        }
    }

    public class GemsWorldV1 : GemsWorld
    {


        public override ISceneSettings ParseSettings(CVARC.Network.HelloPackage helloPackage)
        {
            return SceneSettings.GetRandomMap(helloPackage.MapSeed);
        }
    }

    public abstract class GemsWorld : World
    {
        public static string GripAction = "Grip";
        public static string ReleaseAction = "Release";

        public static readonly Color WallColor=Color.LightGray;

        public override int RobotCount
        {
            get { return 2; }
        }
        public override int CompetitionId { get { return 5; } }

        public override Robot CreateRobot(int robotNumber)
        {
            return new GemsRobot(this, robotNumber);
        }
    }


   
}
