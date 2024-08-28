using System;

namespace UpAndDown.CustomException
{
    public class ExitGameByUserException : Exception
    {
        public ExitGameByUserException()
        {
        }

        public ExitGameByUserException(string message) : base(message)
        {
        }
    }
}
