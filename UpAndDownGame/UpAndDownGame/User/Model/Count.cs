using System;

namespace UpAndDown.User.Model
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
