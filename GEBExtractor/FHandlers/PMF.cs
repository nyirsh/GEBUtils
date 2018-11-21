using System;
using System.Collections.Generic;
using System.Text;

namespace GEBUtils.FHandlers
{
    class PMFInfo
    {
        public static byte[] header = new byte[12] { 0x50, 0x53, 0x4D, 0x46, 0x30, 0x30, 0x31, 0x35, 0x00, 0x00, 0x08, 0x00 };
        public static long dimensionOffset = 12;
        public static long dimensionDelta = 0x800;
        public static bool littlendian = false;
    }
}
