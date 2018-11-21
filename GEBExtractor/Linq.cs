using System;
using System.Collections.Generic;
using System.Text;

namespace GEBUtils
{
    class Linq
    {
        public static bool SequenceEqual(byte[] source, byte[] pattern)
        {
            if (source.Length != pattern.Length)
                return false;
            else
            {
                for (int i = 0; i < source.Length; i++)
                {
                    if (source[i] != pattern[i])
                        return false;
                }
            }

            return true;
        }
    }
}
