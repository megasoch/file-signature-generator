using file_signature_gen.hasher;
using file_signature_gen.input;
using file_signature_gen.processor;
using System;
using System.IO;
using System.Threading;

namespace file_signature_gen
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                String fileName = args[0];
                int chunkSize = Int32.Parse(args[1]);

                ByteBufferOnArray byteBuffer1 = new ByteBufferOnArray(chunkSize);
                ByteBufferOnArray byteBuffer2 = new ByteBufferOnArray(chunkSize);
                ByteBufferOnArray byteBuffer = byteBuffer1;
                bool firstBuffer = true;

                Processor processor = new Processor(Environment.ProcessorCount, new SHA256Hasher());
                Thread processingBufferThread = new Thread(processor.Start);

                using (FileStream file = File.OpenRead(fileName))
                {
                    FileByteInputer inputer = new FileByteInputer(file);
                    while (inputer.HasNext())
                    {
                        if (byteBuffer.HasSpaceForChunk(inputer.GetChunkSize()))
                        {
                            byteBuffer.InsertNext(inputer.Next());
                        }
                        else if (byteBuffer.BytesForAlliquotSize() != 0)
                        {
                            byteBuffer.InsertByte(inputer.NextByte());
                        }
                        else
                        {
                            if (processingBufferThread.IsAlive)
                                processingBufferThread.Join();
                            processingBufferThread = new Thread(processor.Start);
                            processingBufferThread.Start(byteBuffer);

                            byteBuffer = byteBuffer2;
                            if (!firstBuffer)
                                byteBuffer = byteBuffer1;
                            firstBuffer = !firstBuffer;

                            byteBuffer.Clear();
                        }
                    }
                    if (processingBufferThread.IsAlive)
                        processingBufferThread.Join();
                    processor.Start(byteBuffer);
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Not enought arguments: Need path to file and processed chunk size");
            }
            catch (FormatException)
            {
                Console.WriteLine("Second parameter must be an integer");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
}
