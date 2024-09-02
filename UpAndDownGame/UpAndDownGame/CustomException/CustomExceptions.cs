using System;

namespace UpAndDown.CustomException
{
    public class CustomExceptions
    {
        public class ExitGameByUserException : Exception
        {
            public ExitGameByUserException(string message) : base(message) { }
        }

        public class MemberNotFoundException : Exception
        {
            public MemberNotFoundException(string message) : base(message) { }
        }
    }
}
