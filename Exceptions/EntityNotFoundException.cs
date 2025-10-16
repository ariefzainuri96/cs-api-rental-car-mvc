using System;

namespace cs_api_rental_car_mvc.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message) : base(message) { }

        // Allows chaining the inner exception if one occurred during reflection
        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}