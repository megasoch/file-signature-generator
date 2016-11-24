using System;
using System.IO;

namespace file_signature_gen.input
{
    class FileByteInputer
    {
        private const int CHUNK_SIZE = 64 * 1024; // read the file by chunks of 64KB
        private FileStream file;
        private byte[] buffer;

        public FileByteInputer(FileStream file)
        {
            this.file = file;
            buffer = new byte[CHUNK_SIZE];
        }

        public byte[] Next()
        {
            int bytesRead;
            if (file.Position != file.Length)
            {
                bytesRead = file.Read(buffer, 0, buffer.Length);
                if (bytesRead < CHUNK_SIZE)
                {
                    var cuttedBuffer = new byte[bytesRead];
                    Array.Copy(buffer, cuttedBuffer, bytesRead);
                    return cuttedBuffer;
                }
                return buffer;
            }
            return new byte[0];
        }

        public byte NextByte()
        {
            return (byte)file.ReadByte();
        }

        public bool HasNext()
        {
            return file.Position != file.Length;
        }

        public int GetChunkSize()
        {
            return CHUNK_SIZE; 
        }
    }
}
