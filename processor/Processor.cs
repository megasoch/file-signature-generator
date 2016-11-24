using file_signature_gen.hasher;
using file_signature_gen.input;
using file_signature_gen.output;
using System;
using System.Collections.Generic;
using System.Threading;

namespace file_signature_gen.processor
{
    class Processor
    {
        private int threadCount = 4;
        private long currentChunk = 1;
        private Dictionary<long, byte[]> dict = new Dictionary<long, byte[]>();
        private List<Thread> processingThreads = new List<Thread>();
        private Hasher hasher;
        private int processedChunksCount = 0;

        public Processor() { }

        public Processor(int threadCount, Hasher hasher)
        {
            this.threadCount = threadCount;
            this.hasher = hasher;
        }

        public void Start(object buffer)
        {
            ByteBufferOnArray byteBuffer = (ByteBufferOnArray)buffer;
            while (byteBuffer.HasAlmostOneChunk())
            {
                processedChunksCount = 0;
                dict.Clear();
                processingThreads.Clear();
                for (int i = 0; i < threadCount; i++)
                {
                    if (byteBuffer.HasAlmostOneChunk())
                    {
                        try
                        {
                            byte[] chunk = byteBuffer.GetChunk();
                            long number = currentChunk;

                            Thread thread = new Thread(() => ProcessChunk(chunk, number));
                            processingThreads.Add(thread);
                            thread.Start();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                        }

                        processedChunksCount++;
                        currentChunk++;
                    }
                }
                foreach (Thread thread in processingThreads)
                    thread.Join();

                int id = 0;
                foreach (byte[] chunk in GetOrderedChunks())
                {
                    ConsoleOutputter.Write(currentChunk - processedChunksCount + id, chunk);
                    id++;
                }
            }
            if (byteBuffer.NotProcessedBytesCount() > 0)
            {
                ConsoleOutputter.Write(currentChunk, hasher.ComputeHash(byteBuffer.GetLastChunk()));
            }
        }

        private void ProcessChunk(object chunk, object chunkNumber)
        {
            try
            {
                lock (dict)
                {
                    dict.Add((long)chunkNumber, hasher.ComputeHash((byte[])chunk));
                    Monitor.Pulse(dict);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }

        private List<byte[]> GetOrderedChunks()
        {
            List<byte[]> chunks = new List<byte[]>();
            try
            {
                for (int i = 0; i < processedChunksCount; i++)
                {
                    chunks.Add(dict[currentChunk - processedChunksCount + i]);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
            return chunks;
        }
    }
}
