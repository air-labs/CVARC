using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AIRLab.Mathematics;
using CVARC.V2;

namespace Demo
{
    public class DemoWorld : World<DemoWorldState, IDemoWorldManager>
    {
		public override void AdditionalInitialization()
		{
			base.AdditionalInitialization();
			var detector = new CollisionDetector(this);
			detector.FindControllableObject = side =>
			{
				var actor = Actors
					.OfType<DemoRobot>()
					.Where(z => z.ObjectId == side.ObjectId || z.Gripper.GrippedObjectId == side.ObjectId)
					.FirstOrDefault();
				if (actor != null)
				{
					side.ControlledObjectId = actor.ObjectId;
					side.ControllerId = actor.ControllerId;
				}
			};

			detector.Account = c =>
			{
				if (!c.Victim.IsControllable) return;
				if (!detector.Guilty(c)) return;
				Scores.Add(c.Offender.ControllerId, -1, "Collision");
			};
		}

        public override void CreateWorld()
        {
           Manager.CreateWorld(IdGenerator);
            foreach (var obj in WorldState.Objects)
            {
                var id = IdGenerator.CreateNewId(obj);
                Manager.CreateObject(obj);
            }
        }

    }
}
