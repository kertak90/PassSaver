namespace PassSaver
{
    class Program
    {
        static void Main(string[] args)
        {
            var crypter = new CrypterService(args);
            System.Console.WriteLine("done");
        }
    }
}
