using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIRLab.Mathematics;
using CVARC.V2;

namespace Demo
{
    public class DemoRobot : Robot<IActorManager,DemoWorld,DemoSensorsData,DemoCommand,DemoRules>,
                IGrippableRobot, IDWMRobot
    {
		public DWMUnit DWMUnit { get; private set;  }
		public SimpleMovementUnit SimpleMovementUnit { get; private set; }
		public GripperUnit Gripper { get; private set; }

		public override IEnumerable<IUnit> Units
		{
			get
			{
				yield return Gripper;
				yield return SimpleMovementUnit;
				yield return DWMUnit;
			}
		}
	
		public override void AdditionalInitialization()
		{
			base.AdditionalInitialization();
			
			SimpleMovementUnit = new SimpleMovementUnit(this);
			Gripper = new GripperUnit(this);
			DWMUnit = new DWMUnit(this);
		    Gripper.FindDetail = () =>
		    {
		        var all = World.IdGenerator.GetAllPairsOfType<DemoObjectData>()
		            .Where(z => World.Engine.ContainBody(z.Item2))
		            .Where(z => !World.Engine.IsAttached(z.Item2))
                    .Where(z=>!z.Item1.IsStatic)
		            .Select(z => new {Item = z, Availability = Gripper.GetAvailability(z.Item2)})
		            .ToList();

		        return all
		            .Where(z => z.Availability.Distance < 3)
		            .OrderBy(z => z.Availability.Distance)
		            .Select(z => z.Item.Item2)
		            .FirstOrDefault();
		    };
            Gripper.GrippingPoint = new Frame3D(12.5, 0, 10);
            DWMData = new DWMData();
		}

        public DWMData DWMData
        {
            get;
            set;
        }
    }
}
