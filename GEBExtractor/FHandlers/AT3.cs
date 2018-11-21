using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GEBUtils.FHandlers
{
    class AT3Info
    {
        public static byte[] header = new byte[4] { 0x52, 0x49, 0x46, 0x46 };
        public static long dimensionOffset = 0x04;
        public static long dimensionDelta = 0x08;
        public static bool littlendian = true;
    }
}
