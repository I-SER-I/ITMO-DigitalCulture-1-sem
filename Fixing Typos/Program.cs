using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Fixing_Typos
{
    class Program
    {
        public static int LevenshteinDistance(string firstword, string secondword)
        {
            if (firstword == null) throw new ArgumentNullException("first");
            if (firstword == null) throw new ArgumentNullException("second");
            int diff;
            int[,] m = new int[firstword.Length + 1, secondword.Length + 1];
            for (int i = 0; i <= firstword.Length; i++)
            {
                m[i, 0] = i;
            }
            for (int j = 0; j <= secondword.Length; j++)
            {
                m[0, j] = j;
            }
            for (int i = 1; i <= firstword.Length; i++)
            {
                for (int j = 1; j <= secondword.Length; j++)
                {
                    diff = (firstword[i - 1] == secondword[j - 1]) ? 0 : 1;
                    m[i, j] = Math.Min(Math.Min(m[i - 1, j] + 1, m[i, j - 1] + 1), m[i - 1, j - 1] + diff);
                }
            }
            return m[firstword.Length, secondword.Length];
        }
        static void Main()
        {
            StreamWriter sw = new StreamWriter("output.txt");
            string[] globaltext = File.ReadAllLines("dict1.txt", Encoding.GetEncoding(1251)).Select(k => k.Trim().Split(' ').First()).ToArray();
            string[] gg = File.ReadAllLines("dict1.txt", Encoding.GetEncoding(1251)).Select(k => k.Trim().Split(' ').ElementAtOrDefault(1)).ToArray();
            string brain446 = File.ReadAllText("brain446.txt", Encoding.GetEncoding(1251));

            //First task
            brain446 = Regex.Replace(brain446, @"[!?,;.:«»()]", " ");
            brain446 = Regex.Replace(brain446, @"\s+", " ");
            string[] localtext = brain446.Split(' ');
            Array.Resize(ref localtext, localtext.Length - 1);
            for (int i = 0; i < localtext.Length; i++)
            {
                localtext[i] = localtext[i].ToLower();
                sw.WriteLine(localtext[i]);
            }

            //Second task
            Console.WriteLine($"Количество словоформ: {localtext.Length}");
            Console.WriteLine();
            var buff = localtext.Distinct();
            string[] differentwords = buff.ToArray();
            for (int i = 0; i < differentwords.Length; i++)
            {
                sw.WriteLine(differentwords[i]);
            }
            Console.WriteLine($"Количество разных словоформ: {differentwords.Length}");
            Console.WriteLine();
            buff = differentwords.Except(globaltext);
            string[] wrongwords = buff.ToArray();
            Console.WriteLine($"Количество разных словоформ, которые присутствуют в словаре: {differentwords.Length - wrongwords.Length}");
            Console.WriteLine();

            //Third task
            Console.WriteLine($"Количество словоформ, которые не присутствуют в словаре (потенциальные ошибки): {wrongwords.Length}");
            for (int i = 0; i < wrongwords.Length; i++)
            {
                Console.WriteLine(wrongwords[i]);
            }
            Console.WriteLine();

            for (int i = 0; i < wrongwords.Length; i++)
            {
                int MinLevDist = 100;
                int buf_i = 0;
                int buf_j = 0;
                for (int j = 0; j < globaltext.Length; j++)
                {
                    if (MinLevDist >= LevenshteinDistance(wrongwords[i], globaltext[j]))
                    {
                        MinLevDist = LevenshteinDistance(wrongwords[i], globaltext[j]);
                        buf_i = i;
                        buf_j = j;
                    }
                }
                Console.WriteLine($"{wrongwords[buf_i]} {globaltext[buf_j]} {LevenshteinDistance(wrongwords[buf_i], globaltext[buf_j])}");
            }
            Console.ReadKey();
            sw.Dispose();
        }
    }
}