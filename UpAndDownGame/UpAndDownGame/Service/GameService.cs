using System;
using UpAndDown.Core.Domain;
using UpAndDown.CustomException;
using UpAndDown.Game.Enum;
using UpAndDown.Interface;
using UpAndDown.User.Model;

namespace UpAndDown.Service
{
    public class GameService : GameLevelManager
    {
        private const int LEVEL_INDEX_OFFSET = -1;
        private const int TRY_COUNT_INDEX_OFFSET = 1;

        private readonly IMemberService memberService;
        private readonly IJudgementManager judgementManager;

        private Member CurrentMember { get; set; }
        private int TryCount { get; set; }

        public GameService(IMemberService memberService, IJudgementManager judgementManager)
        {
            this.memberService = memberService;
            this.judgementManager = judgementManager;

            this.Run();
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
            // MemberService 내부 필드에서 members 정보를 관리한다
            memberService.ReadMembersInformation();
            memberService.SelectMemberInformation();
            CurrentMember = memberService.GetCurrentMember();

            this.TryCount = 0;

            SelectGameLevel();
        }

        private void PlayGame(out bool isSuccess)
        {
            Judgement result;
            do
            {
                int currentUserInputNumber = InputUserNumber();

                result = judgementManager.JudgeUpOrDownResultMulti(currentUserInputNumber, TargetValuesSet);
                DisplayResultMessage(result);
            } while (!judgementManager.IsSolvedTargetAll(TargetValuesSet));

            isSuccess = true;   // todo: 현재는 성공만 리턴함
        }

        private void EndGame(bool isSuccess)
        {
            ApplySuccessAndFailureCount(isSuccess);

            memberService.SaveCurrentMember(CurrentMember);
            memberService.UpdateMembersInformation();
        }

        /// <summary>
        /// 플레이한 난이도에 따라 현재 유저에게 성공 또는 실패를 카운트 하는 메서드
        /// </summary>
        /// <param name="isSuccess">성공 또는 실패 여부</param>
        private void ApplySuccessAndFailureCount(bool isSuccess)
        {
#if (false) // Count 를 struct 로 선언했을 때는 이렇게 해야 정상적으로 값이 들어간다    -> 왜?
            Count cnt = currentMember.PlayCountList[level + LEVEL_OFFSET];
            cnt.IncreaseCount(isSuccess);
            currentMember.PlayCountList[level + LEVEL_OFFSET] = cnt;

#else       // Count 를 class 로 선언했을 때는 이렇게 해도 정상적으로 값이 증가한다
            CurrentMember.PlayCountList[Level + LEVEL_INDEX_OFFSET].IncreaseCount(isSuccess);

#endif
        }

        private int InputUserNumber()
        {
            Console.WriteLine();
            Console.Write($"Step{this.TryCount + TRY_COUNT_INDEX_OFFSET, 3} - [Remain {judgementManager.FindTargetRemain(TargetValuesSet)}] " +
                          $"숫자를 입력해주세요({GetGuessNumberMin()}~{GetGuessNumberMax()}, 0=포기): ");

            int readUserNumber;
            while (!int.TryParse(Console.ReadLine(), out readUserNumber) ||
                   readUserNumber < GetGuessNumberMin() || readUserNumber > GetGuessNumberMax())
            {
                if(readUserNumber == 0)
                {
                    throw new ExitGameByUserException("유저가 게임을 포기했습니다 !!");
                }

                Console.WriteLine($"잘못된 형식이거나 범위를 벗어났습니다. 다시 입력해주세요 ({GetGuessNumberMin()}~{GetGuessNumberMax()}).");
            }

            this.TryCount++;

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
