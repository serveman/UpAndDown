using System;
using System.Collections.Generic;

namespace UpAndDown.Game
{
    public struct TargetValue
    {
        public int Value { get; set; }
        public bool IsSolved { get; set; }
    }

    public class GameLevel
    {
        protected int level = 1;
        protected HashSet<TargetValue> targetValues = new HashSet<TargetValue>();

        /// <summary>
        /// 난이도 선택
        /// 난이도 숫자 = 목표로 해야하는 Target 의 개수
        /// </summary>
        protected void SelectGameLevel()
        {
            int inputLevel = 0;
            do
            {
                Console.Write("난이도를 선택해주세요(쉬움:1 ~ 5:어려움): ");
            } while (!int.TryParse(Console.ReadLine(), out inputLevel) || (inputLevel < 1 || inputLevel > 5));

            this.level = inputLevel;

            targetValues = MakeTargetValueList();
        }

        private HashSet<TargetValue> MakeTargetValueList()
        {
            HashSet<TargetValue> targetValues = new HashSet<TargetValue>();
            int retryCount = 0;

            for (int i = 0; i < this.level + retryCount; i++)
            {
                int randomValue = GenerateRandomTargetValue(retryCount);
                TargetValue newTarget = new TargetValue { Value = randomValue, IsSolved = false };
                if (targetValues.Contains(newTarget))
                {
                    retryCount++;    // 무한루프 가능성 있음. 주의필요
                    if (retryCount > 100)
                    {
                        throw new NotImplementedException("난이도에서 랜덤 숫자 배정 시 재시도 횟수 100회 이상일 때 미구현");
                    }
                }
                else
                {
                    targetValues.Add(newTarget);
                }
            }

            return targetValues;
        }

        private int GenerateRandomTargetValue(int additionalSeed)
        {
            Random r = new Random(unchecked((int)DateTime.Now.Ticks + additionalSeed));
            return r.Next(1, 100);
        }

    }
}
