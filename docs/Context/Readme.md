Context
===============

Estate Context

---

## EstateManager

| type          | name          | description
| ---           | ---           | ---   
| IDictionary<string, Estate> | Estates | The estates.
| Estate        | GetEstate(string estateName) | Gets the specified estate.
     


## Estate

| type          | name          | description
| ---           | ---           | ---   
| string        | Id            | The estate identifier.
| string        | Name          | The estate name.
| string        | Studio        | The estate studio.
| string        | Description   | The estate description.
| Type          | PakFileType   | The type of the pak file.
| PakMultiType  | PakMulti      | The multi-pak.
| Type          | Pak2FileType  | The type of the pak file.
| PakMultiType  | Pak2Multi     | The multi-pak.
| IDictionary<string, EstateGame> | Games | Gets the estates games.
| FileManager   | FileManager   | Gets the estates file manager.
| (string id, EstateGame game)  | GetGame(string id) | Gets the estates game.
| Resource      | ParseResource(Uri uri) | Parses the estates resource.
| EstatePakFile | OpenPakFile(string[] filePaths, string game) | Opens the estates pak file.
| EstatePakFile | OpenPakFile(Resource resource) | Opens the estates pak file.
| EstatePakFile | OpenPakFile(Uri uri) | Opens the estates pak file.

## struct Resource
* bool StreamPak
* Uri Host
* string[] Paths
* string Game

## class EstateGame
* string Game
* string Name
* IList<Uri> DefaultPaks
* bool Found


## EstateDebug
