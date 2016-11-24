using System;

namespace file_signature_gen.output
{
    class ConsoleOutputter
    {
        public static void Write(long chunkNumber, byte[] hash)
        {
            Console.Write(chunkNumber + " ");
            PrintByteArray(hash);
        }

        private static void PrintByteArray(byte[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(String.Format("{0:X2}", array[i]));
                if ((i % 4) == 3) Console.Write(" ");
            }
            Console.WriteLine();
        }
    }
}
