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
            int targetValue = GenerateRandomTargetValue();

#if DEBUG
            new Game.Game(this.ms, 50);     // 테스트를 위해 50으로 고정
#else
            new Game.Game(this.ms, targetValue);
#endif
        }

        private void Initialize()
        {
            // 지금은 초기화 할 게 없음
        }

        private int GenerateRandomTargetValue()
        {
            //Random r = new Random(unchecked((int)DateTime.Now.Ticks));
            Random r = new Random();    // C# 4.0부터는 시드를 입력하지 않아도 충분히 안전하게 난수를 발생해준다. 시작 시 항상 같은 난수를 발생하고싶은 경우만 사용
            return r.Next(1, 100);
        }
    }
}
