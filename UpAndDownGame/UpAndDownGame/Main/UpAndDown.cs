using System;
using UpAndDown.User;

namespace UpAndDown.Main
{
    public class UpAndDown
    {
        private readonly MemberService ms = new MemberService();

        public UpAndDown()
        {
            this.Initialize();

            ms.ReadMemberInfomation();

            this.Run();

            ms.UpdateMemberInformation();
        }

        private void Run()
        {
            new Game.Game(this.ms);
        }

        private void Initialize()
        {
            // 지금은 초기화 할 게 없음
        }
    }
}
