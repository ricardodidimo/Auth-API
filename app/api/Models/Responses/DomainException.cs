using System;

namespace api.Models.Responses
{
    public class DomainException : Exception
    {
        public int StatusCode { get; set; }
        public string[] ErrorMessages { get; set; }

        public DomainException(int statusCode, string[] messages)
        {
            StatusCode = statusCode;
            ErrorMessages = messages;
        }
    }
}