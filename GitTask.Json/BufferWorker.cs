using System.Text;

namespace GitTask.Json
{
    public static class BufferWorker
    {
        private static readonly Encoding Encoding = Encoding.UTF8;

        public static string ToString(byte[] buffer)
        {
            return Encoding.GetString(buffer);
        }

        public static byte[] ToBuffer(string str)
        {
            return Encoding.GetBytes(str);
        }
    }
}
