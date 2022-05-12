using System;

namespace Cryptocop.Software.API.Models.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException() : base("A bad request") {}
    }
}