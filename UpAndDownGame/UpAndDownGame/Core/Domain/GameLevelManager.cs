using System;
using System.Collections.Generic;
using UpAndDown.Game.Model;

namespace UpAndDown.Core.Domain
{
    public class GameLevelManager
    {
        public const int GUESS_NUMBER_MIN = 1;
        public const int GUESS_NUMBER_MAX = 100;

        private const int GAME_LEVEL_MIN = 1;
        private const int GAME_LEVEL_MAX = 5;

        private static readonly Random RandomGenerator = new Random();

        protected int Level { get; set; }
        protected HashSet<TargetValue> TargetValuesSet { get; set; }


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
                    retryCount++;
                    if (retryCount > 1000)
                    {
                        throw new NotImplementedException("난이도에서 랜덤 숫자 배정 시 무한루프 방지에 걸림. 게임 조건이나 중복 로직 확인 필요");
                    }
                }
            }

            return newTargetValuesSet;
        }

        private int GenerateRandomTargetValue()
        {
            return RandomGenerator.Next(GUESS_NUMBER_MIN, GUESS_NUMBER_MAX);
        }
    }
}
