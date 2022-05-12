using System;

namespace Cryptocop.Software.API.Models.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Model not found") {}
    }
}