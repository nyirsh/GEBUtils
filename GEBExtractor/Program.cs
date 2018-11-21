using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using GEBUtils.FHandlers;

namespace GEBUtils
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("\nGod Eater Burst Universal Tool v0.4 Beta - by DarkVanth (DarkVanth@msn.com)\n\n");

            try
            {
                if (args.Length > 0)
                {
                    switch (args[0])
                    {
                        case "-extract-rdp":
                            #region
                            if (args.Length == 1)
                            {
                                Console.WriteLine(" -extract-rdp [iso.path]\t   Extracts the package.rdp from a GEB Iso\n");
                                Console.WriteLine(" iso.path : the path of the GEB Iso");
                            }
                            else
                            {
                                if (File.Exists(args[1]))
                                {
                                    Console.WriteLine("Extracting package.rdp");
                                    if (IsoOperations.Extract(args[1], true))
                                        Console.WriteLine("File package.rdp extracted!");
                                    else
                                        Console.WriteLine("The selected file is not a valid God Eater Burst Iso");
                                }
                                else
                                {
                                    Console.WriteLine("Could not find the file: " + args[1]);
                                }
                            }
                            break;
                            #endregion
                        
                        case "-insert-rdp":
                            #region
                            if (args.Length == 1)
                            {
                                Console.WriteLine(" -insert-rdp [iso.path]\t   Inerts the package.rdp into a GEB Iso\n");
                                Console.WriteLine(" iso.path : the path of the GEB Iso");
                            }
                            else
                            {
                                if (File.Exists("package.rdp"))
                                {
                                    if (File.Exists(args[1]))
                                    {
                                        Console.WriteLine("Inserting package.rdp");
                                        switch (IsoOperations.Insert(args[1], true))
                                        {
                                            case -2:
                                                Console.WriteLine("The package.rdp has a different dimension from the original one.");
                                                Console.WriteLine("This will cause your PSP to freeze. File not inserted");
                                                break;
                                            case -1:
                                                Console.WriteLine("The selected file is not a valid God Eater Burst Iso");
                                                break;
                                            case 1:
                                                Console.WriteLine("File package.rdp inserted!");
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Could not find the file: " + args[1]);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Could not find the file: package.rdp");
                                }
                            }
                            break;
                            #endregion
                        
                        case "-gen-index":
                            #region

                            if (File.Exists("package.rdp"))
                            {
                                RDPOperations.GenerateIndex(true);
                            }
                            else
                            {
                                Console.WriteLine("Could not find the file: package.rdp");
                            }
                            break;
                            #endregion

                        case "-unpack":
                            #region

                            string un_specifics = "";
                            if (args.Length > 1)
                            {
                                un_specifics = args[1];
                                for (int i = 2; i < args.Length; i++)
                                {
                                    un_specifics += "|" + args[i];
                                }

                                if (File.Exists("index.lst"))
                                {
                                    RDPOperations.Unpack(un_specifics, true);
                                }
                                else
                                {
                                    Console.WriteLine("Could not find the file: index.lst");
                                    Console.WriteLine("Use -gen-index command first");
                                }
                            }
                            else
                            {
                                Console.WriteLine(" -unpack [ext]\t\t   Unpacks the specified files type from package.rdp\n");
                                Console.WriteLine(" Exts:");
                                Console.WriteLine("   all\t\t Extracts all files type");
                                Console.WriteLine("   pmf\t\t Extracts all pmf files");
                                Console.WriteLine("   at3\t\t Extracts all at3 files");
                                Console.WriteLine("   gim\t\t Extracts all gim files");
                                Console.WriteLine("   gmo\t\t Extracts all gmo files");
                                Console.WriteLine("   vag\t\t Extracts all vag files");
                                Console.WriteLine("   tr2\t\t Extracts all tr2 files\n");
                                Console.WriteLine("   pres\t\t Extracts all pres files\n");
                                Console.WriteLine(" You can also use more than one extension separated by a space:");
                                Console.WriteLine(" -unpack gim tr2\t\tUnpacks only gim and tr2 files\n"); ;
                            }
                            break;
                            #endregion

                        case "-pack":
                            #region

                            string specifics = "";
                            if (args.Length > 1)
                            {
                                specifics = args[1];
                                for (int i = 2; i < args.Length; i++)
                                {
                                    specifics += "|" + args[i];
                                }

                                if (File.Exists("index.lst"))
                                {
                                    RDPOperations.Pack(specifics, true);
                                }
                                else
                                {
                                    Console.WriteLine("Could not find the file: index.lst");
                                    Console.WriteLine("Use -gen-index command first");
                                }
                            }
                            else
                            {
                                Console.WriteLine(" -pack [ext]\t\t   Packs the specified files type in package.rdp\n");
                                Console.WriteLine(" Exts:");
                                Console.WriteLine("   all\t\t Inject all files type");
                                Console.WriteLine("   pmf\t\t Inject all pmf files");
                                Console.WriteLine("   at3\t\t Inject all at3 files");
                                Console.WriteLine("   gim\t\t Inject all gim files");
                                Console.WriteLine("   gmo\t\t Inject all gmo files");
                                Console.WriteLine("   vag\t\t Inject all vag files");
                                Console.WriteLine("   tr2\t\t Inject all tr2 files\n");
                                Console.WriteLine("   pres\t\t Inject all pres files\n");
                                Console.WriteLine(" You can also use more than one extension separated by a space:");
                                Console.WriteLine(" -pack gim tr2\t\tInject only gim and tr2 files\n"); ;
                            }
                            break;
                            #endregion

                        case "-unpack-single":
                            #region

                            string s_un_specifics = "";
                            if (args.Length > 1)
                            {
                                s_un_specifics = args[1];
                                for (int i = 2; i < args.Length; i++)
                                {
                                    s_un_specifics += "|" + args[i];
                                }

                                if (File.Exists("index.lst"))
                                {
                                    RDPOperations.SingleUnpack(s_un_specifics, true);
                                }
                                else
                                {
                                    Console.WriteLine("Could not find the file: index.lst");
                                    Console.WriteLine("Use -gen-index command first");
                                }
                            }
                            else
                            {
                                Console.WriteLine(" -unpack-single [files]\t   Unpacks the specified files\n");
                                Console.WriteLine(" You can also use more than one file separated by a space:");
                                Console.WriteLine(" -unpack-single 00001.gim 00012.at3\t\tUnpacks only 00001.gim and 00012.at3\n");
                            }
                            break;
                            #endregion

                        case "-pack-single":
                            #region

                            string s_specifics = "";
                            if (args.Length > 1)
                            {
                                s_specifics = args[1];
                                for (int i = 2; i < args.Length; i++)
                                {
                                    s_specifics += "|" + args[i];
                                }

                                if (File.Exists("index.lst"))
                                {
                                    RDPOperations.SinglePack(s_specifics, true);
                                }
                                else
                                {
                                    Console.WriteLine("Could not find the file: index.lst");
                                    Console.WriteLine("Use -gen-index command first");
                                }
                            }
                            else
                            {
                                Console.WriteLine(" -pack-single [files]\t   Packs the specified files\n");
                                Console.WriteLine(" You can also use more than one file separated by a space:");
                                Console.WriteLine(" -pack-single 00001.gim 00012.at3\t\tPacks only 00001.gim and 00012.at3\n");
                            }
                            break;
                            #endregion

                        case "-dlc-gen-index":
                            #region
                            if (File.Exists("dlc.edat"))
                            {
                                EDATOperations.GenerateIndex(true);
                            }
                            else
                            {
                                Console.WriteLine("Could not find the file: package.rdp");
                            }
                            break;
                            #endregion

                        case "-dlc-unpack":
                            #region

                            string dlc_un_specifics = "";
                            if (args.Length > 1)
                            {
                                dlc_un_specifics = args[1];
                                for (int i = 2; i < args.Length; i++)
                                {
                                    dlc_un_specifics += "|" + args[i];
                                }

                                if (File.Exists("dlc-index.lst"))
                                {
                                    EDATOperations.Unpack(dlc_un_specifics, true);
                                }
                                else
                                {
                                    Console.WriteLine("Could not find the file: dlc-index.lst");
                                    Console.WriteLine("Use -dlc-gen-index command first");
                                }
                            }
                            else
                            {
                                Console.WriteLine(" -dlc-unpack [ext]\t\t   Unpacks the specified files type from dlc.edat\n");
                                Console.WriteLine(" Exts:");
                                Console.WriteLine("   all\t\t Extracts all files type");
                                Console.WriteLine("   pmf\t\t Extracts all pmf files");
                                Console.WriteLine("   at3\t\t Extracts all at3 files");
                                Console.WriteLine("   gim\t\t Extracts all gim files");
                                Console.WriteLine("   gmo\t\t Extracts all gmo files");
                                Console.WriteLine("   vag\t\t Extracts all vag files");
                                Console.WriteLine("   tr2\t\t Extracts all tr2 files\n");
                                Console.WriteLine("   pres\t\t Extracts all pres files\n");
                                Console.WriteLine(" You can also use more than one extension separated by a space:");
                                Console.WriteLine(" -unpack gim tr2\t\tUnpacks only gim and tr2 files\n"); ;
                            }
                            break;
                            #endregion

                        case "-dlc-pack":
                            #region

                            string dlc_specifics = "";
                            if (args.Length > 1)
                            {
                                dlc_specifics = args[1];
                                for (int i = 2; i < args.Length; i++)
                                {
                                    dlc_specifics += "|" + args[i];
                                }

                                if (File.Exists("dlc-index.lst"))
                                {
                                    EDATOperations.Pack(dlc_specifics, true);
                                }
                                else
                                {
                                    Console.WriteLine("Could not find the file: dlc-index.lst");
                                    Console.WriteLine("Use -dlc-gen-index command first");
                                }
                            }
                            else
                            {
                                Console.WriteLine(" -dlc-pack [ext]\t\t   Packs the specified files type in dlc.edat\n");
                                Console.WriteLine(" Exts:");
                                Console.WriteLine("   all\t\t Inject all files type");
                                Console.WriteLine("   pmf\t\t Inject all pmf files");
                                Console.WriteLine("   at3\t\t Inject all at3 files");
                                Console.WriteLine("   gim\t\t Inject all gim files");
                                Console.WriteLine("   gmo\t\t Inject all gmo files");
                                Console.WriteLine("   vag\t\t Inject all vag files");
                                Console.WriteLine("   tr2\t\t Inject all tr2 files\n");
                                Console.WriteLine("   pres\t\t Inject all pres files\n");
                                Console.WriteLine(" You can also use more than one extension separated by a space:");
                                Console.WriteLine(" -pack gim tr2\t\tInject only gim and tr2 files\n"); ;
                            }
                            break;
                            #endregion

                        case "-dlc-unpack-single":
                            #region

                            string dlc_s_un_specifics = "";
                            if (args.Length > 1)
                            {
                                dlc_s_un_specifics = args[1];
                                for (int i = 2; i < args.Length; i++)
                                {
                                    dlc_s_un_specifics += "|" + args[i];
                                }

                                if (File.Exists("dlc-index.lst"))
                                {
                                    RDPOperations.SingleUnpack(dlc_s_un_specifics, true);
                                }
                                else
                                {
                                    Console.WriteLine("Could not find the file: dlc-index.lst");
                                    Console.WriteLine("Use -dlc-gen-index command first");
                                }
                            }
                            else
                            {
                                Console.WriteLine(" -dlcunpack-single [files]\t   Unpacks the specified files from dlc.edat\n");
                                Console.WriteLine(" You can also use more than one file separated by a space:");
                                Console.WriteLine(" -dlcunpack-single 00001.gim 00012.at3\t\tUnpacks only 00001.gim and 00012.at3\n");
                            }
                            break;
                            #endregion

                        case "-dlc-pack-single":
                            #region

                            string dlc_s_specifics = "";
                            if (args.Length > 1)
                            {
                                dlc_s_specifics = args[1];
                                for (int i = 2; i < args.Length; i++)
                                {
                                    dlc_s_specifics += "|" + args[i];
                                }

                                if (File.Exists("dlc-index.lst"))
                                {
                                    RDPOperations.SinglePack(dlc_s_specifics, true);
                                }
                                else
                                {
                                    Console.WriteLine("Could not find the file: dlc-index.lst");
                                    Console.WriteLine("Use -dlc-gen-index command first");
                                }
                            }
                            else
                            {
                                Console.WriteLine(" -dlc-pack-single [files]\t   Packs the specified files in dlc.edat\n");
                                Console.WriteLine(" You can also use more than one file separated by a space:");
                                Console.WriteLine(" -dlc-pack-single 00001.gim 00012.at3\t\tPacks only 00001.gim and 00012.at3\n");
                            }
                            break;
                            #endregion

                        default:
                            UnknownCommand(args[0]);
                            break;
                    }
                }
                else
                {
                    Help();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n" + ex.Message);
            }
#if DEBUG
            Console.ReadKey();
#endif
        }

        private static void UnknownCommand(string command)
        {
            Console.WriteLine("Unknown command: " + command);
        }

        private static void Help()
        {
            Console.WriteLine("Commands:\n");
            Console.WriteLine(" -extract-rdp\t\t   Extracts the package.rdp from a GEB Iso");
            Console.WriteLine(" -insert-rdp\t\t   Injects the package.rdp into a GEB Iso");
            Console.WriteLine("");
            Console.WriteLine(" -gen-index\t\t   Generates the package.rdp files index");
            Console.WriteLine(" -unpack\t\t   Unpacks the specified files type from package.rdp");
            Console.WriteLine(" -unpack-single\t\t   Unpacks the specified files");
            Console.WriteLine(" -pack\t\t\t   Packs the specified files type in package.rdp");
            Console.WriteLine(" -pack-single\t\t   Packs the specified files");
            Console.WriteLine("");
            Console.WriteLine(" -dlc-gen-index\t\t   Generates the dlc.edat files index");
            Console.WriteLine(" -dlc-unpack\t\t   Unpacks the specified files type from dlc.edat");
            Console.WriteLine(" -dlc-unpack-single\t   Unpacks the specified files from dlc.edat");
            Console.WriteLine(" -dlc-pack\t\t   Packs the specified files type in dlc.edat");
            Console.WriteLine(" -dlc-pack-single\t   Packs the specified files in dlc.edat");
        }
    }
}
