using System;
using System.Collections.Generic;
using System.Text;
using GEBUtils.FHandlers;
using System.IO;
using System.Globalization;

namespace GEBUtils
{
    class EDATOperations
    {
        public static void GenerateIndex(bool debug)
        {
            try
            {
                if (debug)
                    Console.WriteLine("# Initializing dlc buffer");
                if (debug)
                {
                    Console.Write("# Indexing all files - 0%");
                }
                FileStream packagestream = new FileStream("dlc.edat", FileMode.Open, FileAccess.Read);

                List<byte[]> patterns = new List<byte[]>();

                patterns.Add(PMFInfo.header);
                patterns.Add(GIMInfo.header);
                patterns.Add(AT3Info.header);
                patterns.Add(GMOInfo.header);
                patterns.Add(VAGInfo.header);
                patterns.Add(TR2Info.header);
                patterns.Add(PRESInfo.header);

                List<Dictionary<long, byte>> specials = new List<Dictionary<long, byte>>();

                specials.Add(new Dictionary<long, byte>()); // PMF
                specials.Add(new Dictionary<long, byte>()); // GIM
                specials.Add(new Dictionary<long, byte>()); // AT3
                specials[2].Add(0x08, 0x57);
                specials[2].Add(0x09, 0x41);
                specials[2].Add(0x0A, 0x56);
                specials[2].Add(0x0B, 0x45);

                specials.Add(new Dictionary<long, byte>()); // GMO
                specials.Add(new Dictionary<long, byte>()); // VAG
                specials.Add(new Dictionary<long, byte>()); // TR2
                specials.Add(new Dictionary<long, byte>()); // PRES

                List<List<long>> allPositions = MultiIndexOfSequenceSpecial(packagestream, patterns, 0, specials, debug);
                List<List<long>> allDimensions = new List<List<long>>();

                if (debug)
                    Console.WriteLine("\n# Analyzing all files headers");
                for (int i = 0; i < allPositions.Count - 1; i++)
                {
                    allDimensions.Add((List<long>)AnalyzeHeader(packagestream, allPositions[i], i));
                }

                // Need special type for tr2
                Dictionary<string, long> tr2fileinfo = TR2HeadersAnalyzer(packagestream, allPositions[5]);

                // Need special type for pres
                Dictionary<string, long> presfileinfo = PRESHeadersAnalyzer(packagestream, allPositions[6]);

                
               if (debug)
                    Console.WriteLine("\n# Saving indexes file...");
                if (File.Exists("dlc-index.lst"))
                    File.Delete("dlc-index.lst");
                if (debug)
                    Console.WriteLine("\n Files Statistics");


                string ext = "";
                for (int i = 0; i < allPositions.Count; i++)
                {
                    int count = 0;
                    switch (i)
                    {
                        case 0:
                            ext = "pmf";
                            if (debug)
                                Console.Write("PMF: ");
                            break;
                        case 1:
                            ext = "gim";
                            if (debug)
                                Console.Write("GIM: ");
                            break;
                        case 2:
                            ext = "at3";
                            if (debug)
                                Console.Write("AT3: ");
                            break;
                        case 3:
                            ext = "gmo";
                            if (debug)
                                Console.Write("GMO: ");
                            break;
                        case 4:
                            ext = "vag";
                            if (debug)
                                Console.Write("VAG: ");
                            break;
                        case 5:
                            ext = "tr2";
                            if (debug)
                                Console.Write("TR2: ");
                            break;
                        case 6:
                            ext = "pres";
                            if (debug)
                                Console.Write("PRES: ");
                            break;
                    }

                    if (i < 5)
                    {
                        foreach (long position in allPositions[i])
                        {
                            string prefixName = "";
                            if (count < 10)
                                prefixName = "0000";
                            else if (count < 100)
                                prefixName = "000";
                            else if (count < 1000)
                                prefixName = "00";
                            else if (count < 10000)
                                prefixName = "0";

                            File.AppendAllText("dlc-index.lst", position.ToString("X") + "|" + allDimensions[i][count].ToString("X") + "|0|" + prefixName + count.ToString() + "." + ext + "\r\n");
                            count++;
                        }
                    }
                    else
                    {

                        if (i == 5)
                        {
                            foreach (KeyValuePair<string, long> kvp in tr2fileinfo)
                            {
                                File.AppendAllText("dlc-index.lst", allPositions[i][count].ToString("X") + "|" + kvp.Value.ToString("X") + "|0|" + kvp.Key + "." + ext + "\r\n");
                                count++;
                            }
                        }
                        else if (i == 6)
                        {
                            foreach (KeyValuePair<string, long> kvp in presfileinfo)
                            {
                                File.AppendAllText("dlc-index.lst", allPositions[i][count].ToString("X") + "|" + kvp.Value.ToString("X") + "|0|" + kvp.Key + "." + ext + "\r\n");
                                count++;
                            }
                        }
                    }
                    if (debug)
                        Console.WriteLine(count);
                }

                if (debug)
                    Console.WriteLine("");

                packagestream.Close();
                packagestream.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        // Restituisce le stringhe di un singolo index
        public static string[] SubIndexType(string ext)
        {
            List<string> subindex = new List<string>();

            string[] allindexes = File.ReadAllLines("dlc-index.lst");

            foreach (string index in allindexes)
            {
                if (index.EndsWith(ext))
                    subindex.Add(index);
            }

            return subindex.ToArray();
        }

        public static string SubIndexSingle(string file)
        {
            string[] allindexes = File.ReadAllLines("dlc-index.lst");

            foreach (string index in allindexes)
            {
                if (index.EndsWith(file))
                    return index;
            }

            return "";
        }





        // Estrae i file dagli index
        public static void Unpack(string args, bool debug)
        {
            if (debug)
                Console.WriteLine("# Initializing Extractor...");
            FileStream packagestream = new FileStream("dlc.edat", FileMode.Open, FileAccess.Read);

            if ((args == "all") || (args == "") || (args == null))
            {
                for (int i = 0; i < 7; i++)
                    UnpackSpecifiedTypes(packagestream, i, debug);
            }
            else
            {
                string[] splitted = args.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string type in splitted)
                {
                    switch (type)
                    {
                        case "pmf":
                            UnpackSpecifiedTypes(packagestream, 0, debug);
                            break;
                        case "gim":
                            UnpackSpecifiedTypes(packagestream, 1, debug);
                            break;
                        case "at3":
                            UnpackSpecifiedTypes(packagestream, 2, debug);
                            break;
                        case "gmo":
                            UnpackSpecifiedTypes(packagestream, 3, debug);
                            break;
                        case "vag":
                            UnpackSpecifiedTypes(packagestream, 4, debug);
                            break;
                        case "tr2":
                            UnpackSpecifiedTypes(packagestream, 5, debug);
                            break;
                        case "pres":
                            UnpackSpecifiedTypes(packagestream, 6, debug);
                            break;
                    }
                }
            }

            if (debug)
                Console.WriteLine("# Closing Streams");
            packagestream.Close();
            packagestream.Dispose();
        }

        public static void SingleUnpack(string args, bool debug)
        {
            if (debug)
                Console.WriteLine("# Initializing Extractor...");
            if (!Directory.Exists("dlc"))
                Directory.CreateDirectory("dlc");

            FileStream packagestream = new FileStream("dlc.edat", FileMode.Open, FileAccess.Read);

            string[] allfiles = args.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string file in allfiles)
            {
                string rawdata = SubIndexSingle(file);
                if (rawdata != "")
                {
                    string fileDir = Path.Combine("dlc", Path.GetExtension(file).Replace(".", ""));

                    if (!Directory.Exists(fileDir))
                        Directory.CreateDirectory(fileDir);

                    string[] rawFileData = rawdata.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    long fileOffset = long.Parse(rawFileData[0], NumberStyles.AllowHexSpecifier);
                    long fileDimension = long.Parse(rawFileData[1], NumberStyles.AllowHexSpecifier);
                    string fileDestination = Path.Combine(fileDir, rawFileData[3]);

                    Console.WriteLine("# Extracting " + rawFileData[3]);
                    UnpackSpecifiedFile(packagestream, fileOffset, fileDimension, fileDestination);
                }
                else
                {
                    if (debug)
                        Console.WriteLine(" - Could not extract " + file + " - File not found");
                }
            }

            if (debug)
                Console.WriteLine("# Closing Streams");
            packagestream.Close();
            packagestream.Dispose();
        }

