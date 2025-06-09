namespace Extractor
{
    internal class ConsoleMessageWriter : IMessageWriter
    {
        public void WriteLine(string value)
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {value}");
        }
    }
}
