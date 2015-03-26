using System;
using System.Security.Cryptography;

namespace ServerReplayPlayer.Logic.Cryptographer
{
    public class TripleDesCryptographer
    {
        private readonly byte[] iv;
        private readonly byte[] key;

        public TripleDesCryptographer(string[] keyParts)
        {
            if (keyParts.Length != 2)
                throw new Exception("Invalid TripleDesKey description");
            key = Convert.FromBase64String(keyParts[0]);
            iv = Convert.FromBase64String(keyParts[1]);
        }

        public TripleDesCryptographer(byte[] key, byte[] iv)
        {
            this.key = key;
            this.iv = iv;
        }

        public byte[] Encrypt(byte[] input)
        {
            using (var tripleDes = TripleDES.Create())
            using (var tripleDesEncryptor = tripleDes.CreateEncryptor(key, iv))
                return tripleDesEncryptor.TransformFinalBlock(input, 0, input.Length);
        }

        public byte[] Decrypt(byte[] source)
        {
            using (var tripleDes = TripleDES.Create())
            using (var tripleDesEncryptor = tripleDes.CreateDecryptor(key, iv))
                return tripleDesEncryptor.TransformFinalBlock(source, 0, source.Length);
        }
    }
}
