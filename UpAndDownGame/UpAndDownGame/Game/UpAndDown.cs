using UpAndDown.Core.Domain;
using UpAndDown.Interface;
using UpAndDown.Service;

namespace UpAndDown.Game
{
    public class UpAndDownGame
    {
        public UpAndDownGame()
        {
            this.Initialize();

            this.Run();
        }

        private void Run()
        {
            IGameLevelManager gameLevelManager = new GameLevelManager();

            new GameService(
                gameLevelManager,
                new MemberService(),
                new JudgementManager(gameLevelManager)
                );
        }

        private void Initialize()
        {
            // 지금은 초기화 할 게 없음
        }
    }
}
