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

        public Judgement JudgeUpOrDownResultMulti(int userInput, HashSet<TargetValue> targetValues)
        {
            if (targetValues == null)
            {
                throw new ArgumentNullException(nameof(targetValues), "TargetValues 는 null 이면 안된다 !!!");
            }

            TargetValue mostCloseTarget = targetValues
                .Where(tv => !tv.IsSolved)
                .OrderBy(tv => Math.Abs(userInput - tv.Value))
                .FirstOrDefault();

            int mostCloseDifference = userInput - mostCloseTarget.Value;

            if (mostCloseDifference == 0)
            {
                // todo: List 를 이용해서 값을 직접 변경하는 방법이 나은지???
                if (targetValues.Remove(mostCloseTarget))
                {
                    targetValues.Add(new TargetValue { Value = mostCloseTarget.Value, IsSolved = true });
                    gameLevelManager.UpdateTargetRemains();

                    return Judgement.Equal;
                }
                else
                {
                    throw new InvalidOperationException("정상적으로 삭제가 안된다면 뭔가 잘못된 것임");
                }
            }

            return mostCloseDifference > 0
                ? Judgement.InputIsHigherThanTarget
                : Judgement.InputIsLowerThanTarget;
        }

        public bool IsSolvedTargetAll(HashSet<TargetValue> targetValues)
        {
            return targetValues.All(tv => tv.IsSolved);
        }
    }
}