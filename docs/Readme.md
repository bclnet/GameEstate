Game Estate
===============

Game Estate is an open-source, cross-platform solution for delivering game assets as a service.

### Game Estate Benefits
* Portable (windows, apple, linux, mobile, intel, arm)
* Loads textures, models, animations, sounds, and levels
* Avaliable with streaming assets (cached)
* References assets with a uniform resource location (url)
* Loaders for Unreal and Unity
* Locates installed games
* Estate centric context
* Includes a desktop app to explore assets
* Includes a command line interface to export assets (list, unpack, shred)
* *future:* Usage tracking (think Spotify)
* *future:* Entitlement (think drm)

### Context
    Context

### Location (find installed games)
    First step is locating installed games
    Location definition by platform. For instance windows usually uses registration entries.

### Runtime (c++ vs .net)
    dotnet runtime
    Hosted manage for unreal or native

### Uniform Resource Location (url)
    TBD

## [Applications](Applications/Readme.md)
Multiple applicates are included in GameEstate to make it easier to work with the game assets.

## [Context](Context/Readme.md)
Estate Context

## [Estates](Estates/Readme.md)
Estates are the primary grouping mechanism for interacting with the asset services.

## [Platforms](Platforms/Readme.md)
Platforms provide the interface to each platform.
