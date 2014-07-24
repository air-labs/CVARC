using CVARC.Core;

namespace CVARC.Basic
{
    public interface ICvarcRules
    {
        Body CreateWorld(ISceneSettings settings);
        void PerformAction(ICvarcEngine engine, string actor, string action);
    }
}
