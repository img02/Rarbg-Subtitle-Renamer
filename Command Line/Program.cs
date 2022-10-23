using lib;

namespace CommandLine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0) return;

            var baseDir = args[0];

            if (baseDir is "-h" or "--help")
            {
                PrintHelp();
                return;
            }
            if (!Directory.Exists(baseDir))
            {
                Console.WriteLine($"Unable to find directory - {baseDir}");
                return;
            }

            var lang = string.Empty;
            var priority = 0;
            var outputDir = string.Empty;
            var deleteAfterRename = false;

            for (int i = 1; i < args.Length; i++)
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
                    case "-h":
                    case "--help":
                        PrintHelp();
                        break;
                    default:
                        Console.WriteLine($"Unknown command {args[i]}. Use -h for help");
                        break;
                }
            }

            var renamer = new Renamer();
            if (!renamer.SetBaseDirectory(baseDir)) return;
            renamer.SetOutputDirectory(outputDir);
            renamer.SetSubtitleLanguage(lang);
            renamer.SetSubtitleFileSizePriority(priority);
            renamer.SetDeleteAfterRename(deleteAfterRename);
            renamer.RenameAndMoveSubtitles();
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Help: ");
            Console.WriteLine("-------------------------------");
            Console.WriteLine(@"The first param must be the video or sub folder (e.g. 'D:\Movies\Show Name' or 'D:\Movies\Show Name\Subs' ) ");
            Console.WriteLine("-------------------------------");
            Console.WriteLine("-l --language");
            Console.WriteLine("\t\t Sets the subtitle language to match. Must match the language in the file name. Defaults to English");
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