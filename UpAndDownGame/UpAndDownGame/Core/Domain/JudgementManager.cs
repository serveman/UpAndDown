﻿using System;
using System.Collections.Generic;
using System.Linq;
using UpAndDown.Game.Enum;
using UpAndDown.Game.Model;

namespace UpAndDown.Core.Domain
{
    public class JudgementManager
    {
        /// <summary>
        /// 결과를 판정하는 메서드
        /// </summary>
        /// <param name="userInput">현재 유저가 입력한 숫자</param>
        /// <param name="targetValue">목표값</param>
        /// <returns>
        /// 비교 결과를 나타내는 Judgement enum:
        /// <list type="bullet">
        /// <item><description><see cref="Judgement.InputIsHigherThanTarget"/>: 입력값이 목표값보다 큼.</description></item>
        /// <item><description><see cref="Judgement.InputIsLowerThanTarget"/>: 입력값이 목표값보다 작음.</description></item>
        /// <item><description><see cref="Judgement.Equal"/>: 입력값이 목표값과 같음.</description></item>
        /// </list>
        /// </returns>
        public Judgement JudgeUpOrDownResult(int userInput, int targetValue)
        {
            if (userInput == targetValue)
            {
                return Judgement.Equal;
            }

            return userInput > targetValue
                ? Judgement.InputIsHigherThanTarget
                : Judgement.InputIsLowerThanTarget;
        }

        /// <summary>
        /// 결과를 판정하는 메서드
        /// 난이도 확장으로, 목표하는 값들을 2개 이상으로 확장
        /// 가장 가까운 목표값과 비교를 해서 결과를 리턴
        /// 정답을 맞춘 목표값은 이후 결과에 영향을 주지 않도록 함
        /// </summary>
        /// <param name="userInput">현재 유저가 입력한 숫자</param>
        /// <param name="targetValues">목표값들</param>
        /// <returns>
        /// 비교 결과를 나타내는 Judgement enum:
        /// <list type="bullet">
        /// <item><description><see cref="Judgement.InputIsHigherThanTarget"/>: 입력값이 가장 가까이 있는 목표값보다 큼.</description></item>
        /// <item><description><see cref="Judgement.InputIsLowerThanTarget"/>: 입력값이 가장 가까이 있는 목표값보다 작음.</description></item>
        /// <item><description><see cref="Judgement.Equal"/>: 입력값이 목표값과 같음.</description></item>
        /// </list>
        /// </returns>
        public Judgement JudgeUpOrDownResultMulti(int userInput, HashSet<TargetValue> targetValues)
        {
            TargetValue mostCloseTarget = targetValues
                .Where(tv => !tv.IsSolved)
                .OrderBy(tv => Math.Abs(userInput - tv.Value))
                .FirstOrDefault();

            int mostCloseDifference = userInput - mostCloseTarget.Value;

            if (mostCloseDifference == 0)
            {
                // List 를 이용해서 값을 직접 변경하는 방법이 나은지???
                if (targetValues.Remove(mostCloseTarget))
                {
                    targetValues.Add(new TargetValue { Value = mostCloseTarget.Value, IsSolved = true });
                }
                else
                {
                    throw new InvalidOperationException("정상적으로 삭제가 안된다면 뭔가 잘못된 것임");
                }

                return Judgement.Equal;
            }

            return mostCloseDifference > 0
                ? Judgement.InputIsHigherThanTarget
                : Judgement.InputIsLowerThanTarget;
        }

        public int FindTargetRemain(HashSet<TargetValue> targetValues)
        {
            return targetValues.Sum(tv => tv.IsSolved == false ? 1 : 0);
        }

        public bool IsSolvedTargetAll(HashSet<TargetValue> targetValues)
        {
            return targetValues.All(tv => tv.IsSolved);
        }
    }
}