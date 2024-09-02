using System;
using UpAndDown.Game;

namespace UpAndDown
{
    class Program
    {
        static void Main(string[] args)
        {
            new UpAndDownGame();

            Console.WriteLine("엔터 키를 눌러주세요.");
            Console.ReadLine();
            //new History.History();
        }
    }
}
