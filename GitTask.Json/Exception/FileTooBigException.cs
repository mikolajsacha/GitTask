namespace GitTask.Json.Exception
{
    public class FileTooBigException : System.Exception 
    {
        public FileTooBigException() : base("Provided file is too big")
        {
        }

        public FileTooBigException(string fileName, long maxLengthInBytes) : base($"File \"{fileName}\" is too big (maximum file length is {maxLengthInBytes} bytes)")
        {
        }

        public FileTooBigException(string message) : base(message)
        {
        }
    }
}
