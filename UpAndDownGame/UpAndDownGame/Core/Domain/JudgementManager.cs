using System;
using System.Collections.Generic;
using System.Linq;
using UpAndDown.Game.Enum;
using UpAndDown.Game.Model;
using UpAndDown.Interface;

namespace UpAndDown.Core.Domain
{
    public class JudgementManager : IJudgementManager
    {
        private IGameLevelManager gameLevelManager;

        public JudgementManager(IGameLevelManager gameLevelManager)
        {
            this.gameLevelManager = gameLevelManager;
        }

        public JudgementEnum JudgeUpOrDownResult(int userInput, int targetValue)
        {
            if (userInput == targetValue)
            {
                return JudgementEnum.Equal;
            }

            return userInput > targetValue
                ? JudgementEnum.InputIsHigherThanTarget
                : JudgementEnum.InputIsLowerThanTarget;
        }

        public JudgementEnum JudgeUpOrDownResultMulti(int userInput, HashSet<TargetValueStruct> targetValues)
        {
            if (targetValues == null)
            {
                throw new ArgumentNullException(nameof(targetValues), "TargetValues 는 null 이면 안된다 !!!");
            }

            TargetValueStruct mostCloseTarget = targetValues
                .Where(tv => !tv.IsSolved)
                .OrderBy(tv => Math.Abs(userInput - tv.Value))
                .FirstOrDefault();

            int mostCloseDifference = userInput - mostCloseTarget.Value;

            if (mostCloseDifference == 0)
            {
                // todo: List 를 이용해서 값을 직접 변경하는 방법이 나은지???
                if (targetValues.Remove(mostCloseTarget))
                {
                    targetValues.Add(new TargetValueStruct { Value = mostCloseTarget.Value, IsSolved = true });
                    gameLevelManager.UpdateTargetRemains();

                    return JudgementEnum.Equal;
                }
                else
                {
                    throw new InvalidOperationException("정상적으로 삭제가 안된다면 뭔가 잘못된 것임");
                }
            }

            return mostCloseDifference > 0
                ? JudgementEnum.InputIsHigherThanTarget
                : JudgementEnum.InputIsLowerThanTarget;
        }

        public bool IsSolvedTargetAll(HashSet<TargetValueStruct> targetValues)
        {
            return targetValues.All(tv => tv.IsSolved);
        }
    }
}