using System;

namespace CSharpClient
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Start");
            string ip = "127.0.0.1";
            int port = 8888;
            
            NetManager.Instance.Connect(ip, port);

            Console.ReadKey();
        }
    }
}