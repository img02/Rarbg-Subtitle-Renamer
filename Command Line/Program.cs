using lib;

namespace CommandLine
{
    public class Program
    {
        public const string DefaultLanguage = "English";
        public static void Main(string[] args)
        {
            var lang = DefaultLanguage;
            var priority = 0;
            var outputDir = string.Empty;
            var deleteAfterRename = false;

            //if no args try to rename from curr directory
            if (args.Length == 0)
            {
                Rename(Directory.GetCurrentDirectory(), outputDir, lang, priority, deleteAfterRename);
                return;
            }

            //else try to parse args
            var i = 1;
            var baseDir = args[0];

            if (baseDir is "-h" or "--help")
            {
                PrintHelp();
                return;
            }
            if (!Directory.Exists(baseDir))
            {
                //set base dir as working dir, start args from 0
                baseDir = Directory.GetCurrentDirectory();
                i = 0;
            }

            for (; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-l":
                    case "--language":
                        lang = args[i++ + 1];
                        break;
                    case "-o":
                    case "--output":
                        outputDir = args[i++ + 1];
                        break;
                    case "-d":
                    case "--delete-after":
                        deleteAfterRename = true;
                        if (i != args.Length - 1)
                        {
                            Console.WriteLine("The delete flag must be the last argument.");
                            return;
                        }
                        break;
                    case "-p":
                    case "--priority-file-size":
                        if (!int.TryParse(args[i++ + 1], out priority))
                        {
                            Console.WriteLine("Incorrect priority param.");
                            return;
                        }

                        break;
                    case "-h":
                    case "--help":
                        PrintHelp();
                        break;
                    default:
                        Console.WriteLine($"Unknown command {args[i]}. Use -h for help");
                        return;
                }
            }

            Rename(baseDir, outputDir, lang, priority, deleteAfterRename);
        }

        private static void Rename(string baseDir, string outputDir, string lang, int priority, bool delete)
        {
            Console.WriteLine($"Renaming subs from \n" +
                              $"\t\t{baseDir}");
            if (outputDir != string.Empty) Console.WriteLine("To: \n" +
                                                             $"\t\t {outputDir}");
            if (lang != string.Empty) Console.WriteLine($"For language: '{lang}' with file size priority '{priority}'");
            if (delete) Console.WriteLine("Deleting folder / subdirectories after rename");

            var renamer = new Renamer();
            if (!renamer.SetBaseDirectory(baseDir)) return;
            renamer.SetOutputDirectory(outputDir);
            renamer.SetSubtitleLanguage(lang);
            renamer.SetSubtitleFileSizePriority(priority);
            renamer.SetDeleteAfterRename(delete);
            renamer.RenameAndMoveSubtitles();
        }
        private static void PrintHelp()
        {
            Console.WriteLine("Help: ");
            Console.WriteLine("-------------------------------");
            Console.WriteLine(@"The base directory must either be the first param (e.g. 'D:\Television\Show Name' or 'D:\Television\Show Name\Subs' ) ");
            Console.WriteLine("Or if left out, is assumed to be the current directory.");
            Console.WriteLine();
            Console.WriteLine(@"Basic usage would be to open a terminal in the show's folder, then use 'rarbg-sub-renamer.exe -d'");
            Console.WriteLine("To rename and move Eng subs to the 'Subs' folder and then delete all subdirectories.");
            Console.WriteLine("-------------------------------");
            Console.WriteLine("-l --language");
            Console.WriteLine("\t\t Sets the subtitle language to match. Must match the language in the file name. Defaults to English");
            Console.WriteLine("-p --priority");
            Console.WriteLine("\t\t Sets the file size priority for subtitles, 0 being the largest file.");
            Console.WriteLine("\t\t For example, SDH subtitles (Subtitles for the deaf or hard-of-hearing) will have more lines and therefore be a higher file size");
            Console.WriteLine("\t\t Whereas subtitles that only contain signs or translated foreign lines will be smaller in size");
            Console.WriteLine("-o --output");
            Console.WriteLine("\t\t Sets the output folder path. Defaults to the 'Subs' folder");
            Console.WriteLine("-d --delete");
            Console.WriteLine("\t\t Must be the last flag passed.");
            Console.WriteLine("\t\t Deletes the 'Subs' directory and subdirectories after renaming and outputting to the specified path.");
            Console.WriteLine("\t\t If no path specified, subdirectories will be deleted but the Subs folder will remain, with the new renamed subtitles");
            Console.WriteLine("-h --help");
            Console.WriteLine("\t\t Displays help.");
        }
    }
}