using System.Collections;

namespace file_signature_gen.input
{
    class ByteBufferOnArray
    {
        private int bufferSize = 32 * 1024 * 1024; // 32MB buffer
        private int processedChunkSize; // size of processed chunk
        private ArrayList buffer = new ArrayList();
        private int currentChunkPosition = 0;

        public ByteBufferOnArray(int processedChunkSize)
        {
            this.processedChunkSize = processedChunkSize;
            bufferSize = (bufferSize / processedChunkSize + 1) * processedChunkSize;
            buffer.Capacity = bufferSize;
        }

        public void InsertNext(byte[] inserted)
        {
            foreach (byte b in inserted)
            {
                buffer.Add(b);
            }
        }

        public void InsertByte(byte b)
        {
            buffer.Add(b);
        }

        public int BytesForAlliquotSize()
        {
            return (processedChunkSize - buffer.Count % processedChunkSize) % processedChunkSize;
        }

        public bool HasAlmostOneChunk()
        {
            return buffer.Count - currentChunkPosition >= processedChunkSize;
        }

        public byte[] GetChunk()
        {
            int chunkLeftBound = currentChunkPosition;
            currentChunkPosition += processedChunkSize;
            return (byte[])buffer.GetRange(chunkLeftBound, processedChunkSize).ToArray(typeof(byte));
        }

        public bool HasSpaceForChunk(int size)
        {
            return buffer.Count + size <= bufferSize;
        }

        public byte[] GetLastChunk()
        {
            return (byte[])buffer.GetRange(currentChunkPosition, buffer.Count - currentChunkPosition).ToArray(typeof(byte));
        }

        public int NotProcessedBytesCount()
        {
            return buffer.Count - currentChunkPosition;
        }

        public void Clear()
        {
            buffer.Clear();
            currentChunkPosition = 0;
        }
    }
}
