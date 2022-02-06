using GameEstate.Graphics.OpenGL;
using System;

namespace GameEstate.Graphics.ObjectBuilders
{
    public class OpenGLObjectBuilder : AbstractObjectBuilder<object, Material, int>
    {
        public override void EnsurePrefabContainerExists() { }
        public override object CreateObject(object prefab) => throw new NotImplementedException();
        public override object BuildObject(object source, MaterialManager<Material, int> materialManager) => throw new NotImplementedException();
    }
}