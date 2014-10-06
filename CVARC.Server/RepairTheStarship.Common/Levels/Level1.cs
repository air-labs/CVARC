using CVARC.Basic;
using RepairTheStarship;

namespace RepairTheStarship.Levels
{
    public class Level1 : BaseLevel
    {
        public override ISceneSettings ParseSettings(CVARC.Network.HelloPackage helloPackage)
        {
            return SceneSettings.GetRandomMap();
        }
    };
}