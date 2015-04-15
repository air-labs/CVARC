using CVARC.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    public class KnownWorldStates
    {
        public static DemoWorldState EmptyWithOneRobot(bool robotIsRectangular)
        {
            return new DemoWorldState
            {
                Robots = 
				{
					new DemoRobotData
					{
						 Color= ObjectColor.Red,
						 IsRound=!robotIsRectangular,
						 RobotName=TwoPlayersId.Left,
						 X=0,
						 Y=0,
						 YSize=robotIsRectangular?10:5,
						 XSize=robotIsRectangular?10:5,
						 ZSize=10
					},
                    new DemoRobotData
					{
						 Color= ObjectColor.Blue,
						 IsRound=!robotIsRectangular,
						 RobotName=TwoPlayersId.Right,
						 X=400,
						 Y=400,
						 YSize=robotIsRectangular?10:5,
						 XSize=robotIsRectangular?10:5,
						 ZSize=10
					}
				}
            };
        }
        public static DemoWorldState EmptyWithTwoRobot(bool robotIsRectangular)
        {
            return new DemoWorldState
            {
                Robots = 
				{
					new DemoRobotData
					{
						 Color= ObjectColor.Red,
						 IsRound=!robotIsRectangular,
						 RobotName=TwoPlayersId.Left,
						 X=-25,
						 Y=0,
						 YSize=robotIsRectangular?10:5,
						 XSize=robotIsRectangular?10:5,
						 ZSize=10
					},
                    new DemoRobotData
					{
						 Color= ObjectColor.Blue,
						 IsRound=!robotIsRectangular,
						 RobotName=TwoPlayersId.Right,
						 X=25,
						 Y=0,
						 YSize=robotIsRectangular?10:5,
						 XSize=robotIsRectangular?10:5,
						 ZSize=10
					}
				}
            };
        }

        public static DemoWorldState InteractionScene(bool robotIsRectangular)
        {
            var scene = EmptyWithOneRobot(robotIsRectangular);
            scene.Objects.Add(new DemoObjectData
            {
                XSize = 80,
                YSize = 5,
                ZSize = 10,
                X = 0,
                Y = 30,
                Color = ObjectColor.Black,
                IsStatic = true

            });
            scene.Objects.Add(new DemoObjectData
            {
                XSize = 15,
                YSize = 15,
                ZSize = 15,
                X = 50,
                Y = 0,
                IsStatic = false,
                Color = ObjectColor.Blue
            });
            scene.Objects.Add(new DemoObjectData
            {
                XSize = 15,
                YSize = 15,
                ZSize = 15,
                X = -50,
                Y = 0,
                IsStatic = false,
                Color = ObjectColor.Red
            });
            scene.Objects.Add(new DemoObjectData
            {
                XSize = 15,
                YSize = 15,
                ZSize = 15,
                X = 0,
                Y = 35,
                IsStatic = false,
                Color = ObjectColor.Green
            });
            return scene;
        }
        public static DemoWorldState CollisionScene(bool type, bool robotIsRectangular)
        {
            var scene = EmptyWithTwoRobot(robotIsRectangular);
            scene.Objects.Add(new DemoObjectData
            {
                XSize = 80,
                YSize = 5,
                ZSize = 10,
                X = 0,
                Y = 80,
                Color = ObjectColor.Black,
                IsStatic = true

            });
            scene.Objects.Add(new DemoObjectData
            {
                XSize = 80,
                YSize = 5,
                ZSize = 10,
                X = 0,
                Y = -80,
                Color = ObjectColor.Black,
                IsStatic = true

            });
            scene.Objects.Add(new DemoObjectData
            {
                XSize = 15,
                YSize = 15,
                ZSize = 15,
                X = 70,
                Y = 0,
                IsStatic = false,
                Color = ObjectColor.Blue
            });
            scene.Objects.Add(new DemoObjectData
            {
                XSize = 15,
                YSize = 15,
                ZSize = 15,
                X = -70,
                Y = 0,
                IsStatic = false,
                Color = ObjectColor.Red
            });
            scene.Objects.Add(new DemoObjectData
            {
                XSize = 15,
                YSize = 15,
                ZSize = 15,
                X = 20,
                Y = 30,
                IsStatic = false,
                Color = ObjectColor.Green
            });
            return scene;
        }

        
    }
}
