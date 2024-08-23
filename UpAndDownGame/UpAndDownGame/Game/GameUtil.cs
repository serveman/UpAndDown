using UpAndDown.Enum;

namespace UpAndDown.Game
{
    public class GameUtil
    {
        /// <summary>
        /// 결과를 판정하는 메서드
        /// </summary>
        /// <param name="userInput">현재 유저가 입력한 숫자</param>
        /// <returns>
        /// 비교 결과를 나타내는 Judgement enum:
        /// <list type="bullet">
        /// <item><description><see cref="Judgement.InputIsHigherThanTarget"/>: 입력값이 목표값보다 큼.</description></item>
        /// <item><description><see cref="Judgement.InputIsLowerThanTarget"/>: 입력값이 목표값보다 작음.</description></item>
        /// <item><description><see cref="Judgement.Equal"/>: 입력값이 목표값과 같음.</description></item>
        /// </list>
        /// </returns>
        public static Judgement JudgeUpOrDownResult(int userInput, int targetValue)
        {
            if (userInput == targetValue)
            {
                return Judgement.Equal;
            }
            if (userInput > targetValue)
            {
                return Judgement.InputIsHigherThanTarget;
            }
            else
            {
                return Judgement.InputIsLowerThanTarget;
            }
        }
    }

}