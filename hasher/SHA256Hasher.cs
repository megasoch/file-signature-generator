using System.Security.Cryptography;

namespace file_signature_gen.hasher
{
    class SHA256Hasher : Hasher
    {
        private SHA256 sha256 = SHA256Managed.Create();
        public byte[] ComputeHash(byte[] data)
        {
            return sha256.ComputeHash(data);
        }
    }
}
