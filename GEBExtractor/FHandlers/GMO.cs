using System;
using System.Collections.Generic;
using System.Text;

namespace GEBUtils.FHandlers
{
    class GMOInfo
    {
        public static byte[] header = new byte[16] { 0x4F, 0x4D, 0x47, 0x2E, 0x30, 0x30, 0x2E, 0x31, 0x50, 0x53, 0x50, 0x00, 0x00, 0x00, 0x00, 0x00 };
        public static long dimensionOffset = 0x14;
        public static long dimensionDelta = 0x10;
        public static bool littlendian = true;
    }
}
