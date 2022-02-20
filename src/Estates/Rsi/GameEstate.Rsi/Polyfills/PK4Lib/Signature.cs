namespace P4KLib
{
    public enum Signature : uint
    {
        CentralDirectory = 0x0201,
        FileStructure = 0x1403,
        CentralDirectoryLocator = 0x0606,
        CentralDirectoryLocatorOffset = 0x0706,
        CentralDirectoryEnd = 0x0605,
    }
}
