using System;

namespace UpAndDown.User
{
    public struct Count
    {
        private int total;
        private int success;
        private int failure;

        public int Total { get; }
        public int Success { get; }
        public int Failure { get; }

        public void IncreaseSuccessCount() { this.success++; UpdateTotal(); }
        public void IncreaseFailureCount() { this.failure++; UpdateTotal(); }

        private void UpdateTotal() { total = this.success + this.failure; }
    }
}
