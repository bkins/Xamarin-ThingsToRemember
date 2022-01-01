using System;

namespace ApplicationExceptions
{
    [Serializable]
    public class RecordNotFoundException : Exception
    {
        public string EntityName  { get; set; }
        public string EntityTitle { get; set; }

        public RecordNotFoundException() : base() { }
        
        public RecordNotFoundException(string entityName
                                     , string entityTitle) 
                : base($"A record with the title {entityTitle} could not be found in {entityName}")
        {
            EntityName  = entityName;
            EntityTitle = entityTitle;
        }
    }
}
