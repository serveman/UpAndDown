using System;
using System.Collections.Generic;
using System.Linq;
using UpAndDown.Game.Model;
using UpAndDown.Interface;

namespace UpAndDown.Core.Domain
{
    public class GameLevelManager : IGameLevelManager
    {
        public int GuessNumberMin { get; private set; } = 1;
        public int GuessNumberMax { get; private set; } = 100;

        public int GameLevelMin { get; private set; } = 1;
        public int GameLevelMax { get; private set; } = 5;

        private static readonly Random RandomGenerator = new Random();

        public int Level { get; set; }
        public int TargetRemains { get; set; }


        /// <summary>
        /// 난이도 선택
        /// 난이도 숫자 = 목표로 해야하는 Target 의 개수
        /// </summary>
        public void SelectGameLevel(out HashSet<TargetValue> targetValuesSet)
        {
            int inputLevel;
            do
            {
                Console.Write($"난이도를 선택해주세요(쉬움:{GameLevelMin} ~ {GameLevelMax}:어려움): ");
            } while (!int.TryParse(Console.ReadLine(), out inputLevel)
                  || inputLevel < GameLevelMin || inputLevel > GameLevelMax);

            this.Level = inputLevel;

            targetValuesSet = GenerateTargetValuesSet(totalTargetCount: this.Level);
        }

        private HashSet<TargetValue> GenerateTargetValuesSet(int totalTargetCount)
        {
            int maxTargetcount = GuessNumberMax - GuessNumberMin;

            if (totalTargetCount > maxTargetcount)
            {
                throw new InvalidOperationException("가능한 추측 범위보다 레벨이 더 높음. 최대 -> 레벨:범위=1:1");
            }

            HashSet<TargetValue> newTargetValuesSet = new HashSet<TargetValue>();
            int retryCount = 0;

            while (newTargetValuesSet.Count < totalTargetCount)
            {
                int randomValue = GenerateRandomTargetValue();
                TargetValue newTarget = new TargetValue
                {
                    Value = randomValue,
                    IsSolved = false
                };

                if (!newTargetValuesSet.Add(newTarget))
                {
                    retryCount++;   // 디버깅용
                }
            }

            UpdateTargetRemains(newTargetValuesSet);

            return newTargetValuesSet;
        }

        public void UpdateTargetRemains(HashSet<TargetValue> targetValuesSet) => this.TargetRemains = targetValuesSet.Sum(tv => tv.IsSolved == false ? 1 : 0);

        private int GenerateRandomTargetValue() => RandomGenerator.Next(GuessNumberMin, GuessNumberMax);
    }
}
