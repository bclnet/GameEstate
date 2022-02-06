using System.Text.Json;

namespace GameEstate
{
    /// <summary>
    /// LinuxFileManager
    /// </summary>
    public class LinuxFileManager : FileManager
    {
        #region Parse File-Manager

        public override FileManager ParseFileManager(JsonElement elem)
        {
            base.ParseFileManager(elem);
            if (!elem.TryGetProperty("linux", out var z)) return this;
            //elem = z;

            return this;
        }

        #endregion
    }
}

