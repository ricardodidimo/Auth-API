using System;
using System.Collections;
using System.Collections.Generic;

namespace api.Models.Responses
{
    public class DomainException : Exception
    {
        public int StatusCode { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; }

        public DomainException(int statusCode, IEnumerable<string> messages)
        {
            StatusCode = statusCode;
            ErrorMessages = messages;
        }
    }
}