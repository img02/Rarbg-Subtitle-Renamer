using System.Globalization;

namespace lib;


public class Renamer
{
    private DirectoryInfo? _baseDir;
    private DirectoryInfo? _outputDir;
    private string _language = "english";
    private int _priority = 0;
    private bool _deleteFilesAfterRenaming = false;
    private DirectoryInfo[] baseDirSubDirectories;

    /// <summary>
    /// Sets the base 'Subs' directory. If the parent video directory is passed, it checks if the 'Subs' folder exists. 
    /// </summary>
    /// <param name="baseDirPath"></param>
    /// <returns>True if 'Subs' folder exists</returns>
    public bool SetBaseDirectory(string baseDirPath)
    {
        if (!Directory.Exists(baseDirPath)) return false;

        if (baseDirPath.ToLowerInvariant().EndsWith("subs"))
        {
            _baseDir = Directory.CreateDirectory(baseDirPath);
            baseDirSubDirectories = _baseDir.GetDirectories();
            return true;
        }

        var subDir = Directory.GetDirectories(baseDirPath)
            .FirstOrDefault(f => f.ToLowerInvariant().Contains("subs"));
        if (subDir == null) return false;
        else
        {
            _baseDir = Directory.CreateDirectory(subDir);
            baseDirSubDirectories = _baseDir.GetDirectories();
            return true;
        }
    }

    /// <summary>
    /// Sets the output directory for the subtitles. Base directory must be set first. 
    /// If unable to create output dir. the base dir is used. 
    /// </summary>
    /// <param name="outputDir"></param>
    public void SetOutputDirectory(string outputDir)
    {
        if (_baseDir == null) return;

        try
        {
            _outputDir = Directory.CreateDirectory(outputDir);
            return;
        }
        catch (Exception e)
        {
            _outputDir = _baseDir;
        }
    }

    /// <summary>
    /// Sets the language to match from the file names. Defaults to English.
    /// </summary>
    /// <param name="language"></param>
    public void SetSubtitleLanguage(string language) => _language = language.ToLowerInvariant();

    /// <summary>
    /// Sets the subtitle priority by file size.
    /// For example, SDH subtitles will be contain more lines and therefore be the larger file.
    /// Signs and foreign language only subtitles will be the smallest.
    /// </summary>
    /// <param name="priority"></param>
    public void SetSubtitleFileSizePriority(int priority) => _priority = priority;

    /// <summary>
    /// Sets whether or not to delete the 'Subs' folder after renaming.
    /// </summary>
    /// <param name="deleteAfterRename"></param>
    public void SetDeleteAfterRename(bool deleteAfterRename) => _deleteFilesAfterRenaming = deleteAfterRename;

    /// <summary>
    /// Renames and moves subtitles the the output folder based on language, priority inputs.
    /// </summary>
    public void RenameAndMoveSubtitles()
    {
        if (_baseDir == null) return;
        if (_outputDir == null) SetOutputDirectory(string.Empty);

        foreach (var folder in _baseDir.GetDirectories())
        {
            var subName = folder.Name;
            var files = folder.GetFiles();
            Utils.RenameFiles(files, _outputDir!, subName, _language, _priority);
        }

        if (_deleteFilesAfterRenaming) DeleteSubFolders();
    }

    private void DeleteSubFolders()
    {
        foreach (var dir in baseDirSubDirectories)
        {
            dir.Delete(true);
        }
        if (_baseDir!.GetDirectories().Length == 0 && _baseDir.GetFiles().Length == 0) _baseDir.Delete();
    }
}

internal static class Utils
{
    public static void RenameFiles(FileInfo[] files, DirectoryInfo outputDir, string subtitleName, string language, int priority)
    {
        var languageCode = language;
        if (languageCode.Length > 3)//If 3 chars or less, we assume the language provided was already a valid language code
        {
            languageCode = GetISOCodeMatchingLanguage(language);
        }

        var subs = GetFilesMatchingLanguage(files, language);
        if (subs.Count == 0) return;

        if (priority >= subs.Count) priority = subs.Count - 1;

        //sort by file size
        subs.Sort((s1, s2) => s2.Length.CompareTo(s1.Length));

        var subFile = subs[priority];
        var newPath = Path.Combine($@"{outputDir.FullName}", $@"{subtitleName}.{languageCode}{subFile.Extension}");
        subFile.CopyTo(newPath, true);
    }

    private static List<FileInfo> GetFilesMatchingLanguage(FileInfo[] files, string language)
    {
        var subs = new List<FileInfo>();

        foreach (var fileInfo in files)
        {
            if (fileInfo.Name.ToLowerInvariant().Contains(language)) subs.Add(fileInfo);
        }

        return subs;
    }
    public static string GetISOCodeMatchingLanguage(string languageName)
    {
        //Compile a dict to access a culture by its english name
        var cultureInfosDict = CultureInfo.GetCultures(CultureTypes.AllCultures)
                    .ToLookup(x => x.EnglishName.ToLowerInvariant());

        var requestedCulture = cultureInfosDict[languageName].FirstOrDefault();
        var languageCode = requestedCulture?.ThreeLetterISOLanguageName;

        //If languageCode was not acquired, return the provided language.
        return languageCode ?? languageName;
    }

}
