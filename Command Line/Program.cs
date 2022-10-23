using lib;

namespace CommandLine
{
    public class Program
    {
        public static void Main(string[] args)
        {

            /*
            if (args.Length == 0) return;

            //base dir
            var baseDir = args[0];
            if (!Directory.Exists(baseDir))
            {
                Console.WriteLine("Unable to find base directory");
                return;
            }


            //language
            var lang = string.Empty;
            try
            {
                lang = args[1];
            }
            catch (IndexOutOfRangeException)
            {
            }

            //sub filesize priority
            var priority = 0;
            try
            {
                int.TryParse(args[2], out priority);
            }
            catch (IndexOutOfRangeException)
            {
            }

            //output dir
            var outputDir = string.Empty;
            try
            {
                outputDir = args[3];
            }
            catch (IndexOutOfRangeException)
            {
            }

            //delete after rename
            var deleteAfterRename = false;



            for (int i = 1; i < args.Length; i++)
            {
                // iterate through args, args[0] must be the base directory.

                //match flags, -l --language, -d --delete, -o --output, -h --help;  use switch statement?

                //delete should jsut take flag to mark as true

                //others should iterate i + 1 to get paired parameter (example: -l english)

                //if any errors, maybe print reason, then quit 
            }
            */




            var baseDir1 = @"Z:\New\Schitts.Creek.S01.1080p.WEBRip.x265-RARBG";
            var outputdir1 = @"Z:\New\Schitts.Creek.S01.1080p.WEBRip.x265-RARBG\test";
            var renamer1 = new Renamer();
            renamer1.SetBaseDirectory(baseDir1);
            renamer1.SetOutputDirectory(outputdir1);
            renamer1.RenameAndMoveSubtitles();

            /*var renamer = new Renamer();
            if (renamer.SetBaseDirectory(baseDir)) return;
            renamer.SetOutputDirectory(outputDir);
            renamer.SetSubtitleLanguage(lang);
            renamer.SetSubtitleFileSizePriority(priority);
            renamer.SetDeleteAfterRename(deleteAfterRename);
            renamer.RenameAndMoveSubtitles();*/
        }
    }
}

/*
 *
 * get base dir (should be the Subs folder)
 *
 * get list of subfolders
 *  
 * get language to look for
 * 
 * get file size prior?
 * (e.g. SDH subtitles for hearing impaired will be the largest file size,
 * whereas some videos come with min subtitles for other spoken languages or signs which will be the smallest file size)
 *
 * if (file size prior not avail) go with closest.
 *
 *
 * set to video folder?
 *      -o 'output path'  -- create dir and save to here
 *      else move renamed subs to base Subs folder
 *
 * clean up remaining subtitles after? -d
 *      if used - delete all subfolders and files in them (pref move to recycle bin, if simple to implement)
 *
 *
 *
 * for each subfolder
 *      get subfolder name
 *      reorder files based on size (largest > smallest?)
 *      find first that matches language name and priority
 *      rename to subfolder name + original extention (prob .srt)
 *      move file out to ../Subs/MOVE FILE HERE 
 * DONE
 *
 *
 *
 *
 */