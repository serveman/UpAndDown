using System;
using UpAndDown.Game.Enum;
using UpAndDown.Game.Initialize;
using UpAndDown.Interface;
using UpAndDown.User.Model;

using static UpAndDown.CustomException.CustomExceptions;

namespace UpAndDown.Service
{
    public class GameService
    {
        private const int LEVEL_INDEX_OFFSET = -1;
        private const int TRY_COUNT_INDEX_OFFSET = 1;

        private readonly IGameLevelManager gameLevelManager;
        private readonly IMemberService memberService;
        private readonly IJudgementManager judgementManager;

        private Member currentMember;
        private int tryCount;

        public GameService(IGameLevelManager gameLevelManager, IMemberService memberService, IJudgementManager judgementManager)
        {
            this.gameLevelManager = gameLevelManager;
            this.memberService = memberService;
            this.judgementManager = judgementManager;

            Run();
        }

        private void Run()
        {
            Initialize();

            bool isSuccess = true;

            try
            {
                PlayGame(out isSuccess);
            }
            catch (ExitGameByUserException e)
            {
                Console.WriteLine(e.Message);
                isSuccess = false;
            }
            finally
            {
                EndGame(isSuccess);
            }

        }

        private void Initialize()
        {
            var initializer = new GameInitializer(gameLevelManager, memberService);
            initializer.InitializeGame();

            currentMember = memberService.GetCurrentMember();
            tryCount = 0;
        }

        private void PlayGame(out bool isSuccess)
        {
            Judgement result;
            do
            {
                int currentUserInputNumber = GetUserInputNumber();

                result = judgementManager.JudgeUpOrDownResultMulti(currentUserInputNumber, gameLevelManager.TargetValuesSet);

                DisplayResultMessage(result);
            } while (gameLevelManager.TargetRemains != 0);

            isSuccess = true;   // todo: 현재는 성공만 리턴함
        }

        private void EndGame(bool isSuccess)
        {
            ApplySuccessAndFailureCount(isSuccess);

            memberService.SaveCurrentMember(currentMember);
        }

        /// <summary>
        /// 플레이한 난이도에 따라 현재 유저에게 성공 또는 실패를 카운트 하는 메서드
        /// </summary>
        /// <param name="isSuccess">성공 또는 실패 여부</param>
        private void ApplySuccessAndFailureCount(bool isSuccess)
        {
            currentMember.PlayCountList[gameLevelManager.Level + LEVEL_INDEX_OFFSET].IncreaseCount(isSuccess);
        }

        private int GetUserInputNumber()
        {
            int stepIndex = tryCount + TRY_COUNT_INDEX_OFFSET;
            int remains = gameLevelManager.TargetRemains;

            int min = gameLevelManager.GuessNumberMin;
            int max = gameLevelManager.GuessNumberMax;

            Console.WriteLine();
            Console.Write($"Step{stepIndex, 3} - [Remain {remains}] " +
                          $"숫자를 입력해주세요({min}~{max}, 0=포기): ");

            return ValidateUserInput(min, max);
        }

        private int ValidateUserInput(int min, int max)
        {
            int readUserNumber;
            bool isValid;

            do
            {
                isValid = int.TryParse(Console.ReadLine(), out readUserNumber);

                if(!isValid)
                {
                    Console.Write("잘못된 형식입니다. 숫자를 입력해주세요: ");
                }
                else if (readUserNumber == 0)
                {
                    throw new ExitGameByUserException("유저가 게임을 포기했습니다 !!");
                }
                else if (readUserNumber < min || readUserNumber > max)
                {
                    Console.Write("범위를 벗어났습니다. 다시 입력해주세요: ");
                    isValid = false;
                }
            } while (!isValid);

            tryCount++;

            return readUserNumber;
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
                    throw new Exception("뭔가 프로그램 로직이 잘못된 것 같습니다!!!");
            }
        }
    }
}
