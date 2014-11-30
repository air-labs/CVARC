using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.V2;
using CVARC.V2.SimpleMovement;

namespace Demo
{
    public class MultiControlLogicPart : LogicPart<
                                                           MovementWorld,
                                                           MultiCommandKeyboardPool,
                                                           MovementRobot,
                                                           MultiCommandPreprocessor,
                                                           NetworkController<SimpleMovementCommand>,
                                                        MovementWorldState
                                                >
    {
        public MultiControlLogicPart()
            : base(new[] { "Left" })
        {

        }

        static Settings GetDefaultSettings()
        {
            return new Settings { TimeLimit = 1000 };
        }
    }
}
