using System;

namespace Reflight.Core.FlightAcademy
{
    public class InvalidLoginException : Exception
    {
        public InvalidLoginException(string message) : base(message)
        {
        }
    }
}