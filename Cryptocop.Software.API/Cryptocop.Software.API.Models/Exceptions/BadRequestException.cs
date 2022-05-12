using System;

namespace Cryptocop.Software.API.Models.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException() : base("A bad request") {}
    }
}