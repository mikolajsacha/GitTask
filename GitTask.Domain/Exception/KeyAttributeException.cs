using System;

namespace GitTask.Domain.Exception
{
    public class KeyAttributeException : System.Exception
    {
        public KeyAttributeException() : base("There has been an error related to the [Key] attribute")
        {
        }

        public KeyAttributeException(Type type) : base($"There has been an error related to the [Key] attribute of type \"{type.Name}\"")
        {
        }

        public KeyAttributeException(string message) : base(message)
        {
        }
    }
}
