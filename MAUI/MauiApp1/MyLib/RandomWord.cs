using System;
using System.Collections.Generic;
using System.Linq;
namespace Lib
{
    public class RandomWord
    {
        public static string GetWord(int sizeWord)
        {
            var random = new Random();

            string letters = "2345689qwertyupasdfghkzxcvbnm";

            int sizeLetters = letters.Length;

            string resultWord = "";

            for (int i = 0; i < sizeWord; i++)
            {
                resultWord += letters[random.Next(0, sizeLetters - 1)];
            }

            return resultWord;
        }
    }
}
