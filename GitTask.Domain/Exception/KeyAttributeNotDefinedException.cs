using System;

namespace GitTask.Domain.Exception
{
    public class KeyAttributeNotDefinedException : KeyAttributeException
    {
        public KeyAttributeNotDefinedException() : base("Provided type has no [Key] attribute")
        {
        }

        public KeyAttributeNotDefinedException(Type type) : base($"Type \"{type.Name}\" has no [Key] attribute")
        {
        }
        
        public KeyAttributeNotDefinedException(string message)
        : base(message)
        {
        }
    }
}
