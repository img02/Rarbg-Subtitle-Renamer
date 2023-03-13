# Rarbg Subtitle Renamer

Requires .net 6

Command line tool for bulk renaming and relocation of sub files for when some rarbg torrents bundle subtitles into individual folders per episode.

e.g.

```
TV Show Name
├── Subs
│   ├── Episode 1
|   |   ├── 2_English.srt
|   |   ├── 3_English.srt
|   |   ├── 2_French.srt
|   |   └── 2_Japanese.srt
|   ├── Episode 2
|   |   ├── 2_English.srt
|   |   ├── 3_English.srt
|   |   ├── 2_French.srt
|   |   └── 2_Japanese.srt
│   └── ...
├── Episode 1.mkv
├── Episode 2.mkv
├── Episode 3.mkv
└── Episode 4.mkv
```
to  
```
TV Show Name
├── Subs
│   ├── Episode 1.eng.srt
│   ├── Episode 2.eng.srt
│   ├── Episode 3.eng.srt
│   └── Episode 4.eng.srt
├── Episode 1.mkv
├── Episode 2.mkv
├── Episode 3.mkv
└── Episode 4.mkv
```  

using 'rarbg-sub-renamer.exe -d'


## Doc
```
Help:
-------------------------------
The base directory must either be the first param (e.g. 'D:\Television\Show Name' or 'D:\Television\Show Name\Subs' )
Or if left out, is assumed to be the current directory.

Basic usage would be to open a terminal in the show's folder, then use 'rarbg-sub-renamer.exe -d'
To rename and move Eng subs to the 'Subs' folder and then delete all subdirectories.
-------------------------------
-l --language
                 Sets the subtitle language to match. Must match the language in the file name. Defaults to English
Multiple language input supported. Example: '-l english german korean'
-p --priority
                 Sets the file size priority for subtitles, 0 being the largest file.
                 For example, SDH subtitles (Subtitles for the deaf or hard-of-hearing) will have more lines and therefore be a higher file size
                 Whereas subtitles that only contain signs or translated foreign lines will be smaller in size
-o --output
                 Sets the output folder path. Defaults to the 'Subs' folder
-d --delete
                 Must be the last flag passed.
                 Deletes the 'Subs' directory and subdirectories after renaming and outputting to the specified path.
                 If no path specified, subdirectories will be deleted but the Subs folder will remain, with the new renamed subtitles
-h --help
                 Displays help.
```
