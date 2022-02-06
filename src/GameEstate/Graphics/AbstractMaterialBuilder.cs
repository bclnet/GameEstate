namespace GameEstate.Graphics
{
    public abstract class AbstractMaterialBuilder<Material, Texture>
    {
        protected TextureManager<Texture> _textureManager;

        public AbstractMaterialBuilder(TextureManager<Texture> textureManager)
            => _textureManager = textureManager;

        public float? NormalGeneratorIntensity = 0.75f;
        public abstract Material DefaultMaterial { get; }
        public abstract Material BuildMaterial(object key);
    }
}
