using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chiến_Binh_Rồng.Sources.Application
{
    public class EncryptionMss
    {
        private unsafe static void FredouImplementation(string input, int inputLength, string replace, string[] replaceBy)
        {
            var indexes = new List<int>();

            int len = inputLength;

            fixed (char* i = input, r = replace)
            {
                int replaceValAsInt = *((int*)r);

                while (--len > -1)
                {
                    if (replaceValAsInt == *((int*)&i[len]))
                    {
                        indexes.Add(len--);
                    }
                }
            }

            var idx = indexes.ToArray();
            len = indexes.Count;

            Parallel.For(0, replaceBy.Length, l =>
                Process(input, inputLength, replaceBy[l], idx, len)
            );
        }

        private unsafe static void Process(string input, int len, string replaceBy, int[] idx, int idxLen)
        {
            var output = new char[len];

            fixed (char* o = output, i = input, r = replaceBy)
            {
                int replaceByValAsInt = *((int*)r);

                while (--len > -1)
                {
                    o[len] = i[len];
                }

                while (--idxLen > -1)
                {
                    ((int*)&o[idx[idxLen]])[0] = replaceByValAsInt;
                }
            }

            //Console.WriteLine(output);
        }
    }
}
