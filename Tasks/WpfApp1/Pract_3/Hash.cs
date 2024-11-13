using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Pract_3
{
    internal class Hash
    {
        public static string createHash(string stringInput)
        {
            SHA256 hash = SHA256.Create();

            return BitConverter.ToString(hash.ComputeHash(Encoding.UTF8.GetBytes(stringInput)));
        }
    }
}
