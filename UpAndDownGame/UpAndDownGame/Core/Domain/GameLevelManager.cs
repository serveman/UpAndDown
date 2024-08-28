using System;
using System.Collections.Generic;
using UpAndDown.Game.Model;
using UpAndDown.Interface;

namespace UpAndDown.Core.Domain
{
    public class GameLevelManager : IGameLevelManager
    {
        public const int GUESS_NUMBER_MIN = 1;
        public const int GUESS_NUMBER_MAX = 100;

        private const int GAME_LEVEL_MIN = 1;
        private const int GAME_LEVEL_MAX = 5;

        private static readonly Random RandomGenerator = new Random();

        public int Level { get; set; }
        public HashSet<TargetValue> TargetValuesSet { get; set; }

        public int GetGuessNumberMin() => GUESS_NUMBER_MIN;
        public int GetGuessNumberMax() => GUESS_NUMBER_MAX;
        public int GetGameLevelMin() => GAME_LEVEL_MIN;
        public int GetGameLevelMax() => GAME_LEVEL_MAX;


        /// <summary>
        /// 난이도 선택
        /// 난이도 숫자 = 목표로 해야하는 Target 의 개수
        /// </summary>
        public void SelectGameLevel()
        {
            int inputLevel = 0;
            do
            {
                Console.Write($"난이도를 선택해주세요(쉬움:{GAME_LEVEL_MIN} ~ {GAME_LEVEL_MAX}:어려움): ");
            } while (!int.TryParse(Console.ReadLine(), out inputLevel) || (inputLevel < GAME_LEVEL_MIN || inputLevel > GAME_LEVEL_MAX));

            this.Level = inputLevel;

            TargetValuesSet = GenerateTargetValuesSet();
        }

        private HashSet<TargetValue> GenerateTargetValuesSet()
        {
            if(this.Level > GUESS_NUMBER_MAX - GUESS_NUMBER_MIN)
            {
                throw new InvalidOperationException("가능한 추측 범위보다 레벨이 더 높음. 최대 -> 레벨:범위=1:1");
            }

            HashSet<TargetValue> newTargetValuesSet = new HashSet<TargetValue>();
            int retryCount = 0;

            while (newTargetValuesSet.Count < this.Level)
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

            return newTargetValuesSet;
        }

        private int GenerateRandomTargetValue() => RandomGenerator.Next(GUESS_NUMBER_MIN, GUESS_NUMBER_MAX);
    }
}
