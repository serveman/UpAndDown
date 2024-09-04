using System;

namespace UpAndDown.Game.Model
{
    public class PlayCount
    {
        public int Level { get; set; }
        public int Success { get; set; }
        public int Failure { get; set; }
        public int Total { get; private set; }

        public PlayCount(int level, int success = 0, int failure = 0)
        {
            Level = level;
            Success = success;
            Failure = failure;
            Total = Success + Failure;
        }

        public void IncreaseCount(bool isSuccess)
        {
            if (isSuccess)
            {
                Success++;
            }
            else
            {
                Failure++;
            }

            Total = Success + Failure;
        }
    }
}
