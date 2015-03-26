using System;
using System.IO;
using System.Text;

namespace ServerReplayPlayer.Logic.Cryptographer
{
    public class TokenSerializer
    {
        private readonly TripleDesCryptographer cryptographer;

        public TokenSerializer()
        {
            cryptographer = new TripleDesCryptographer(File.ReadAllLines(Helpers.GetServerPath("settings\\tokenTripleDESKey.txt")));
        }

        public string Serialize(string token)
        {
            return Convert.ToBase64String(cryptographer.Encrypt(Encoding.UTF8.GetBytes(token)));
        }

        public string Deserialize(string data)
        {
            return Encoding.UTF8.GetString(cryptographer.Decrypt(Convert.FromBase64String(data)));
        }
    }
}