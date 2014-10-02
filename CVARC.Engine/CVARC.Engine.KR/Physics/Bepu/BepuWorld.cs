using BEPUphysics;
using BEPUphysics.MathExtensions;
using BEPUphysics.Entities;
using BEPUphysics.Constraints.SolverGroups;
using BEPUphysics.Settings;
using BEPUphysics.SolverSystems;

namespace CVARC.Core.Physics.BepuWrap
{
	public class BepuWorld : IWorld
	{
		public BepuWorld() 
		{
			World.ForceUpdater.Gravity = new Vector3(0, 0, -9.8f);

			//CollisionDetectionSettings.AllowedPenetration
			CollisionDetectionSettings.ContactInvalidationLengthSquared = 0.001f;
			CollisionDetectionSettings.DefaultMargin = 0.01f;
			CollisionDetectionSettings.MaximumContactDistance = 0.001f;
			CollisionResponseSettings.BouncinessVelocityThreshold = 5;
			World.Solver.IterationLimit = 20;

			//Increase CollisionDetectionSettings.ContactInvalidationLengthSquared
			//Increase CollisionDetectionSettings.ContactMinimumSeparationDistanceSquared
			//Increase CollisionDetectionSettings.DefaultMargin
			// May need to increase GJKToolbox.HighGJKIterations depending on other tuning
			// May need to increase GJKToolbox.MaximumGJKIterations depending on other tuning
			//Increase PairSimplex.ProgressionEpsilon
			//Increase PairSimplex.DistanceConvergenceEpsilon
			//Increase CollisionResponseSettings.StaticFrictionVelocityThreshold
			//Increase CollisionResponseSettings.MaximumPositionCorrectionSpeed
			//Increase CollisionResponseSettings.BouncinessVelocityThreshold
			//Increase Space.DeactivationManager.VelocityLowerLimit

			//FarseerPhysics.Settings.TOIPositionIterations = 100;
			//FarseerPhysics.Settings.TOIVelocityIterations = 100;
		}

		static public Space World = new Space();

		//--------------------------------------------------------------------------

		#region Iterations logic

		/// <summary>
		/// Просчёт в физического мира, изменившегося на время dt
		/// </summary>		
		public void MakeIteration(double dt, Body root)
		{
			//foreach (var i in World.Entities)
			//{

			//    if (i.Tag is int && (int)i.Tag == 23 || (int)i.Tag == 22 || (int)i.Tag == 6)
			//    {
			//        i.Orientation.Normalize();
			//        System.Diagnostics.Debug.WriteLine(i.Position + " " + (int)i.Tag + " " + i.LinearVelocity + " " +
			//            new Vector3(i.Orientation.X, i.Orientation.Y, i.Orientation.Z) + ", angle: " + Angle.FromRad(i.Orientation.W).Grad);
			//    }
			//}

			try
			{
				World.Update((float)dt);
			}
			catch { }

			//foreach (var i in World.Entities)
			//{

			//    if (i.Tag is int && (int)i.Tag == 23 || (int)i.Tag == 22 || (int)i.Tag == 6)
			//    {
			//        i.Orientation.Normalize();
			//        System.Diagnostics.Debug.WriteLine(i.Position + " " + (int)i.Tag + " " + i.LinearVelocity + " " +
			//            new Vector3(i.Orientation.X, i.Orientation.Y, i.Orientation.Z) + ", angle: " + Angle.FromRad(i.Orientation.W).Grad);
			//    }
			//}

			if (root != null)
			{
				lock (root)
					UpdateAllBodies(root);
			}

			//foreach (ComplexBody robot in Emulator.Robots)
			//{
			//    robot.UpdateLocation();
			//    //System.Diagnostics.Debug.WriteLine("rotation is " + robot.PhysicalBody.Rotation);
			//}
		}

		private void UpdateAllBodies(CVARC.Core.Body root)
		{
			//TODO. fix after new bodies.
			//TODO. get rid of copypaste.
			/*
			foreach (var body in root.Nested)
			{
				if (body is PhysicalPrimitiveBody)
				{
					var primBody = body as PhysicalPrimitiveBody;
					//System.Diagnostics.Debug.WriteLine(primBody.Location + " " + primBody.Name);
					primBody.UpdateLocation();
					//System.Diagnostics.Debug.WriteLine(primBody.Location + " " + primBody.Name);
				}
				else
					UpdateAllBodies(body);
//				}
			}*/
		}

		#endregion

		//--------------------------------------------------------------------------

		#region Joints

		static public void RemoveJoint(SolverUpdateable joint)
		{
			World.Remove(joint);
		}

		//static public void MakeWeldJoint(Entity a, Entity b, Frame3D bBodyOffset)
		//{
		//    WeldJoint wj = new WeldJoint(a, b);
		//    //return JointFactory.CreateWeldJoint(World, a, b, new Microsoft.Xna.Framework.Vector2(0, 0),
		//    //    ConvertUnits.ToSimUnits(new Microsoft.Xna.Framework.Vector2((float)-bBodyOffset.X, (float)-bBodyOffset.Y)));
		//    //без минусов не работает как надо
		//}

		static public WeldJoint MakeWeldJoint(Entity a, Entity b)
		{
			WeldJoint wj = new WeldJoint(a, b);

			World.Add(wj);

			return wj;
			//return JointFactory.CreateWeldJoint(World, a, b, new Microsoft.Xna.Framework.Vector2(0, 0));
			//return MakeWeldJoint(a, b, new Frame3D(b.Position.X, b.Position.Y, 0));
		}

		#endregion

		//--------------------------------------------------------------------------

		#region Making primitives

		public IPhysical MakeBox(double xsize, double ysize, double zsize)
		{
			var body = new BEPUphysics.Entities.Prefabs.Box(new Vector3(0, 0, 0),
			                                                BepuConverter.ToSimUnits((float)xsize),
			                                                BepuConverter.ToSimUnits((float)ysize),
			                                                BepuConverter.ToSimUnits((float)zsize), 1);
			// в мир сразу не добавляем
			return new BepuBody(body);
		}

		public IPhysical MakeCyllinder(double rbottom, double rtop, double height)
		{
			var body = new BEPUphysics.Entities.Prefabs.Cylinder(new Vector3(0, 0, 0),
			                                                     BepuConverter.ToSimUnits((float)height),
			                                                     BepuConverter.ToSimUnits((float)rbottom), 1);
			// в мир сразу не добавляем
			return new BepuBody(body) {InitialOrientation = Quaternion.CreateFromYawPitchRoll(MathHelper.PiOver2, 0, 0)};
		}

		#endregion

		//--------------------------------------------------------------------------

		//public void Initialize()
		//{

		//}

	}
}
