using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Pract_3
{
    internal class RandomWord
    {
        public static string createRandomWord(int size)
        {
            string resultString = "";
            
            string chars = "QWERTYUIPASDFGHJKLZXCVBNM123456789";

            var random = new Random();

            for (int i = 0; i < size; i++)
            {
                int randomNumber = random.Next(chars.Length);

                resultString += chars[randomNumber];
            }

            return resultString;
        }
    }
}