        public static void UnpackSpecifiedTypes(FileStream packagestream, int filetype, bool debug)
        {
            if (!Directory.Exists("dlc"))
                Directory.CreateDirectory("dlc");

            string ext = "";
            string fileDir = "";
            string basepercentString = "";
            string percentString = "";
            switch (filetype)
            {
                case 0:
                    ext = "pmf";
                    break;
                case 1:
                    ext = "gim";
                    break;
                case 2:
                    ext = "at3";
                    break;
                case 3:
                    ext = "gmo";
                    break;
                case 4:
                    ext = "vag";
                    break;
                case 5:
                    ext = "tr2";
                    break;
                case 6:
                    ext = "pres";
                    break;
            }

            fileDir = Path.Combine("dlc", ext);
            basepercentString = "# Extracting " + ext + " files - ";

            if (!Directory.Exists(fileDir))
                Directory.CreateDirectory(fileDir);

            string[] rawAllFiles = SubIndexType(ext);
            int count = 0;
            foreach (string rawFile in rawAllFiles)
            {
                string[] rawFileData = rawFile.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                long fileOffset = long.Parse(rawFileData[0], NumberStyles.AllowHexSpecifier);
                long fileDimension = long.Parse(rawFileData[1], NumberStyles.AllowHexSpecifier);
                string fileDestination = Path.Combine(fileDir, rawFileData[3]);

                UnpackSpecifiedFile(packagestream, fileOffset, fileDimension, fileDestination);

                count++;

                if (debug)
                {
                    int percent = ((count * 100) / rawAllFiles.Length);
                    if (percentString != (basepercentString + percent + "%"))
                    {
                        percentString = basepercentString + percent + "%";
                        Console.Write("\r" + percentString);
                    }
                }
            }

            if (debug)
                Console.WriteLine("");
        }

