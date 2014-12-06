using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class MoveAndGripRobot<TActorManager,TWorld,TSensorsData> : Robot<TActorManager,TWorld,TSensorsData,MoveAndGripCommand,MoveAndGripRules>
        where TActorManager : IActorManager
        where TWorld : IWorld
        where TSensorsData : new()
    {
        public SimpleMovementUnit SimpleMovementUnit { get; private set; }
        public GripperUnit GripperUnit { get; private set; }
        public override void AdditionalInitialization()
        {
            base.AdditionalInitialization();
            SimpleMovementUnit = new SimpleMovementUnit(this);
            GripperUnit = new GripperUnit(this);
        }

        public override IEnumerable<IUnit<MoveAndGripCommand>> Units
        {
            get
            {
                yield return GripperUnit;
                yield return SimpleMovementUnit;
            }
        }
    }
}
