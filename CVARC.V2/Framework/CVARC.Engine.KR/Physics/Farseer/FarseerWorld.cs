using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Physics.Farseer;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;
using FBody = FarseerPhysics.Dynamics.Body;
using AIRLab.Mathematics;
using CVARC.Core.Physics;

namespace CVARC.Core.Physics.FarseerWrap
{
	public class FarseerWorld : IWorld
	{
		public FarseerWorld()
		{
            World = new World(Microsoft.Xna.Framework.Vector2.Zero);
			FarseerPhysics.Settings.VelocityIterations = 5;
			FarseerPhysics.Settings.PositionIterations = 3;
			//FarseerPhysics.Settings.TOIPositionIterations = 100;
			//FarseerPhysics.Settings.TOIVelocityIterations = 100;
		}		

		public World World = new World(Microsoft.Xna.Framework.Vector2.Zero);

		internal readonly Dictionary<FarseerPhysics.Dynamics.Body, FarseerBody> FBodyToFarseerBody = new Dictionary<FarseerPhysics.Dynamics.Body, FarseerBody>();

		//--------------------------------------------------------------------------

		#region Iterations logic

		/// <summary>
		/// Просчёт в физического мира, изменившегося на время dt
		/// </summary>		
		public void MakeIteration(double dt, Body root)
		{
			//foreach (var i in World.BodyList)
			//{
			//    System.Diagnostics.Debug.WriteLine(i.Position + " " + i.BodyId + " " + i.LinearVelocity + " "
			//        + i.IsStatic + " " + i.Enabled + " " + i.Rotation + " " + i.LinearDamping);
			//}

			try
			{
				World.Step((float)dt);
			}
			catch(Exception e)
			{
				System.Diagnostics.Debug.WriteLine(e);
			}

			//foreach (var i in World.BodyList)
			//{
			//    System.Diagnostics.Debug.WriteLine(i.Position + " " + i.BodyId + " " + i.LinearVelocity + " "
			//        + i.IsStatic + " " + i.Enabled + " " + i.Rotation);
			//}
		}

		#endregion

		//--------------------------------------------------------------------------

		#region Joints

		public void RemoveJoint(Joint joint)
		{
			World.RemoveJoint(joint);
		}

		public WeldJoint MakeWeldJoint(FBody a, FBody b, Frame3D bBodyOffset)
		{
			return JointFactory.CreateWeldJoint(World, a, b, new Microsoft.Xna.Framework.Vector2(0, 0),
				FarseerConverter.ToSimUnits(new Microsoft.Xna.Framework.Vector2((float)-bBodyOffset.X, (float)-bBodyOffset.Y))); 
			//без минусов не работает как надо
		}

		public WeldJoint MakeWeldJoint(FBody a, FBody b)
		{
			//return JointFactory.CreateWeldJoint(World, a, b, new Microsoft.Xna.Framework.Vector2(0, 0));
			return MakeWeldJoint(a, b, new Frame3D(b.Position.X, b.Position.Y, 0));
		}

		#endregion

		//--------------------------------------------------------------------------

		#region Making primitives

		public IPhysical MakeBox(double xsize, double ysize, double zsise)
		{
			FBody fb = BodyFactory.CreateRectangle(World,
				FarseerConverter.ToSimUnits((float)xsize), FarseerConverter.ToSimUnits((float)ysize), (float)1);
			fb.BodyType = BodyType.Dynamic;
			return new FarseerBody(fb, this);
		}

		public IPhysical MakeCyllinder(double rbottom, double rtop, double height)
		{
			FBody fb = BodyFactory.CreateCircle(World, FarseerConverter.ToSimUnits((float)rbottom), (float)1);
			fb.BodyType = BodyType.Dynamic;
			return new FarseerBody(fb, this);
		}		

		#endregion

		//--------------------------------------------------------------------------

	}
}
