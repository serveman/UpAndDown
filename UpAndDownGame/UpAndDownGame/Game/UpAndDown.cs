using System;
using UpAndDown.Core.Domain;
using UpAndDown.Interface;
using UpAndDown.Service;

using static UpAndDown.CustomException.CustomExceptions;

namespace UpAndDown.Game
{
    public class UpAndDownGame
    {
        public UpAndDownGame()
        {
            Initialize();

            Run();
        }

        private void Run()
        {
            IGameLevelManager gameLevelManager = new GameLevelManager();

            try
            {
                new GameService(
                    gameLevelManager,
                    new MemberService(),
                    new JudgementManager(gameLevelManager)
                    );
            }
            catch (ExitGameByUserException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.WriteLine();
                Console.Write("종료하려면 엔터 키를 입력해주세요: ");
                Console.ReadLine();
            }

        }

        private void Initialize()
        {
            // 지금은 초기화 할 게 없음
        }
    }
}
