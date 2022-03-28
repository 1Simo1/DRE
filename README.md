# DRE Editor - First Readme version

Editor for DreeRally/Death Rally

## Getting Started

First of all, open and compile code in Visual Studio. DRE editor starts from a Uno Platform template, so it should be cross-platform, in particular:

- Choose, compile and run "DRE.Windows.Package" configuration for Windows 11 and 10 version.

- "DRE.Skia.WPF.Host" for Windows 7

- "Dre.SKia.GTK" for Linux/WSL into Windows

### First DRE Setup

At the first editor execution, or if you choose to rename/delete DRE's database, program starts with language selection 

![select_language](https://user-images.githubusercontent.com/37436823/160473012-2bdc6127-cc07-40bf-8fc7-6ea9751e3ef8.png)

click on corresponding flag, or, if you don't see flags, on two letter language identifier (IT/EN/ES)

**from now on, due to a known issue, please maximize window for optimal editor use**

Steps for setup

![setup_1](https://user-images.githubusercontent.com/37436823/160474424-c6e0e056-d7a9-4fff-8df1-55b93d3b28b8.png)

- Write new project name and press "Confirm" as shown in above image
- Now the search for a suitable game folder starts, when you see your folder in list, you can already choose it, you don't need to wait searching ending, you can now click enabled "DRE Project Setup" button
- Wait for setup steps (you can also see messages while in progress), until you see this :

![setup_3](https://user-images.githubusercontent.com/37436823/160475613-91bde35f-a3e4-492a-b01a-61b9173f0318.png)

Good! All set, press button "DRE Project ready!" and you can start to use editor!

### Editor features

***Make a copy of your files before editing, some features overwrite game file(s)***

At the moment, with DRE Editor you can:

- Manage BPA Archives (mainly: see it, extract files as they are, extract images)
- Edit savegames (I think almost completely!)
- Manage Tracks (for now, open Tracks, extract images, and extract all textures)

Some other implementation is already here, but at the moment "only" saved and loaded using editor database.

### Screenshots

![bpa](https://user-images.githubusercontent.com/37436823/160477589-e7ef8af7-bef3-43af-89a0-a2bc1dd996bf.png)

 You can extract ALL game default "standard menu images" with only one click, or you can choose one BPA from horizontal list, then view archive file list, choose one of them and view available operations (normally, extract it from archive as is, expand it if is BPK, extract images if possibile).
 
 ![sg](https://user-images.githubusercontent.com/37436823/160478363-475b1cf7-28a3-4288-8466-903740717a1f.png)

In this screenshot, DR.SG7 savegame is already chosen, and from driver list my player data is already chosen and detailed, from now on you can also choose any other driver to modify and/or to edit main game information(s) (e.g. Savegame name).
When done, click on "Save Game", which overwrites SG in game folder! Also, if you have played DR and saved a new game file, you can load it into editor with "Update from file" (that reads from game folder and updates program database)

![trk](https://user-images.githubusercontent.com/37436823/160479424-d3138525-7130-4b62-97f7-c486ac09aa35.png)

Now, track page. At the moment, you can choose a track from game track list (in this example, The Arena is selected), wait for track data loading into editor, then you'll see now enabled buttons.
At the moment, you can only "Extract textures".

## Editor folders

There are two main folder, both direct subfolders of DRE executable location : 

- db, which contains, well, default database and DRE database. Default database is only written at the end of first setup. So, if you have some issue with DRE database (which is main project database) you can delete DRE.db file, then copy DEFAULT.db in DRE.db.
  - this folder contains also locale folder, for xml localization files
- files, which contains all files extracted from editor. It's divided into BPA, TRK, and TRK has one folder per track.

None of this file is in repository. In fact, first setup writes almost all the initial folder/file structure you need!
 
