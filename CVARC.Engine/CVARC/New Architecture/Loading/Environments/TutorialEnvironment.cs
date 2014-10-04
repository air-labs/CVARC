using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic;

namespace CVARC.V2
{
    public class TutorialEnvironment<TSceneSettings> : Environment
        where TSceneSettings : ISceneSettings
    {
        TSceneSettings sceneSettings;
        IKeyboardControllerPool pool;

        public TutorialEnvironment(Competitions competitions, TSceneSettings sceneSettings) : base(competitions)
        {
            this.sceneSettings = sceneSettings;
            pool = competitions.Logic.KeyboardControllerPoolFactory(competitions.Engine.Keyboard);
        }


        public override ISceneSettings GetSceneSettings()
        {
            return sceneSettings;
        }

        public override IController GetController(string controllerId)
        {
            return pool.CreateController(controllerId);
        }
    }
}
