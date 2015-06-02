### Version 1.1.0 beta 1 for Unity3d 3.0.0 beta4 ###
`2010-07-28`

I needed bit-gui with for unity3d 3.0.0b4 and it blew in my face!

I fixed most problems and released a new version. Also updated the screenshot at the main page. It need more tests, anything please fell free to post an Issue.

- Sperry

### Working ###
`2010-06-30`

The lastest changes from our internal repository are integrated. Most of features are already working on new example scene. More will be added/updatede soon.

- Willian

### Mercurial ###
`2010-06-25`

Migrating the project to mercurial. Will ease the integration with the lastest changes from our internal repository.

- Sperry

### Working ###
`2010-02-22`

Sorry about the delay to commit. I'm working on some new Controls and the entire system is now kinda unstable. But as soon as the project is "commitable" again, I'll do it :)

- Kleber

### Incompatibility ###
`2010-02-05`

There was an incompatibility with default Unity compiler. Here we was using another compiler (Mono 2.4), but for now, we rollback to make the GUI system compatible with Unity default installation.


### Opensourcing ###
`2010-01-15`

In january 2009 we got the approval from Hoplon to publish opensource the project.


### Start ###
`2010-01-12`

Bitverse-unity-gui started in december 2009 at [Hoplon Infotainment](http://www.hoplon.com/) as a RD free time project for [Taikodom 2.0](http://www.taikodom.com/). We intented to build a unity3d GUI system that could be loaded from xml. It was supposed to be a quick experiment in the last week before christmas. Someone recalled [reading](http://forum.unity3d.com/viewtopic.php?t=34027) about [guiforms](http://code.google.com/p/unityforms/) from Marcelo Roca, a clean and simple winforms like library over unity immediate mode gui. So we modified guiforms code to use it inside the editor and started coding the editor tools. Soon it became obvious that the unity editor (with prefabs and assetbundles) approach was better suited for us than the xml parsing.