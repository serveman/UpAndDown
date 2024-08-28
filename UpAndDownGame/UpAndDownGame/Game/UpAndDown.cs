using UpAndDown.Core.Domain;
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
            new GameService(
                new MemberService(),
                new JudgementManager()
                );
        }

        private void Initialize()
        {
            // 지금은 초기화 할 게 없음
        }
    }
}
