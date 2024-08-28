using System.Collections.Generic;
using UpAndDown.Game.Enum;
using UpAndDown.Game.Model;

namespace UpAndDown.Interface
{
    public interface IJudgementManager
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
        Judgement JudgeUpOrDownResult(int userInput, int targetValue);

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
        Judgement JudgeUpOrDownResultMulti(int userInput, HashSet<TargetValue> targetValues);

        int FindTargetRemain(HashSet<TargetValue> targetValues);
        bool IsSolvedTargetAll(HashSet<TargetValue> targetValues);
    }
}