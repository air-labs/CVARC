using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace StarshipRepair
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
        public readonly Map Map;

        public SceneSettings()
        {
            VerticalWalls = new WallSettings[5, 4];
            HorizontalWalls = new WallSettings[6, 3];
            Details = new DetailData[6];
            Map = new Map();
        }

        public static SceneSettings GetRandomMap(int seed = -1)
        {
            var rand = new Random();
            if (seed == 0)
                return GetDefaulSettings();
            if (seed > 0)
                rand = new Random(seed);
            SceneSettings setts;
            do
            {
                setts = new SceneSettings();
                var toAdd = Enumerable.Range(0, 6 * 4).Select(a => WallSettings.NoWall).ToList();
                toAdd[0] = WallSettings.BlueSocket;
                toAdd[1] = WallSettings.RedSocket;
                toAdd[2] = WallSettings.GreenSocket;
                toAdd[3] = WallSettings.Wall;
                toAdd[4] = WallSettings.Wall;
                toAdd[5] = WallSettings.Wall;
                toAdd[6] = WallSettings.Wall;
                
                while (toAdd.Any())
                {
                    var ind = rand.Next(toAdd.Count);
                    var wall = toAdd[ind];
                    toAdd.RemoveAt(ind);
                    bool isHorizontal = rand.NextDouble() > 0.5;
                    if (isHorizontal)
                    {
                        while (true)
                        {
                            var x = rand.Next(3);
                            var y = rand.Next(setts.HorizontalWalls.GetLength(1));
                            if (setts.HorizontalWalls[x, y] == WallSettings.NoWall)
                            {
                                setts.HorizontalWalls[x, y] = wall;
                                setts.HorizontalWalls[setts.HorizontalWalls.GetLength(0) - x-1, y] = wall ==
                                                                                                   WallSettings
                                                                                                       .RedSocket
                                                                                                       ? WallSettings
                                                                                                             .BlueSocket
                                                                                                       : wall ==
                                                                                                         WallSettings
                                                                                                             .BlueSocket
                                                                                                             ? WallSettings
                                                                                                                   .RedSocket
                                                                                                             : wall;
                                break;
                            }
                        }
                    }
                    else
                    {
                        while (true)
                        {
                            var x = rand.Next(3);
                            var y = rand.Next(setts.VerticalWalls.GetLength(1));
                            if (wall != WallSettings.Wall && x == 2) continue;
                            if (setts.VerticalWalls[x, y] == WallSettings.NoWall)
                            {
                                setts.VerticalWalls[x, y] = wall;
                                setts.VerticalWalls[setts.VerticalWalls.GetLength(0) - x-1, y] = wall ==
                                                                                                   WallSettings
                                                                                                       .RedSocket
                                                                                                       ? WallSettings
                                                                                                             .BlueSocket
                                                                                                       : wall ==
                                                                                                         WallSettings
                                                                                                             .BlueSocket
                                                                                                             ? WallSettings
                                                                                                                   .RedSocket
                                                                                                             : wall;
                                break;
                            }
                        }
                    }
                }
                setts.Map.Init(setts);
            } while (!setts.Map.IsValid());
            CreateDetails(setts);
            return setts;
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

            CreateDetails(settings);
            settings.Map.Init(settings);
            return settings;
        }

        private static void CreateDetails(SceneSettings settings)
        {
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
        }
    }
}
