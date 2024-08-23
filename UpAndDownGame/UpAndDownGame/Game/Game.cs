using System;
using UpAndDown.Enum;

namespace UpAndDown.Game
{
    public class Game
    {
        private int targetNumber;

        public Game(int targetNumber)
        {
            this.targetNumber = targetNumber;
            this.Run();
        }

        private void Run()
        {
            Judgement result;
            do
            {
                int currentUserInputNumber;

                // 시작
                currentUserInputNumber = InputUserNumber();

                // 판정
                result = GameUtil.JudgeUpOrDownResult(userInput: currentUserInputNumber, targetValue: this.targetNumber);
                DisplayResultMessage(result);
            } while (result != Judgement.Equal);

            // 종료

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

            return readUserNumber;
        }


        /// <summary>
        /// 메세지 출력
        /// </summary>
        /// <returns></returns>
        private void DisplayUserInputMessage()
        {
            Console.Write("숫자를 입력해주세요: ");
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
