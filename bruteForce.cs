using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BruteForce
{

    public static class Program
    {

        private static List<Char> ascii_printable_characters = new List<Char>();
        private static int ile_prob_one_way = 0;
        private static int ile_prob_collision = 0;

        private static readonly String INITIAL_PASSWORD = "abcd";
        private static readonly String INITIAL_MD5_HASH_FIRST_3_BYTES_ONEWAY_TESTING = "E2FC71";
        
        private static readonly int ASCII_PRINTABLE_CHARACTERS_LENGTH = 95;
        

        private static String INTIAL_MD5_HASH_FIRST_3_BYTES_COLLISION_TESTING = "A1B2C3";
        private static String first_text_with_hash_A1B2C3 = "";
        private static String second_text_with_hash_A1B2C3 = "";

        private static Random rng = new Random();


        static void Main(string[] args)
        {
            
            initialize_ascii_printable_characters();
            ascii_printable_characters.Shuffle(); // wymieszanie tablicy ascii
            initialize_md5_hash_first_3_bytes_collision();
            Console.WriteLine("Nasz wygenerowany hash do sprawdzania kolizji " + INTIAL_MD5_HASH_FIRST_3_BYTES_COLLISION_TESTING);

            //ascii_printable_characters.ForEach(Print);
            //function above display ascii characters form 32 to 126 -> testing if list of ascii_printable_characters is well filled

            //Console.WriteLine(CalculateMD5Hash("abcd").Substring(0,6));
            //function above tests intial_md5_hash_first_3_bytes;

            var watch1 = System.Diagnostics.Stopwatch.StartNew();
            bruteForceOneWay();
            watch1.Stop();

            Console.WriteLine("Aby dostać zgodność - JEDYNIE PIERWSZYCH 3 BAJTÓW, gdzie MD5 generuje ich aż 16!! musieliśmy czekać, aż");
            Console.WriteLine(watch1.ElapsedMilliseconds + " ms");
            Console.WriteLine("Potrzebowalismy do tego az " + ile_prob_one_way + " prób.");

            var watch2 = System.Diagnostics.Stopwatch.StartNew();
            bruteForceCollision();
            watch2.Stop();

            Console.WriteLine("Tym razem wygenerowaliśmy hash o wartości " + INTIAL_MD5_HASH_FIRST_3_BYTES_COLLISION_TESTING);
            Console.WriteLine("Dla danego HASH'a rowniez tylko pierwszych 3 bajtów, aby otrzymać kolizje tekstów musielismy czekać, aż");
            Console.WriteLine(watch2.ElapsedMilliseconds + " ms");
            Console.WriteLine("Potrzebowalismy do tego az " + ile_prob_collision + " prób.");
            Console.WriteLine("Teksty, które nam znalazło, nie są sobie równe");
            Console.WriteLine("Tekst 1 = " + first_text_with_hash_A1B2C3);
            Console.WriteLine("Tekst 2 = " + second_text_with_hash_A1B2C3);


            Console.Read();
        }

        private static void initialize_ascii_printable_characters()
        {
            for (int i = 32; i <= 126; i++)
            {
                char c = Convert.ToChar(i);
                if (!char.IsControl(c))
                {
                    ascii_printable_characters.Add(c);
                }
            }
        }

        private static void Print(Char c)
        {
            Console.WriteLine(c);
        }

        private static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private static void bruteForceCollision()
        {
            String bruteString = "";
            String bruteHash_First_3_Bytes = "";

            for(int i=0; i < ASCII_PRINTABLE_CHARACTERS_LENGTH; i++)
            {
                for(int j = 0; j < ASCII_PRINTABLE_CHARACTERS_LENGTH; j++)
                {
                    for(int k = 0; k < ASCII_PRINTABLE_CHARACTERS_LENGTH; k++)
                    {
                        for (int l = 0; l < ASCII_PRINTABLE_CHARACTERS_LENGTH; l++)
                        {
                            bruteString = ascii_printable_characters[i].ToString() +
                                          ascii_printable_characters[j].ToString() +
                                          ascii_printable_characters[k].ToString() +
                                          ascii_printable_characters[l].ToString();
                            
                                bruteHash_First_3_Bytes = CalculateMD5Hash(bruteString).Substring(0, 6);
                                ile_prob_collision++;

                                if (first_text_with_hash_A1B2C3 == "" && bruteHash_First_3_Bytes.Equals(INTIAL_MD5_HASH_FIRST_3_BYTES_COLLISION_TESTING))
                                {
                                    first_text_with_hash_A1B2C3 = bruteString;
                                }
                                else if(second_text_with_hash_A1B2C3 == "" && bruteHash_First_3_Bytes.Equals(INTIAL_MD5_HASH_FIRST_3_BYTES_COLLISION_TESTING))
                                {
                                    second_text_with_hash_A1B2C3 = bruteString;
                                    return;
                                }
                            

                        }
                    }
                }
            }

        }

        private static void bruteForceOneWay()
        {
            String bruteString = "";
            String bruteHash_First_3_Bytes = "";

            for (int i = 0; i < ASCII_PRINTABLE_CHARACTERS_LENGTH; i++)
            {
                for (int j = 0; j < ASCII_PRINTABLE_CHARACTERS_LENGTH; j++)
                {
                    for (int k = 0; k < ASCII_PRINTABLE_CHARACTERS_LENGTH; k++)
                    {
                        for (int l = 0; l < ASCII_PRINTABLE_CHARACTERS_LENGTH; l++)
                        {
                            bruteString = ascii_printable_characters[i].ToString() +
                                          ascii_printable_characters[j].ToString() +
                                          ascii_printable_characters[k].ToString() +
                                          ascii_printable_characters[l].ToString();

                            ile_prob_one_way++;

                            if (!bruteString.Equals(INITIAL_PASSWORD))
                            {
                                bruteHash_First_3_Bytes = CalculateMD5Hash(bruteString).Substring(0, 6);

                                if (bruteHash_First_3_Bytes.Equals(INITIAL_MD5_HASH_FIRST_3_BYTES_ONEWAY_TESTING))
                                {
                                    return;
                                }
                            }

                        }
                    }
                }
            }
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void initialize_md5_hash_first_3_bytes_collision()
        {

            String toIntialize = "";
            for(int i = 0; i < 6; i++)
            {
                toIntialize += getRandomHexLetter();
            }
            INTIAL_MD5_HASH_FIRST_3_BYTES_COLLISION_TESTING = toIntialize;
        }

        public static String getRandomHexLetter()
        {
            String[] hex = new String[16] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };
            hex.Shuffle();
            return hex[rng.Next(0, 16)];
        }



    }
}
