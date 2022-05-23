using System;

namespace ApplicationExceptions
{
    public class FailedMediaMoveException : Exception
    {
        public FailedMediaMoveException (string message) : base(message)
        {
            
        }
    }
}