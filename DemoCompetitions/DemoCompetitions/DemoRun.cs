using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CVARC.V2;
using CVARC.V2.SimpleMovement;

namespace DemoCompetitions
{
    class DemoRun
    {
        public static void Main()
        {
            var world = new DemoWorld();
            var engine = new Engine(
                new KRPhysical(),
                new DemoWorldManager(),
                new IActorManagerFactory[] { new ActorManagerFactory<DemoActorManager>() });
            var controllers = new IController[] { new SquareWalkingBot() };
            world.Initialize(new object(), engine, controllers);
            var form = new KRForm(world);
            Application.Run(form);

        }
    }
}
