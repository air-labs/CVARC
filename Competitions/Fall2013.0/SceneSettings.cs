using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Gems
{
    public enum WallSettings
    {
        NoWall,
        Wall,
        RedSocket,
        GreenSocket,
        BlueSocket,
    }

    public enum DetailColor
    {
        Red,
        Green,
        Blue,
    }

    public struct DetailData
    {
        public DetailColor Color { get; set; }
        public Point Location { get; set; }
    }

    public class SceneSettings
    {
        public readonly WallSettings[,] VerticalWalls;
        public readonly WallSettings[,] HorizontalWalls;
        public readonly DetailData[] Details;

        public SceneSettings()
        {
            VerticalWalls = new WallSettings[5, 4];
            HorizontalWalls = new WallSettings[6, 3];
            Details = new DetailData[6];
        }

        public static SceneSettings GetDefaulSettings()
        {
            var settings=new SceneSettings();
            settings.VerticalWalls[0, 0] = WallSettings.RedSocket;
            settings.VerticalWalls[4, 0] = WallSettings.BlueSocket;
            
            settings.VerticalWalls[2, 1] = WallSettings.Wall;
            
            settings.HorizontalWalls[1, 0] = 
            settings.HorizontalWalls[4, 0] = 
                WallSettings.Wall;
            
            settings.HorizontalWalls[0, 1] = 
            settings.HorizontalWalls[5, 1] = 
                WallSettings.Wall;

            settings.HorizontalWalls[2, 2] =
            settings.HorizontalWalls[3, 2] =
                WallSettings.Wall;

            settings.HorizontalWalls[1, 2] =
            settings.HorizontalWalls[4, 2] =
                 WallSettings.GreenSocket;

            settings.HorizontalWalls[1, 1] = WallSettings.BlueSocket;
            settings.HorizontalWalls[4, 1] = WallSettings.RedSocket;

            settings.Details[0].Color = DetailColor.Red;
            settings.Details[0].Location = new Point(0, 1);

            settings.Details[1].Color = DetailColor.Red;
            settings.Details[1].Location = new Point(0, 3);

            settings.Details[2].Color = DetailColor.Blue;
            settings.Details[2].Location = new Point(5, 1);

            settings.Details[3].Color = DetailColor.Blue;
            settings.Details[3].Location = new Point(5, 3);

            settings.Details[4].Color = DetailColor.Green;
            settings.Details[4].Location = new Point(2, 0);

            settings.Details[5].Color = DetailColor.Green;
            settings.Details[5].Location = new Point(3, 0);

            return settings;
        }
    }
}
