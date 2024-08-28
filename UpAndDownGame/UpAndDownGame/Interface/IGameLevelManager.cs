using System.Collections.Generic;
using UpAndDown.Game.Model;

namespace UpAndDown.Interface
{
    public interface IGameLevelManager
    {
        int Level { get; set; }
        HashSet<TargetValue> TargetValuesSet { get; set; }

        int GetGuessNumberMin();
        int GetGuessNumberMax();
        int GetGameLevelMin();
        int GetGameLevelMax();

        void SelectGameLevel();
    }
}
