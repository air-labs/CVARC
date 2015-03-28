using CVARC.Basic;
using RepairTheStarship;

namespace Gems.Levels
{
    public class Level1 : BaseLevel
    {
        public override ISceneSettings ParseSettings(CVARC.Network.HelloPackage helloPackage)
        {
            return SceneSettings.GetRandomMap(helloPackage.MapSeed);
        }
    };
}