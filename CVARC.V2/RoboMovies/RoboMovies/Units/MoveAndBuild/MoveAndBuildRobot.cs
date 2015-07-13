using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class MoveAndBuildRobot<TActorManager,TWorld,TSensorsData> : 
                Robot<TActorManager,TWorld,TSensorsData, MoveAndBuildCommand, MoveAndBuildRules>,
                ITowerBuilderRobot
        where TActorManager : IActorManager
        where TWorld : IWorld
        where TSensorsData : new()
    {
        public SimpleMovementUnit SimpleMovementUnit { get; private set; }
        public TowerBuilderUnit TowerBuilder {get; private set;}

        public override void AdditionalInitialization()
        {
            base.AdditionalInitialization();
            SimpleMovementUnit = new SimpleMovementUnit(this);
            TowerBuilder = new TowerBuilderUnit(this);
        }

        public override IEnumerable<IUnit> Units
        {
            get
            {
                yield return SimpleMovementUnit;
                yield return TowerBuilder;
            }
        }
    }
}
