using System;
using System.Collections.Generic;
using System.Text;

namespace GEBUtils.FHandlers
{
    class PRESInfo
    {
        public static byte[] header = new byte[0x0C] { 0x50, 0x72, 0x65, 0x73, 0x10, 0x00, 0x00, 0x00, 0x07, 0x00, 0x00, 0x00 };

    }

    class PRESUtils
    {
        public static int ConvertNumber(byte[] rawdata)
        {
            int num = 0;

            for (int i = 0; i < rawdata.Length; i++)
            {
                num += rawdata[i] * (int)(Math.Pow(256, i));
            }

            return num;
        }
    }
}
