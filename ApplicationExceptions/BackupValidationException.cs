using System;

namespace ApplicationExceptions
{
    public class BackupValidationException : Exception
    {
        public string LogContents { get; }

        public BackupValidationException (string message, string logContents) : base(message)
        {
            LogContents = logContents;
        }

        public BackupValidationException (string    message
                                        , string    logContents
                                        , Exception innerException)
            : base(message
                 , innerException)
        {
            LogContents = logContents;
        }
    }
}