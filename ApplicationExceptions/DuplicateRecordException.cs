using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationExceptions
{
    [Serializable]
    public class DuplicateRecordException : Exception
    {
        public string EntityName  { get; set; }
        public string EntityTitle { get; set; }

        public DuplicateRecordException() { }

        public DuplicateRecordException(string message) 
                : base(message) { }

        public DuplicateRecordException(string    message
                                      , Exception inner) 
                : base(message
                     , inner) { }

        public DuplicateRecordException(string    entityName
                                      , string    entityTitle) 
                : base($"A record in the table {entityName} already has a title of {entityTitle}.")
        {
            EntityName  = entityName;
            EntityTitle = entityTitle;
        }
    }
}
