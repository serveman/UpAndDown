using System;
using UpAndDown.User;

namespace UpAndDown.Game
{
    public class Game : GameLevel
    {
        private Member member = new Member();

        private int TryCount { get; set; }

        private readonly MemberService ms;

        public Game(MemberService ms)
        {
            this.ms = ms;
            this.TryCount = 0;
            this.Run();
        }

        private void Run()
        {
            SelectGameLevel();

            member = ms.GetCurrentMember();

            Judgement result;
            do
            {
                // 시작
                int currentUserInputNumber = InputUserNumber();

                // 판정
                result = GameUtil.JudgeUpOrDownResultMulti(userInput: currentUserInputNumber, targetValues: targetValues);
                DisplayResultMessage(result);
            } while (!GameUtil.IsSolvedTargetAll(targetValues));

            // 종료
            Count cnt = member.PlayCountList[level - 1];
            cnt.IncreaseSuccessCount();
            member.PlayCountList[level - 1] = cnt;

            ms.SaveCurrentMember(member);
        }

        /// <summary>
        /// 유저가 숫자를 입력하는 메서드
        /// </summary>
        /// <returns>유저가 입력한 숫자</returns>
        private int InputUserNumber()
        {
            DisplayUserInputMessage();

            int readUserNumber;
            while (!int.TryParse(Console.ReadLine(), out readUserNumber))
            {
                Console.WriteLine("잘못된 형식입니다. 다시 입력해주세요.");
            }

            this.TryCount++;
            return readUserNumber;
        }


        /// <summary>
        /// 메세지 출력
        /// </summary>
        /// <returns></returns>
        private void DisplayUserInputMessage()
        {
            Console.WriteLine();
            Console.Write($"[Step{this.TryCount,3}] 숫자를 입력해주세요: ");
        }

        private void DisplayResultMessage(Judgement result)
        {
            switch (result)
            {
                case Judgement.InputIsHigherThanTarget:
                    Console.WriteLine($"입력한 값이 목표값보다 큽니다.");
                    break;

                case Judgement.InputIsLowerThanTarget:
                    Console.WriteLine($"입력한 값이 목표값보다 작습니다.");
                    break;

                case Judgement.Equal:
                    Console.WriteLine("정답입니다!");
                    break;

                default:
                    Console.WriteLine("뭔가 프로그램 로직이 잘못된 것 같습니다!!!");
                    break;
            }
        }
    }
}
