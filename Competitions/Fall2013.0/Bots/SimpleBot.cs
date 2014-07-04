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
            Command(Cmd.Grip);
            Mov(-50);
            Rot(90);
            Mov(20);
            Command(Cmd.Release);
        }
    }
}
