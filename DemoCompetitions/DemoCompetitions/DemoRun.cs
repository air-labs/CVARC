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
            //this is loaded from a library
            var competitions = new Competitions(
                new DemoWorld(),
                new KRPhysical(),
                new DemoWorldManager(),
                new IActorManagerFactory[] { new ActorManagerFactory<DemoActorManager>() });

            var environment = new CompetitionsEnvironment(
                new SceneSettings(),
                new IController[] { new SquareWalkingBot(100) });

            competitions.World.Initialize(competitions, environment);
            var form = new KRForm(competitions.World);
            Application.Run(form);

        }
    }
}
