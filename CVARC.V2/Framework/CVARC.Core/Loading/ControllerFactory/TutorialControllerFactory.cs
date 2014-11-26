using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public class TutorialControllerFactory : IControllerFactory
    {
        IKeyboardControllerPool pool;
        public TutorialControllerFactory(IKeyboardControllerPool pool)
        {
            this.pool = pool;
        }


        public IController Create(ControllerRequest request)
        {
            return pool.CreateController(request.ControllerId);
        }
    }
}
