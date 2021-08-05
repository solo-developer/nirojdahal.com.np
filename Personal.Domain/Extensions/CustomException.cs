using System;

namespace Personal.Domain.Exceptions
{
    public class CustomException : Exception
    {
        public CustomException()
        {

        }
        public CustomException(string message) : base(message)
        {

        }
    }
}
