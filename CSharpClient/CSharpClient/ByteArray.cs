using System;

namespace CSharpClient
{
    public class ByteArray
    {
        private const int DEFAULT_SIZE = 1024; // Default size.
        private int _initSize = 0; // Init size.

        // Byte buffer.
        public byte[] bytes;

        // Read and write position.
        public int readIndex;

        public int writeIndex;

        // Capacity.
        private int _capacity = 0;

        // Remain capacity.
        public int Remain
        {
            get { return _capacity - writeIndex; }
        }

        // Length of the buffer.
        public int Length
        {
            get { return writeIndex - readIndex; }
        }

        // Constructor, use in receive.
        public ByteArray(int size = DEFAULT_SIZE)
        {
            bytes = new byte[size];
            _capacity = size;
            _initSize = size;
            readIndex = 0;
            writeIndex = 0;
        }

        // Constructor, use in send.
        public ByteArray(byte[] defaultBytes)
        {
            bytes = defaultBytes;
            _capacity = defaultBytes.Length;
            _initSize = defaultBytes.Length;
            readIndex = 0;
            writeIndex = defaultBytes.Length;
        }

        /// <summary>
        /// Expand byte buffer. 
        /// </summary>
        /// <param name="size">The amount of data space required.</param>
        public void ReSize(int size)
        {
            // Send space.
            if (size < Length)
                return;
            // Receive space.
            if (size < _initSize)
                return;

            int n = 1;
            while (n < size)
                n *= 2;
            // Then new n is 1, 2, 4, 8, 16, 32, 64, 128, 256...
            _capacity = n;
            // Move then old byte array to the new byte array.
            byte[] newBytes = new byte[_capacity];
            Array.Copy(bytes, readIndex, newBytes, 0, Length);
            bytes = newBytes;
            // Reset read and write index.
            writeIndex = Length;
            readIndex = 0;
        }

        public void CheckAndMoveBytes()
        {
            if (Length < 8)
                MoveBytes();
        }

        public void MoveBytes()
        {
            // Move from readIdx to begin.
            Array.Copy(bytes, readIndex, bytes, 0, Length);
            writeIndex = Length;
            readIndex = 0;
        }

        /// <summary>
        /// 数据的写入
        /// </summary>
        /// <param name="bs">待写入的数据</param>
        /// <param name="offset">可以写入数据的下标</param>
        /// <param name="count">待写入的数据长度</param>
        /// <returns></returns>
        public int Write(byte[] bs, int offset, int count)
        {
            if (Remain < count)
                ReSize(Length + count);
            // 将bs加入到bytes(缓冲区)中
            Array.Copy(bs, offset, bytes, writeIndex, count);
            writeIndex += count;
            return count;
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="bs">读取的数据存放数组</param>
        /// <param name="offset">从哪个下标开始读，这是下标</param>
        /// <param name="count">读取多长，这是长度</param>
        /// <returns></returns>
        public int Read(byte[] bs, int offset, int count)
        {
            count = Math.Min(count, Length);
            // 将bytes(缓冲区)中的数据读取到bs中，bs从offset开始写入数据，count是读取多少个数据
            Array.Copy(bytes, 0, bs, offset, count);
            readIndex += count;
            CheckAndMoveBytes();
            return count;
        }

        public Int16 ReadInt16()
        {
            if (Length < 2)
                return 0;
            Int16 ret = (Int16) ((bytes[1] << 8) | bytes[0]);
            readIndex += 2;
            CheckAndMoveBytes();
            return ret;
        }

        public Int32 ReadInt32()
        {
            if (Length < 4)
                return 0;
            Int32 ret = (Int32) ((bytes[3] << 24) | (bytes[2] << 16) | (bytes[1] << 8) | bytes[0]);
            readIndex += 4;
            CheckAndMoveBytes();
            return ret;
        }
    }
}