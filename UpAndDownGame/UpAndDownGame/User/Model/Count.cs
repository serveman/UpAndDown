using System;

namespace UpAndDown.User
{
    public class Count
    {
        public int Success { get; set; }
        public int Failure { get; set; }
        public int Total { get; private set; }

        public Count()
        {
            Success = 0;
            Failure = 0;
            Total = 0;
        }

        // Clone 시 이용하기 위한 생성자
        private Count(int s, int f)
        {
            Success = s;
            Failure = f;
            Total = s + f;
        }

        public void IncreaseCount(bool isSuccess)
        {
            if (isSuccess)
            {
                this.Success++;
            }
            else
            {
                this.Failure++;
            }

            Total = Success + Failure;
        }
    }
}
