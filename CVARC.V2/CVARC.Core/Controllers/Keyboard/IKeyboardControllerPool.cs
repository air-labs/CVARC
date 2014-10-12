using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public interface IKeyboardControllerPool
    {
        void Initialize(IWorld world, IKeyboard keyboard);
        IController CreateController(string controllerId);
    }
}
