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
        static Competitions Create()
        {
            var logicPart = new LogicPart(
                new DemoWorld(), 
                keyboard=>new SimpleMovementTwoPlayersKeyboardControllerPool(keyboard));

            var managerPart=new ManagerPart(
                new DemoWorldManager(),
                new IActorManagerFactory[] { new ActorManagerFactory<DemoActorManager>() });

            return new Competitions(logicPart, new KREnginePart(), managerPart);
        }


        public static void Main()
        {

            var competitions = Create();
            var environment = new TutorialEnvironment<SceneSettings>(competitions, new SceneSettings());
            competitions.Load(environment);
            var form = new KRForm(competitions.Logic.World);
            Application.Run(form);

        }
    }
}
