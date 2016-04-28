namespace GitTask.Storage
{
    // this class is used as a wrapper to simplify using IOC container
    public class StoragePath
    {
        public string Path { get; }

        public StoragePath(string path)
        {
            Path = path;
        }
    }
}