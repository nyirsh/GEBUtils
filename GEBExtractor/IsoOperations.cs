using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DiscUtils.Iso9660;

namespace GEBUtils
{
    class IsoOperations
    {
        // Extracts the package.rdp from a GEB iso
        public static bool Extract(string isoPath, bool debug)
        {
            try
            {
                // Debug mode only variables
                long bytesReadTotal = 0;
                string percentageString = "# Extracting package.rdp - 0%";

                // Initializing virtual ISO disc
                if (debug)
                    Console.WriteLine("# Reading ISO File System...");
                CDReader isoFile = new CDReader(new FileStream(isoPath, FileMode.Open), false);

                if (debug)
                    Console.WriteLine("# Checking ISO Files...");
                // Check if file exists
                if (isoFile.FileExists("\\PSP_GAME\\USRDIR\\package.rdp"))
                {
                    if (debug)
                        Console.WriteLine("# Initializing Streams and Buffers");
                    // Initializing Streams and buffers
                    Stream packageStream = isoFile.OpenFile("\\PSP_GAME\\USRDIR\\package.rdp", FileMode.Open);
                    FileStream outStream = new FileStream("package.rdp", FileMode.Create, FileAccess.Write);
                    int bufferLenght = 2048;
                    byte[] buffer = new byte[bufferLenght];

                    if (debug)
                        Console.Write(percentageString);
                    // Extract the package.rdp
                    int bytesRead = packageStream.Read(buffer, 0, bufferLenght);
                    bytesReadTotal += bytesRead;
                    while (bytesRead > 0)
                    {
                        outStream.Write(buffer, 0, bytesRead);
                        outStream.Flush();
                        bytesRead = packageStream.Read(buffer, 0, bufferLenght);
                        bytesReadTotal += bytesRead;
                        if (debug)
                        {
                            int currentPercentage = (int)((bytesReadTotal * 100) / packageStream.Length);
                            if (percentageString != "# Extracting package.rdp - " + currentPercentage + "%")
                            {
                                percentageString = "# Extracting package.rdp - " + currentPercentage + "%";
                                Console.Write("\r" + percentageString);
                            }
                        }
                    }
                    Console.WriteLine("\n# Closing Streams");
                    // Close Streams
                    outStream.Close();
                    packageStream.Close();
                }
                // Iso not valid
                else
                {
                    if (debug)
                    {
                        Console.WriteLine("# Can't find the file \"\\PSP_GAME\\USRDIR\\package.rdp\" into the Iso File System");
                        Console.WriteLine("# Disposing Iso File System");
                    }
                    // Close Virtual Iso
                    isoFile.Dispose();
                    return false;
                }

                if (debug)
                {
                    Console.WriteLine("# Disposing Iso File System");
                }
                // Close Virtual Iso
                isoFile.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="isoPath"></param>
        /// <param name="debug"></param>
        /// <returns>-2 package.rdp dimension, -1 Iso not valid, 1 inserted</returns>
        public static int Insert(string isoPath, bool debug)
        {
            try
            {
                // Debug mode only variables
                long bytesReadTotal = 0;
                string percentageString = "# Inserting package.rdp - 0%";

                // Initializing virtual ISO disc
                if (debug)
                    Console.WriteLine("# Reading ISO File System...");
                FileStream isoStream = new FileStream(isoPath, FileMode.Open, FileAccess.ReadWrite);
                CDReader isoFile = new CDReader(isoStream, false);

                if (debug)
                    Console.WriteLine("# Checking ISO Files...");
                // Check if file exists
                if (isoFile.FileExists("\\PSP_GAME\\USRDIR\\package.rdp"))
                {
                    if (debug)
                        Console.WriteLine("# Initializing Streams and Buffers");
                    // Initializing Streams and buffers
                    Stream packageStream = isoFile.OpenFile("\\PSP_GAME\\USRDIR\\package.rdp", FileMode.Open, FileAccess.Read);
                    FileStream inStream = new FileStream("package.rdp", FileMode.Open, FileAccess.Read);
                    int bufferLenght = 2048;
                    byte[] buffer = new byte[bufferLenght];

                    if (debug)
                        Console.WriteLine("# Checking package.rdp files dimensions");
                    // Checking package.rdp dimensions
                    if (inStream.Length != packageStream.Length)
                    {
                        if (debug)
                        {
                            Console.WriteLine("# The package.rdp has a different dimension from the original one.");
                            Console.WriteLine("# Disposing Iso File System");
                        }
                        // Close Virtual Iso
                        isoFile.Dispose();
                        return -2;
                    }

                    // Closing useless streams
                    packageStream.Close();
                    packageStream.Dispose();
                    isoFile.Dispose();
                    isoFile = null;

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    if (debug)
                        Console.WriteLine("# Initializing injecter...");
                    
                    byte[] rdpUnique = new byte[0x60];
                    inStream.Seek(0, SeekOrigin.Begin);
                    inStream.Read(rdpUnique, 0, 0x60);
                    inStream.Seek(0, SeekOrigin.Begin);

                    long rdpOffset = localizeRDP(rdpUnique, isoStream);
                    isoStream.Seek(rdpOffset, SeekOrigin.Begin);

                    if (debug)
                        Console.Write(percentageString);
                    // Inserting the package.rdp
                    int bytesRead = inStream.Read(buffer, 0, bufferLenght);
                    bytesReadTotal += bytesRead;
                    while (bytesRead > 0)
                    {
                        isoStream.Write(buffer, 0, bytesRead);
                        isoStream.Flush();
                        bytesRead = inStream.Read(buffer, 0, bufferLenght);
                        bytesReadTotal += bytesRead;
                        if (debug)
                        {
                            int currentPercentage = (int)((bytesReadTotal * 100) / packageStream.Length);
                            if (percentageString != "# Inserting package.rdp - " + currentPercentage + "%")
                            {
                                percentageString = "# Inserting package.rdp - " + currentPercentage + "%";
                                Console.Write("\r" + percentageString);
                            }
                        }
                    }
                    Console.WriteLine("\n# Closing Streams");
                    // Close Streams
                    inStream.Close();
                    isoStream.Close();
                }
                // Iso not valid
                else
                {
                    if (debug)
                    {
                        Console.WriteLine("# Can't find the file \"\\PSP_GAME\\USRDIR\\package.rdp\" into the Iso File System");
                        Console.WriteLine("# Disposing Iso File System");
                    }
                    // Close Virtual Iso
                    isoFile.Dispose();
                    return -1;
                }
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        // Returns the offset of the package.rdp
        private static long localizeRDP(byte[] rdpHeader, FileStream isoStream)
        {
            try
            {
                return IndexOfSequence(isoStream, rdpHeader, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static long IndexOfSequence(FileStream fs, byte[] pattern, int startIndex)
        {
            long readedtot = 0;
            List<long> allToCheck = new List<long>();

            int buffLenght = 0x100000;
            byte[] buffer = new byte[buffLenght];

            int readed = fs.Read(buffer, 0, buffLenght);

            long ToRead = fs.Length;
            while (readed > 0)
            {
                long current = Array.IndexOf<byte>(buffer, pattern[0], startIndex);
                while (current >= 0 && current <= buffer.Length - pattern.Length)
                {
                    if ((buffer[current + 1] == pattern[1]) && (buffer[current + 2] == pattern[2]))
                        allToCheck.Add(current);
                    current = Array.IndexOf<byte>(buffer, pattern[0], (int)current + 1);
                }

                foreach (long toCheck in allToCheck)
                {
                    byte[] segment = new byte[pattern.Length];
                    Buffer.BlockCopy(buffer, (int)toCheck, segment, 0, pattern.Length);
                    if (Linq.SequenceEqual(segment, pattern))
                    {
                        return toCheck + readedtot;
                    }
                }

                readedtot += (long)readed;
                readed = fs.Read(buffer, 0, buffLenght);
            }

            return -1;
        }
    }
}
