using System;
using System.IO;
using ProtoBuf;

namespace BoatRace
{
    public class ProtobufUtil : Singleton<ProtobufUtil>
    {
        ProtobufUtil() { }

        public byte[] ObjectToBytes<T>(T instance)
        {
            try
            {
                byte[] array;
                if (instance == null)
                {
                    array = Array.Empty<byte>();
                }
                else
                {
                    using (MemoryStream memory = new MemoryStream())
                    {
                        ProtoBuf.Serializer.Serialize(memory, instance);
                        array = memory.ToArray();
                    }
                }

                return array;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.ToString());
                return Array.Empty<byte>();
            }
        }

        public T BytesToObject<T>(byte[] bytesData)
        {
            if (bytesData.Length == 0)
            {
                return default(T);
            }

            try
            {
                using (MemoryStream memory = new MemoryStream(bytesData))
                {
                    T result = Serializer.Deserialize<T>(memory);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.ToString());
                return default(T);
            }
        }
    }
}