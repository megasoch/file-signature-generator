namespace file_signature_gen.hasher
{
    interface Hasher
    {
        byte[] ComputeHash(byte[] data);
    }
}
