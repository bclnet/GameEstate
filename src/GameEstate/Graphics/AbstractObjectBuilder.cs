namespace GameEstate.Graphics
{
    public abstract class AbstractObjectBuilder<Object, Material, Texture>
    {
        public abstract Object CreateObject(Object prefab);
        public abstract void EnsurePrefabContainerExists();
        public abstract Object BuildObject(object source, MaterialManager<Material, Texture> materialManager);
    }
}
