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
            
            var managerPart=new ManagerPart(
                new DemoWorldManager(),
                new IActorManagerFactory[] { new ActorManagerFactory<DemoActorManager>() });

            return new Competitions(new DemoLogicPart(), new KREnginePart(), managerPart);
        }


        public static void Main()
        {

            var competitions = Create();
            var environment = new TutorialEnvironment<SceneSettings>(competitions, new SceneSettings());
            var form = new KRForm(competitions.Create(environment));
            Application.Run(form);

        }
    }
}
