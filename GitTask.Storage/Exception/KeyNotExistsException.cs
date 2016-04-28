namespace GitTask.Storage.Exception
{
    public class KeyNotExistsException : System.Exception
    {
        public KeyNotExistsException() : base("An object with provided key does not exist in database")
        {
        }

        public KeyNotExistsException(object keyValue) : base($"An object with key \"{keyValue}\" does not exist in database")
        {
        }

        public KeyNotExistsException(string message) : base(message)
        {
        }
    }
}
