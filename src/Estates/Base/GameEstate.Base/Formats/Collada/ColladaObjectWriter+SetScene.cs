using grendgine_collada;

namespace GameEstate.Formats.Collada
{
    partial class ColladaObjectWriter
    {
        /// <summary>
        /// Adds the Scene element to the Collada document.
        /// </summary>
        void SetScene()
            => daeObject.Scene = new Grendgine_Collada_Scene
            {
                Visual_Scene = new Grendgine_Collada_Instance_Visual_Scene { URL = "#Scene", Name = "Scene" }
            };
    }
}