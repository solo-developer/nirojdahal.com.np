﻿namespace Personal.Domain.Exceptions
{
    public class NonEmptyValueException : CustomException
    {
        public NonEmptyValueException(string message = "Value must be provided.") : base(message)
        {

        }
    }
}
