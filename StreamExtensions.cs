using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Extensions
{
    public static class StreamExtensions
    {
        public static byte[] GenerateHash(this Stream stream)
        {
            return stream.ToByteArray().GenerateHash();
        }

        public static byte[] ToByteArray(this Stream stream)
        {
            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);
            byte[] total_byte_arry = new byte[stream.Length];
            byte[] buffer = new byte[4096];

            int size = stream.Read(buffer, 0, 4096);
            int offset = 0;
            while (size > 0)
            {
                Buffer.BlockCopy(buffer, 0, total_byte_arry, offset, size);
                offset += size;
                size = stream.Read(buffer, 0, 4096);
            }
            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);

            return total_byte_arry;
        }
    }
}
