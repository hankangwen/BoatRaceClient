using System;
using System.Collections.Generic;
using System.Net.Sockets;
using BoatRace;

namespace CSharpClient
{
    public class NetManager : Singleton<NetManager>
    {
        private Socket _socket;
        private ByteArray _readBuff;
        private ProtobufUtil _pbUtil;

        NetManager()
        {
            _pbUtil = ProtobufUtil.Instance;
        }

        public void Send(byte[] sendBytes)
        {
            _socket.BeginSend(sendBytes, 0, sendBytes.Length, 
                SocketFlags.None, SendCallback, _socket);
        }

        private void SendCallback(IAsyncResult ar)
        {
            Console.WriteLine("123");
            Console.ReadKey();
        }
        
        public void Connect(string ip, int port)
        {
            if (_socket != null && _socket.Connected) {
                Debug.LogError("已经连接上了，不能重复连接");
                return;
            }
            
            // 初始化缓冲区
            _readBuff = new ByteArray();
            
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.NoDelay = true;
            _socket.BeginConnect(ip, port, ConnectCallback, _socket);
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket socket = (Socket)ar.AsyncState;
                socket.EndConnect(ar);
                Console.WriteLine("连接成功!");
                
                OnConnectSuc(socket);
            }
            catch (SocketException e)
            {
                Console.WriteLine("连接失败：" + e.Message);
                throw;
            }
        }
        
        private void OnConnectSuc(Socket socket)
        {
            TestProtobuf();

            // socket.BeginReceive(_readBuff.bytes, _readBuff.writeIndex, _readBuff.Remain,
            //     SocketFlags.None, ReceiveCallback, _socket);
        }
        
        // Test Protobuf
        private void TestProtobuf()
        {
            ProtobufUtil pbUtil = ProtobufUtil.Instance; 
            Hero hero = new Hero
            {
                info = new Info
                {
                    age = 18,
                    name = "小乔",
                    sex = "女"
                },
                job = Hero.Job.Swords,
                equip = new List<string>{"5","2","0"}
            };
            var result = pbUtil.ObjectToBytes(hero);
            Send(result);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                Socket socket = (Socket)ar.AsyncState;
                
                // 获取接收数据的长度
                int count = socket.EndReceive(ar);
                if (count == 0)
                {
                    Console.WriteLine("接收到0字节，关闭连接");
                    // Close();
                    return;
                }
                
                _readBuff.writeIndex += count;
                // 处理消息
                OnReceiveData();
                // 继续接收消息
                if (_readBuff.Remain < 8) {
                    _readBuff.MoveBytes();
                    _readBuff.ReSize(_readBuff.Length * 2);
                }
                
                socket.BeginReceive(_readBuff.bytes, _readBuff.writeIndex, _readBuff.Remain,
                    SocketFlags.None, ReceiveCallback, socket);
            }
            catch (SocketException e)
            {
                Console.WriteLine("接收失败：" + e.Message);
            }
        }
            
        private void OnReceiveData()
        {
            Console.WriteLine(_readBuff);
        }
    }
}