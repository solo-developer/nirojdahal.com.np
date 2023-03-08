namespace Personal.Domain.Exceptions
{
    public class InvalidStateTransitionException:CustomException
    {
        public InvalidStateTransitionException(string message="Requested transition state is invalid."):base(message)
        {

        }
    }
}
