using System;

namespace CSharpClient
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Start");
            // string ip = "127.0.0.1";
            string ip = "1.14.208.107";
            int port = 8888; //轻量云主机可用80和443端口
            
            NetManager.Instance.Connect(ip, port);

            Console.ReadKey();
        }
    }
}