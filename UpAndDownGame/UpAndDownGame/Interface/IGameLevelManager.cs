using System.Collections.Generic;
using UpAndDown.Game.Model;

namespace UpAndDown.Interface
{
    public interface IGameLevelManager
    {
        int Level { get; set; }
        int TargetRemains { get; set; }

        int GuessNumberMin { get; }
        int GuessNumberMax { get; }
        int GameLevelMin { get; }
        int GameLevelMax { get; }

        void SelectGameLevel(out HashSet<TargetValue> targetValuesSet);
        void UpdateTargetRemains(HashSet<TargetValue> targetValuesSet);
    }
}
