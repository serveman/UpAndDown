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

        private static readonly Random randomGenerator = new Random();

        public HashSet<TargetValue> TargetValuesSet { get; set; } = new HashSet<TargetValue>();
        public int Level { get; set; }
        public int TargetRemains { get; set; }

        public GameLevelManager() { }

        /// <summary>
        /// 난이도 선택
        /// 난이도 숫자 = 목표로 해야하는 Target 의 개수
        /// </summary>
        public void SelectGameLevel()
        {
            int min = GameLevelMin;
            int max = GameLevelMax;

            bool isInvalid;

            int inputLevel;
            do
            {
                Console.Write($"난이도를 선택해주세요(쉬움:{min} ~ {max}:어려움): ");
                isInvalid = !int.TryParse(Console.ReadLine(), out inputLevel);

                if (isInvalid)
                {
                    Console.WriteLine($"{inputLevel} 입력한 값의 형식이 잘못되었습니다. 숫자로만 넣어주세요!");
                    Console.WriteLine();
                    continue;
                }
                
                if (inputLevel < min || inputLevel > max)
                {
                    Console.WriteLine("입력값이 범위를 벗어났습니다. 다시 시도해주세요!");
                    Console.WriteLine();
                    isInvalid = true;
                }
            } while (isInvalid);

            Level = inputLevel;

            GenerateTargetValuesSet(totalTargetCount: Level);
        }

        private void GenerateTargetValuesSet(int totalTargetCount)
        {
            int min = GuessNumberMin;
            int max = GuessNumberMax;

            int maxTargetcount = max - min;

            if (totalTargetCount > maxTargetcount)
            {
                throw new InvalidOperationException("가능한 추측 범위보다 레벨이 더 높음. 최대 -> 레벨:범위=1:1");
            }

            HashSet<TargetValue> newTargetValuesSet = new HashSet<TargetValue>();
            int retryCount = 0;

            while (newTargetValuesSet.Count < totalTargetCount)
            {
                int randomValue = GenerateRandomTargetValue(min, max);
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
            TargetValuesSet = newTargetValuesSet;

            UpdateTargetRemains();
        }

        public void UpdateTargetRemains() => TargetRemains = TargetValuesSet.Sum(tv => tv.IsSolved == false ? 1 : 0);

        private int GenerateRandomTargetValue(int min, int max) => randomGenerator.Next(min, max);
    }
}
