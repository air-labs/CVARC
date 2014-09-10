using CVARC.Basic;
using CVARC.Basic.Controllers;

namespace RepairTheStarship
{
    public class SimpleBot : FixedProgramBot
    {
        public override void DefineProgram()
        {
 	        Rot(-90);
            Mov(50);
            Mov(-50);
            Rot(90);
            Mov(20);
        }

        public override Command MakeTurn()
        {
            throw new System.NotImplementedException();
        }
    }
}
