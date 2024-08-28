using System.Collections.Generic;
using UpAndDown.Game.Model;

namespace UpAndDown.Interface
{
    public interface IGameLevelManager
    {
        int Level { get; set; }
        int TargetRemains { get; set; }

        int GetGuessNumberMin();
        int GetGuessNumberMax();
        int GetGameLevelMin();
        int GetGameLevelMax();

        void SelectGameLevel(out HashSet<TargetValue> targetValuesSet);
        void UpdateTargetRemains(HashSet<TargetValue> targetValuesSet);
    }
}
