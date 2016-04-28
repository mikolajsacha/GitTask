namespace GitTask.Storage.Exception
{
    public class KeyAlreadyExistsException : System.Exception
    {
        public KeyAlreadyExistsException() : base("An object with provided key already exists in database")
        {
        }

        public KeyAlreadyExistsException(object keyValue) : base($"An object with key \"{keyValue}\" already exists in database")
        {
        }

        public KeyAlreadyExistsException(string message) : base(message)
        {
        }
    }
}