        public static void UnpackSpecifiedFile(FileStream packagestream, long fileOffset, long fileDimension, string fileDestination)
        {
            FileStream outStream = new FileStream(fileDestination, FileMode.Create, FileAccess.Write);

            long readedTot = 0;
            int readed = 0;
            byte[] buffer = new byte[0x1000000];
            packagestream.Seek(fileOffset, SeekOrigin.Begin);

            while (readedTot < fileDimension)
            {
                if ((readedTot + 0x1000000) > fileDimension)
                {
                    readed = packagestream.Read(buffer, 0, (int)(fileDimension - readedTot));
                }
                else
                {
                    readed = packagestream.Read(buffer, 0, 0x1000000);
                }
                readedTot += readed;
                outStream.Write(buffer, 0, readed);
                outStream.Flush();
            }
            outStream.Close();
            outStream.Dispose();
        }





        // Estrae i file dagli index
        public static void Pack(string args, bool debug)
        {
            if (debug)
                Console.WriteLine("# Initializing Injecter...");
            FileStream packagestream = new FileStream("dlc.edat", FileMode.Open, FileAccess.Write);

            if ((args == "all") || (args == "") || (args == null))
            {
                for (int i = 0; i < 7; i++)
                    UnpackSpecifiedTypes(packagestream, i, debug);
            }
            else
            {
                string[] splitted = args.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string type in splitted)
                {
                    switch (type)
                    {
                        case "pmf":
                            PackSpecifiedTypes(packagestream, 0, debug);
                            break;
                        case "gim":
                            PackSpecifiedTypes(packagestream, 1, debug);
                            break;
                        case "at3":
                            PackSpecifiedTypes(packagestream, 2, debug);
                            break;
                        case "gmo":
                            PackSpecifiedTypes(packagestream, 3, debug);
                            break;
                        case "vag":
                            PackSpecifiedTypes(packagestream, 4, debug);
                            break;
                        case "tr2":
                            PackSpecifiedTypes(packagestream, 5, debug);
                            break;
                        case "pres":
                            PackSpecifiedTypes(packagestream, 6, debug);
                            break;
                    }
                }
            }

