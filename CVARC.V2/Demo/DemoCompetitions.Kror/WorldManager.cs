using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIRLab.Mathematics;
using CVARC.Core;
using CVARC.V2;

namespace Demo
{
	public class MovementWorldManager : KroRWorldManager<IWorld>, IDemoWorldManager
	{
		public override void CreateWorld(IdGenerator generator)
		{
			Root.Add(new Box
			{
				XSize = 200,
				YSize = 200,
				ZSize = 3,
				DefaultColor = Color.White,
				Top = new SolidColorBrush { Color = Color.Yellow },
				IsStatic = true,
				NewId = "floor",
			});
		}

		public static Color ToColor(ObjectColor color)
		{
			switch (color)
			{
				case ObjectColor.Black: return Color.Black;
				case ObjectColor.Blue: return Color.Blue;
				case ObjectColor.Green: return Color.Green;
				case ObjectColor.Red: return Color.Red;
				case ObjectColor.White: return Color.White;
				default: throw new Exception("Color " + color.ToString() + " is unknown");
			}
		}

		public void CreateObject(DemoObjectData data)
		{
			Root.Add(new Box
			{
				Location = new Frame3D(data.X, data.Y, data.ZSize / 2),
				XSize = data.XSize,
				YSize = data.YSize,
				ZSize = data.ZSize,
				DefaultColor = ToColor(data.Color),
				IsStatic = data.IsStatic,
				FrictionCoefficient = 99,
                IsMaterial=true,
				NewId = World.IdGenerator.CreateNewId(data)
			});
		}
	}
}
