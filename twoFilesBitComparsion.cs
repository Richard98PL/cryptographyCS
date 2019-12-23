using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ComparsionOfBitsInFiles
{
    //RYSZARD ROGALSKI BSI 3
    class Program
    {
        private static readonly String INPUT_FILENAME_MD5_1 = "hash_inwokacja_md5_1.bin";
        private static readonly String OUTPUT_FILENAME_MD5_1 = "hash_inwokacja_md5_1_bits.txt";

        private static readonly String INPUT_FILENAME_MD5_2 = "hash_inwokacja_md5_2.bin";
        private static readonly String OUTPUT_FILENAME_MD5_2 = "hash_inwokacja_md5_2_bits.txt";

        private static readonly String INPUT_FILENAME_SHA256_1 = "hash_inwokacja_sha256_1.bin";
        private static readonly String OUTPUT_FILENAME_SHA256_1 = "hash_inwokacja_sha256_1_bits.txt";

        private static readonly String INPUT_FILENAME_SHA256_2 = "hash_inwokacja_sha256_2.bin";
        private static readonly String OUTPUT_FILENAME_SHA256_2 = "hash_inwokacja_sha256_2_bits.txt";

        private static String[] md5files = new String[2] { OUTPUT_FILENAME_MD5_1, OUTPUT_FILENAME_MD5_2 };
        private static String[] sha256files = new String[2] { OUTPUT_FILENAME_SHA256_1, OUTPUT_FILENAME_SHA256_2 };

        private static readonly int MD5_HASH_LENGTH = 128;
        private static readonly int SHA256_HASH_LENGTH = 128;

        static void Main(string[] args)
        {

            createOutputFilesInBinaryFormatText();
            putBitsInArrays();
            Dictionary<String,int> results = compareBitsInFiles();

            Console.WriteLine("Funkcja skrótu generowana algorytmem MD5 po zmianie 1 BITU w pliku daje w rezultacie tyle różnych BITÓW: " + results["md5Differences"]);
            Console.WriteLine("Funkcja skrótu generowana algorytmem SHA256 po zmianie 1 BITU w pliku daje w rezultacie tyle różnych BITÓW: " + results["sha256Differences"]);
            Console.Read();

        }
        static void createOutputFilesInBinaryFormatText()
        {
            String[] files = new String[8] { INPUT_FILENAME_MD5_1, OUTPUT_FILENAME_MD5_1,
                                             INPUT_FILENAME_MD5_2, OUTPUT_FILENAME_MD5_2,
                                             INPUT_FILENAME_SHA256_1, OUTPUT_FILENAME_SHA256_1,
                                             INPUT_FILENAME_SHA256_2, OUTPUT_FILENAME_SHA256_2 };

            for (int i = 0; i < files.Length - 1; i += 2)
            {
                byte[] fileBytes = File.ReadAllBytes(files[i]);
                StringBuilder sb = new StringBuilder();

                foreach (byte b in fileBytes)
                {
                    sb.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
                }

                File.WriteAllText(files[i + 1], sb.ToString());
            }

        }

        static void putBitsInArrays()
        {
            for (int i = 0; i < md5files.Length; i++)
            {
                StreamReader sr = new StreamReader(md5files[i]);
                md5files[i] = sr.ReadToEnd();
            }

            for (int i = 0; i < sha256files.Length; i++)
            {
                StreamReader sr = new StreamReader(sha256files[i]);
                sha256files[i] = sr.ReadToEnd();
            }
        }

        static Dictionary<String, int> compareBitsInFiles()
        {
            Dictionary<String, int> resultsDictionary = new Dictionary<String,int>();

            int md5Differences = 0;
            int sha256Differences = 0;

            for (int i = 0; i < MD5_HASH_LENGTH; i++)
            {

                if (md5files[0][i] != md5files[1][i])
                {
                    md5Differences++;
                }
            }

            for (int i = 0; i < SHA256_HASH_LENGTH; i++)
            {
                if (sha256files[0][i] != sha256files[1][i])
                {
                    sha256Differences++;
                }
            }

            resultsDictionary.Add("md5Differences",md5Differences);
            resultsDictionary.Add("sha256Differences", sha256Differences);

            return resultsDictionary;
        }
    }
}
