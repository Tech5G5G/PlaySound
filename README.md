<h1 align="center">PlaySound</h1>

PlaySound is a simple application that can act as a sound board based on the [Windows.Media.Playback.MediaPlayer](https://learn.microsoft.com/en-us/uwp/api/windows.media.playback.mediaplayer) Windows API. You start with a production, to which you may add scenes. A scene is a grouping of sound effects. Each sound effect can have its volume controlled independently of others and be looped if desired. Once you've assembled your production, you can save and reopen it for later use.

Here's an example model of a production:
```
My production.prod
│
├── Scene 1
|   ├── Sound effect 1
|   └── Sound effect 2
|
└── Scene 2
    ├── Sound effect 3
    └── Sound effect 4
```

PlaySound is currently in the prototype phase, so it's very barebones. Feel free to contribute if you'd like.
