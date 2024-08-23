using System;

namespace UpAndDown.Main
{
    public class UpAndDown
    {
        public UpAndDown()
        {
            this.Initialize();

            this.Run();
        }

        private void Run()
        {
            int targetValue = GenerateRandomTargetValue();

            Game.Game game = new Game.Game(targetValue);
        }

        private void Initialize()
        {
            // ������ �ʱ�ȭ �� �� ����
        }

        private int GenerateRandomTargetValue()
        {
            //Random r = new Random(unchecked((int)DateTime.Now.Ticks));
            Random r = new Random();    // C# 4.0���ʹ� �õ带 �Է����� �ʾƵ� ����� �����ϰ� ������ �߻����ش�. ���� �� �׻� ���� ������ �߻��ϰ���� ��츸 ���
            return r.Next(1, 100);
        }
    }
}
