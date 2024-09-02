using UpAndDown.Interface;

namespace UpAndDown.Game.Initialize
{
    public class GameInitializer
    {
        private readonly IGameLevelManager gameLevelManager;
        private readonly IMemberService memberService;

        public GameInitializer(IGameLevelManager gameLevelManager, IMemberService memberService)
        {
            this.gameLevelManager = gameLevelManager;
            this.memberService = memberService;
        }

        public void InitializeGame()
        {
            memberService.ReadMembersInformation();
            
            memberService.HandleMemberSelection();

            gameLevelManager.SelectGameLevel();
        }
    }
}
