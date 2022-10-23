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

            Console.WriteLine($"{baseDir} | {outputDir} | {lang} | {priority} | {deleteAfterRename}");

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
            Console.WriteLine("hello this is the help text");
            Console.WriteLine("tbd");
        }
    }
}