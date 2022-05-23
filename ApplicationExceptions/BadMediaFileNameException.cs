using System;

namespace ApplicationExceptions
{
    public class BadMediaFileNameException : Exception
    {
        public BadMediaFileNameException (string fileName, string slug) : base(BuildMessage(fileName, slug))
        {
            
        }

        private static string BuildMessage(string fileName
                                         , string slug)
        {
            return $"The file name provided: {fileName}, does not contain the proper slug: {slug}. ";
        }
    }
}