            if (debug)
                Console.WriteLine("# Closing Streams");
            packagestream.Close();
            packagestream.Dispose();
        }

        public static void SinglePack(string args, bool debug)
        {
            if (debug)
                Console.WriteLine("# Initializing Injecter...");
            FileStream packagestream = new FileStream("dlc.edat", FileMode.Open, FileAccess.Write);

            string[] allfiles = args.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string file in allfiles)
            {
                string rawdata = SubIndexSingle(file);
                if (rawdata != "")
                {
                    string fileDir = Path.Combine("dlc", Path.GetExtension(file).Replace(".", ""));

                    string[] rawFileData = rawdata.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    long fileOffset = long.Parse(rawFileData[0], NumberStyles.AllowHexSpecifier);
                    long fileDimension = long.Parse(rawFileData[1], NumberStyles.AllowHexSpecifier);
                    long fileExtraSpace = long.Parse(rawFileData[2], NumberStyles.AllowHexSpecifier);
                    string fileOrigin = Path.Combine(fileDir, rawFileData[3]);

                    Console.WriteLine("# Injecting " + rawFileData[3]);
                    PackSpecifiedFile(packagestream, fileOrigin, fileOffset, fileDimension, fileExtraSpace, true);
                }
                else
                {
                    if (debug)
                        Console.WriteLine(" - Could not inject " + file + " - File not found");
                }
            }

