using CVARC.Basic;
using CVARC.Basic.Controllers;
using MapHelper;
using RepairTheStarship.Sensors;

namespace RepairTheStarship.Bots
{
    class MolagBal : FixedProgramBot
    {
        public override void Initialize(Competitions competitions)
        {
            base.Initialize(competitions);
            var map = Competitions.GetSensorsData<SensorsData>(ControlledRobot).BuildStaticMap();
        }

        public override void DefineProgram()
        {
            Rot(-90);
            Mov(50);
            Rot(90);
            Mov(120);

            Rot(-90);
            Mov(50);
            Rot(90);
            Mov(50);
            Rot(90);
            Mov(50);
            Rot(-90);

            Mov(120);
            Rot(90);
            Mov(50);
        }

        public override Command MakeTurn()
        {
            return base.MakeTurn();
        }
    }
}
