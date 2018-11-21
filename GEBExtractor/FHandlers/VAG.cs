using System;
using System.Collections.Generic;
using System.Text;

namespace GEBUtils.FHandlers
{
    class VAGInfo
    {
        public static byte[] header = new byte[8] { 0x56, 0x41, 0x47, 0x70, 0x00, 0x00, 0x00, 0x06 };
        public static long dimensionOffset = 0x0C;
        public static long dimensionDelta = 0x40;
        public static bool littlendian = false;
    }
}
