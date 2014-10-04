using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CVARC.Basic;

namespace CVARC.V2
{
    public class CompetitionsEnvironment
    {
        public readonly ISceneSettings SceneSettings;
        public readonly IEnumerable<IController> Controllers;
        public CompetitionsEnvironment(ISceneSettings sceneSettings, IEnumerable<IController> controllers)
        {
            SceneSettings = sceneSettings;
            Controllers = controllers;
        }


    }

}