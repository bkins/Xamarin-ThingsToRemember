using System;

namespace ApplicationExceptions
{
    [Serializable]
    public class SequenceContainsNoElementsException : Exception
    {
        public string EntityName { get; set; }
        public SequenceContainsNoElementsException() { }

        public SequenceContainsNoElementsException(string message)
                : base(message) { }

        public SequenceContainsNoElementsException(string message, Exception inner)
                : base(message, inner) { }

        public SequenceContainsNoElementsException(string    message
                                                 , string    entityName
                                                 , Exception inner)
        : base(message, inner)
        {
            EntityName = entityName;
        }
    }
}
