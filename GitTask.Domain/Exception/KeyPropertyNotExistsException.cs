using System;

namespace GitTask.Domain.Exception
{
    public class KeyPropertyNotExistsException : KeyAttributeException
    {
        public KeyPropertyNotExistsException() : base("Property with a name provided in [Key] attribute does not exist")
        {
        }

        public KeyPropertyNotExistsException(Type type) : base($"Property with a name provided in [Key] attribute does not exist in type \"{type.Name}\"")
        {
        }

        public KeyPropertyNotExistsException(string message) : base(message)
        {
        }
    }
}