            if (debug)
                Console.WriteLine("# Closing Streams");
            packagestream.Close();
            packagestream.Dispose();
        }

        public static void PackSpecifiedTypes(FileStream packagestream, int filetype, bool debug)
        {
            string ext = "";
            string fileDir = "";
            string basepercentString = "";
            string percentString = "";
            switch (filetype)
            {
                case 0:
                    ext = "pmf";
                    break;
                case 1:
                    ext = "gim";
                    break;
                case 2:
                    ext = "at3";
                    break;
                case 3:
                    ext = "gmo";
                    break;
                case 4:
                    ext = "vag";
                    break;
                case 5:
                    ext = "tr2";
                    break;
                case 6:
                    ext = "pres";
                    break;
            }

            fileDir = Path.Combine("dlc", ext);
            basepercentString = "# Injecting " + ext + " files - ";

            string[] rawAllFiles = SubIndexType(ext);
            int count = 0;
            foreach (string rawFile in rawAllFiles)
            {
                string[] rawFileData = rawFile.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                long fileOffset = long.Parse(rawFileData[0], NumberStyles.AllowHexSpecifier);
                long fileDimension = long.Parse(rawFileData[1], NumberStyles.AllowHexSpecifier);
                long fileExtraSpace = long.Parse(rawFileData[2], NumberStyles.AllowHexSpecifier);
                string fileOrigin = Path.Combine(fileDir, rawFileData[3]);

                PackSpecifiedFile(packagestream, fileOrigin, fileOffset, fileDimension, fileExtraSpace, true);

                count++;

                if (debug)
                {
                    int percent = ((count * 100) / rawAllFiles.Length);
                    if (percentString != (basepercentString + percent + "%"))
                    {
                        percentString = basepercentString + percent + "%";
                        Console.Write("\r" + percentString);
                    }
                }
            }

            if (debug)
                Console.WriteLine("");
        }

        public static void PackSpecifiedFile(FileStream packagestream, string fileOrigin, long fileOffset, long fileDimension, long fileExtraSpace, bool debug)
        {
            FileStream inStream = new FileStream(fileOrigin, FileMode.Open, FileAccess.Read);

            if (inStream.Length <= (fileDimension + fileExtraSpace))
            {
                long readedTot = 0;
                int readed = 0;
                byte[] buffer = new byte[0x1000000];
                packagestream.Seek(fileOffset, SeekOrigin.Begin);

                while (readedTot < inStream.Length)
                {
                    if ((readedTot + 0x1000000) > inStream.Length)
                    {
                        readed = inStream.Read(buffer, 0, (int)(inStream.Length - readedTot));
                    }
                    else
                    {
                        readed = inStream.Read(buffer, 0, 0x1000000);
                    }
                    readedTot += readed;
                    packagestream.Write(buffer, 0, readed);
                    packagestream.Flush();
                }

                if (inStream.Length > fileDimension)
                {
                    if (debug)
                        Console.WriteLine(" - " + Path.GetFileName(fileOrigin) + ": used " + (inStream.Length - fileDimension) + " of free space (" + (fileDimension + fileExtraSpace - inStream.Length) + " bytes more)");
                }

                if (inStream.Length < fileDimension)
                {
                    byte[] voidbuffer = new byte[(int)(fileDimension - readedTot)];

                    packagestream.Write(voidbuffer, 0, voidbuffer.Length);
                    packagestream.Flush();

                    if (debug)
                        Console.WriteLine(" - " + Path.GetFileName(fileOrigin) + ": " + (fileDimension - inStream.Length) + " bytes free");
                }
            }
            else
            {
                if (debug)
                    Console.WriteLine(" - Skipping " + Path.GetFileName(fileOrigin) + ": file is too big");
            }

            inStream.Close();
            inStream.Dispose();
        }





        // Richiama il metodo corretto per l'analisi degli header
        private static object AnalyzeHeader(FileStream fs, List<long> positions, int filetype)
        {
            try
            {
                switch (filetype)
                {
                    case 0:
                        return HeadersAnalyzer(fs, positions, PMFInfo.dimensionOffset, PMFInfo.dimensionDelta, PMFInfo.littlendian, "PMF");
                    case 1:
                        return HeadersAnalyzer(fs, positions, GIMInfo.dimensionOffset, GIMInfo.dimensionDelta, GIMInfo.littlendian, "GIM");
                    case 2:
                        return HeadersAnalyzer(fs, positions, AT3Info.dimensionOffset, AT3Info.dimensionDelta, AT3Info.littlendian, "AT3");
                    case 3:
                        return HeadersAnalyzer(fs, positions, GMOInfo.dimensionOffset, GMOInfo.dimensionDelta, GMOInfo.littlendian, "GMO");
                    case 4:
                        return HeadersAnalyzer(fs, positions, VAGInfo.dimensionOffset, VAGInfo.dimensionDelta, VAGInfo.littlendian, "VAG");
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Costruisce la lista della dimensione di tutti i files
        private static List<long> HeadersAnalyzer(FileStream fs, List<long> positions, long dimensionOffset, long dimensionDelta, bool littlendian, string fileType)
        {
            List<long> dimenions = new List<long>();

            int done = 0;
            int last = -1;
            foreach (long position in positions)
            {
                byte[] buffer = new byte[4];

                fs.Seek(position + dimensionOffset, SeekOrigin.Begin);
                fs.Read(buffer, 0, 4);
                dimenions.Add(SingleHeaderAnalyzer(buffer, dimensionDelta, littlendian));

                done++;
                if (last != (int)((done * 100) / positions.Count))
                {
                    last = (int)((done * 100) / positions.Count);
                    Console.Write("\r - " + fileType + ": " + last + "%");
                }
            }
            Console.WriteLine("\r - " + fileType + ": 100%");

            return dimenions;
        }

        // Ricava la dimensione di un singolo file
        private static long SingleHeaderAnalyzer(byte[] rawdata, long dimensionDelta, bool littlendian)
        {
            long dimension = dimensionDelta;

            if (littlendian)
            {
                dimension += rawdata[0];
                dimension += rawdata[1] * 256;
                dimension += rawdata[2] * 65536;
                dimension += rawdata[3] * 65536 * 256;
            }
            else
            {
                dimension += rawdata[3];
                dimension += rawdata[2] * 256;
                dimension += rawdata[1] * 65536;
                dimension += rawdata[0] * 65536 * 256;
            }
            return dimension;
        }




        // Trova una sequenza di byte all'interno di un array
        private static long SingleIndexOfSequence(byte[] buffer, byte[] pattern, int startIndex)
        {
            long position = -1;
            List<long> allToCheck = new List<long>();

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
                    return toCheck;
            }

            return position;
        }

        private static List<long> IndexOfSequence(FileStream fs, byte[] pattern, int startIndex, bool debug)
        {
            // Debug only variables
            long readedtot = 0;
            string percentString = "# Indexing all files - 0%";

            List<long> positions = new List<long>();
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
                    //if (segment.SequenceEqual<byte>(pattern))
                    if (Linq.SequenceEqual(segment, pattern))
                    {
                        if (!positions.Contains(toCheck + readedtot))
                            positions.Add(toCheck + readedtot);
                    }
                }

                readedtot += (long)readed;

                if (debug)
                {
                    int percent = (int)((readedtot * 100) / ToRead);
                    if (percentString != "# Indexing all files - " + percent + "%")
                    {
                        percentString = "# Indexing all files - " + percent + "%";
                        Console.Write("\r" + percentString);
                    }
                }

                readed = fs.Read(buffer, 0, buffLenght);
            }

            return positions;
        }

        private static List<List<long>> MultiIndexOfSequence(FileStream fs, List<byte[]> patterns, int startIndex, bool debug)
        {
            // Debug only variables
            long readedtot = 0;
            string percentString = "# Indexing all files - 0%";

            List<List<long>> allpositions = new List<List<long>>();
            List<List<long>> allallToCheck = new List<List<long>>();

            int buffLenght = 0x100000;
            byte[] buffer = new byte[buffLenght];

            int readed = fs.Read(buffer, 0, buffLenght);

            for (int i = 0; i < patterns.Count; i++)
            {
                allpositions.Add(new List<long>());
            }

            long ToRead = fs.Length;
            while (readed > 0)
            {
                allallToCheck.Clear();
                for (int i = 0; i < patterns.Count; i++)
                {
                    allallToCheck.Add(new List<long>());
                    long current = Array.IndexOf<byte>(buffer, patterns[i][0], startIndex);
                    while (current >= 0 && current <= buffer.Length - patterns[i].Length)
                    {
                        if ((buffer[current + 1] == patterns[i][1]) && (buffer[current + 2] == patterns[i][2]))
                            allallToCheck[i].Add(current);
                        current = Array.IndexOf<byte>(buffer, patterns[i][0], (int)current + 1);
                    }
                }

                for (int i = 0; i < patterns.Count; i++)
                {
                    if (allallToCheck[i].Count > 0)
                    {
                        foreach (long toCheck in allallToCheck[i])
                        {
                            byte[] segment = new byte[patterns[i].Length];
                            Buffer.BlockCopy(buffer, (int)toCheck, segment, 0, patterns[i].Length);

                            if (Linq.SequenceEqual(segment, patterns[i]))
                            {
                                if (!allpositions[i].Contains(toCheck + readedtot))
                                    allpositions[i].Add(toCheck + readedtot);
                            }
                        }
                    }
                }

                readedtot += (long)readed;

                if (debug)
                {
                    int percent = (int)((readedtot * 100) / ToRead);
                    if (percentString != "# Indexing all files - " + percent + "%")
                    {
                        percentString = "# Indexing all files - " + percent + "%";
                        Console.Write("\r" + percentString);
                    }
                }
                readed = fs.Read(buffer, 0, buffLenght);
            }

            return allpositions;
        }

        private static List<List<long>> MultiIndexOfSequenceSpecial(FileStream fs, List<byte[]> patterns, int startIndex, List<Dictionary<long, byte>> specials, bool debug)
        {
            // Debug only variables
            long readedtot = 0;
            string percentString = "# Indexing all files - 0%";

            List<List<long>> allpositions = new List<List<long>>();
            List<List<long>> allallToCheck = new List<List<long>>();

            int buffLenght = 0x100000;
            byte[] buffer = new byte[buffLenght];

            int readed = fs.Read(buffer, 0, buffLenght);

            for (int i = 0; i < patterns.Count; i++)
            {
                allpositions.Add(new List<long>());
            }

            long ToRead = fs.Length;
            while (readed > 0)
            {
                allallToCheck.Clear();
                for (int i = 0; i < patterns.Count; i++)
                {
                    allallToCheck.Add(new List<long>());
                    long current = Array.IndexOf<byte>(buffer, patterns[i][0], startIndex);
                    while (current >= 0 && current <= buffer.Length - patterns[i].Length)
                    {
                        if ((buffer[current + 1] == patterns[i][1]) && (buffer[current + 2] == patterns[i][2]))
                            allallToCheck[i].Add(current);
                        current = Array.IndexOf<byte>(buffer, patterns[i][0], (int)current + 1);
                    }
                }

                for (int i = 0; i < patterns.Count; i++)
                {
                    if (allallToCheck[i].Count > 0)
                    {
                        foreach (long toCheck in allallToCheck[i])
                        {
                            byte[] segment = new byte[patterns[i].Length];
                            Buffer.BlockCopy(buffer, (int)toCheck, segment, 0, patterns[i].Length);

                            if (Linq.SequenceEqual(segment, patterns[i]))
                            {
                                bool toAdd = true;
                                if (specials[i].Count > 0)
                                {
                                    foreach (KeyValuePair<long, byte> kvp in specials[i])
                                    {
                                        if (buffer[toCheck + kvp.Key] != kvp.Value)
                                        {
                                            toAdd = false;
                                            break;
                                        }
                                    }
                                }
                                if (toAdd)
                                {
                                    if ((toCheck + readedtot) < fs.Length)
                                        if (!allpositions[i].Contains(toCheck + readedtot))
                                            allpositions[i].Add(toCheck + readedtot);
                                }
                            }
                        }
                    }
                }

                readedtot += (long)readed;

                if (debug)
                {
                    int percent = (int)((readedtot * 100) / ToRead);
                    if (percentString != "# Indexing all files - " + percent + "%")
                    {
                        percentString = "# Indexing all files - " + percent + "%";
                        Console.Write("\r" + percentString);
                    }
                }
                readed = fs.Read(buffer, 0, buffLenght);
            }

            return allpositions;
        }



        // Costruisce la lista della dimensione di tutti i file tr2
        private static Dictionary<string, long> TR2HeadersAnalyzer(FileStream fs, List<long> positions)
        {
            Dictionary<string, long> tr2FileInfo = new Dictionary<string, long>();

            int done = 0;
            int last = -1;
            foreach (long position in positions)
            {
                byte[] buffer = new byte[0x1000000];

                fs.Seek(position + TR2Info.header.Length + 0x02, SeekOrigin.Begin);
                fs.Read(buffer, 0, 0x1000000);
                string fname = (string)SingleTR2HeaderAnalyzer(buffer, 0);
                long fend = (long)SingleTR2HeaderAnalyzer(buffer, 1);
                fend += TR2Info.header.Length + 0x02;

                if (fname == "article")
                {
                    byte[] articleBuffer = new byte[4];
                    Buffer.BlockCopy(buffer, 0x15C - TR2Info.header.Length - 0x02, articleBuffer, 0, 0x04);
                    fend = TR2Utils.ConvertNumber(articleBuffer);
                    Buffer.BlockCopy(buffer, 0x164 - TR2Info.header.Length - 0x02, articleBuffer, 0, 0x04);
                    fend += TR2Utils.ConvertNumber(articleBuffer);
                }
                if (fend > 0)
                {
                    if (tr2FileInfo.ContainsKey(fname))
                    {
                        int x = 1;
                        while (tr2FileInfo.ContainsKey(fname + "_" + x))
                        {
                            x++;
                        }
                        tr2FileInfo.Add(fname + "_" + x, fend);
                    }
                    else
                    {
                        tr2FileInfo.Add(fname, fend);
                    }
                }

                done++;
                if (last != (int)((done * 100) / positions.Count))
                {
                    last = (int)((done * 100) / positions.Count);
                    Console.Write("\r - TR2: " + last + "%");
                }
            }
            Console.WriteLine("");

            return tr2FileInfo;
        }

        private static object SingleTR2HeaderAnalyzer(byte[] rawdata, int mode)
        {
            // Restituisce il nome
            if (mode == 0)
            {
                //byte[] endsequence = new byte[4] { 0x40, 0x00, 0x00, 0x00 };
                //long endposition = SingleIndexOfSequence(rawdata, endsequence, 0);
                byte[] rawtr2name = new byte[0x30];
                Buffer.BlockCopy(rawdata, 0, rawtr2name, 0, 0x30);
                string test = Encoding.UTF8.GetString(rawtr2name);
                return test.Replace("\0", "");
            }
            // Restituisce la dimensione
            else
            {
                byte[] rawtestdata = new byte[9];

                long endposition = SingleIndexOfSequence(rawdata, TR2Info.endingheader, 0);

                if (endposition > 0)
                {
                    Buffer.BlockCopy(rawdata, (int)endposition, rawtestdata, 0, 0x09);

                    if (Linq.SequenceEqual(rawtestdata, TR2Info.specialendingheader))
                        return endposition + TR2Info.specialendingheader.Length;
                    return endposition + TR2Info.endingheader.Length;
                }
                return 0;
            }
        }




        // Costruisce la lista della dimensione di tutti i file Pres
        private static Dictionary<string, long> PRESHeadersAnalyzer(FileStream fs, List<long> positions)
        {
            Dictionary<string, long> presFileInfo = new Dictionary<string, long>();

            int done = 0;
            int last = -1;
            foreach (long position in positions)
            {
                byte[] buffer = new byte[0x10000000];

                fs.Seek(position, SeekOrigin.Begin);
                fs.Read(buffer, 0, 0x10000000);
                string prefixName = "";

                if (done < 10)
                    prefixName = "0000";
                else if (done < 100)
                    prefixName = "000";
                else if (done < 1000)
                    prefixName = "00";
                else if (done < 10000)
                    prefixName = "0";

                string fname = prefixName + done.ToString();
                long fend = SinglePRESHeaderAnalyzer(buffer, position);
                if (fend > 0)
                {
                    presFileInfo.Add(fname, fend);
                }
                done++;
                if (last != (int)((done * 100) / positions.Count))
                {
                    last = (int)((done * 100) / positions.Count);
                    Console.Write("\r - PRES: " + last + "%");
                }
            }
            Console.WriteLine();

            return presFileInfo;
        }

        private static long SinglePRESHeaderAnalyzer(byte[] rawdata, long offset)
        {
            byte[] firstFileOffset = new byte[4];
            Buffer.BlockCopy(rawdata, 0x50, firstFileOffset, 0, 4);
            int lastFileHeaderOffset = PRESUtils.ConvertNumber(firstFileOffset) - 0x20;

            if (lastFileHeaderOffset <= 0)
            {
                firstFileOffset = new byte[4];
                Buffer.BlockCopy(rawdata, 0x70, firstFileOffset, 0, 4);
                lastFileHeaderOffset = PRESUtils.ConvertNumber(firstFileOffset) - 0x20;
            }

            byte[] EndingOffset = new byte[4];
            Buffer.BlockCopy(rawdata, lastFileHeaderOffset + 0x08, EndingOffset, 0, 4);

            long endposition = PRESUtils.ConvertNumber(EndingOffset);

            if ((int)endposition < 0)
                endposition = endposition;
            Buffer.BlockCopy(rawdata, (int)endposition, firstFileOffset, 0, 0x04);
            endposition = PRESUtils.ConvertNumber(firstFileOffset);

            if (endposition > 0)
                return endposition;
            return 0;
        }

        private static long SinglePRESHeaderAnalyzerEx(byte[] rawdata)
        {
            byte[] firstFileOffset = new byte[4];
            Buffer.BlockCopy(rawdata, 0x50, firstFileOffset, 0, 4);
            int lastFileHeaderOffset = PRESUtils.ConvertNumber(firstFileOffset) - 0x20;

            byte[] EndingOffset = new byte[4];
            Buffer.BlockCopy(rawdata, lastFileHeaderOffset + 0x08, EndingOffset, 0, 4);

            long endposition = PRESUtils.ConvertNumber(EndingOffset);

            Buffer.BlockCopy(rawdata, (int)endposition, firstFileOffset, 0, 0x04);
            endposition = PRESUtils.ConvertNumber(firstFileOffset);

            if (endposition > 0)
                return endposition;
            return 0;
        }
    }
}
