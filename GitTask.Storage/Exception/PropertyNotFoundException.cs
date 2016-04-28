using System;

namespace GitTask.Storage.Exception
{
    public class PropertyNotFoundException : System.Exception
    {
        public PropertyNotFoundException() : base("Provided type does not have a property with this name.")
        {
        }

        public PropertyNotFoundException(string propertyName, Type type) : base($"Type \"{type}\" does not have a property named \"{propertyName}\"")
        {
        }

        public PropertyNotFoundException(string message) : base(message)
        {
        }
    }
}
