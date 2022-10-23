﻿using System.Resources;

namespace lib;


public class Renamer
{
    private DirectoryInfo? _baseDir;
    private DirectoryInfo? _outputDir;
    private string _language = "english";
    private int _priority = 0;
    private bool _deleteFilesAfterRenaming = false;
    private DirectoryInfo[] baseDirSubDirectories;

    /*public Renamer(string baseDir, string outputDir, string language, int priority = 0)
    {
        _baseDir = baseDir;
        _outputDir = outputDir;
        _language = language;
        _priority = priority;
    }*/

    /// <summary>
    /// Sets the base 'Subs' directory. If the parent video directory is passed, it checks if the 'Subs' folder exists. 
    /// </summary>
    /// <param name="baseDirPath"></param>
    /// <returns>True if 'Subs' folder exists</returns>
    public bool SetBaseDirectory(string baseDirPath)
    {
        if (!Directory.Exists(baseDirPath)) return false;

        if (!baseDirPath.ToLowerInvariant().EndsWith("subs"))
        {
            _baseDir = Directory.CreateDirectory(baseDirPath);
            baseDirSubDirectories = _baseDir.GetDirectories();
            return true;
        }

        var subDir = Directory.GetDirectories(baseDirPath)
            .FirstOrDefault(f => f.ToLowerInvariant() == "subs");
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
    public void SetSubtitleLanguage(string language) => _language = language;

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
    }
}

internal static class Utils
{
    public static void RenameFiles(FileInfo[] files, DirectoryInfo outputDir, string subtitleName , string language, int priority)
    {
        var subs = GetFilesMatchingLanguage(files, language);
        if (subs.Count == 0) return;

        if (priority >= subs.Count) priority = subs.Count-1;

        var subFile = subs[priority];
        var newPath = $@"{outputDir.FullName}\{subtitleName}{subFile.Extension}";
        subFile.MoveTo(newPath, true);
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

}