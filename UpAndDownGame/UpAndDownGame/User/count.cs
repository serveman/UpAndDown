using System;

namespace UpAndDown.User
{
    public struct Count
    {
        public int Total
        {
            get { return Success + Failure; } 
        }
        public int Success { get; set; }
        public int Failure { get; set; }

        public void IncreaseSuccessCount() { this.Success++; }
        public void IncreaseFailureCount() { this.Failure++; }
    }
}
