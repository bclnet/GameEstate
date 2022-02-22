using GameEstate.Formats.Generic;
using System.IO;
using System.Reflection;
using static GameEstate.EstateDebug;

namespace GameEstate.Formats.Wavefront
{
    public partial class WavefrontObjectWriter
    {
        void WriteMaterialFile(IGenericFile file)
        {
            if (file.Materials == null) { Log("No materials loaded"); return; }

            if (!MaterialFile.Directory.Exists) MaterialFile.Directory.Create();

            using var w = new StreamWriter(MaterialFile.FullName);
            w.WriteLine("# gamer .mtl export version {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            w.WriteLine("#");
            foreach (var material in file.Materials)
            {
                w.WriteLine("newmtl {0}", material.Name);
                if (material.Diffuse != null)
                {
                    w.WriteLine("Ka {0:F6} {1:F6} {2:F6}", material.Diffuse.X, material.Diffuse.Y, material.Diffuse.Z); // Ambient
                    w.WriteLine("Kd {0:F6} {1:F6} {2:F6}", material.Diffuse.X, material.Diffuse.Y, material.Diffuse.Z); // Diffuse
                }
                else Log($"Skipping Diffuse for {material.Name}");
                if (material.Specular != null)
                {
                    w.WriteLine("Ks {0:F6} {1:F6} {2:F6}", material.Specular.X, material.Specular.Y, material.Specular.Z); // Specular
                    w.WriteLine("Ns {0:F6}", material.Shininess / 255D); // Specular Exponent
                }
                else Log($"Skipping Specular for {material.Name}");
                w.WriteLine("d {0:F6}", material.Opacity); // Dissolve

                w.WriteLine("illum 2"); // Highlight on. This is a guess.

                // Phong materials

                // 0. Color on and Ambient off
                // 1. Color on and Ambient on
                // 2. Highlight on
                // 3. Reflection on and Ray trace on
                // 4. Transparency: Glass on, Reflection: Ray trace on
                // 5. Reflection: Fresnel on and Ray trace on
                // 6. Transparency: Refraction on, Reflection: Fresnel off and Ray trace on
                // 7. Transparency: Refraction on, Reflection: Fresnel on and Ray trace on
                // 8. Reflection on and Ray trace off
                // 9. Transparency: Glass on, Reflection: Ray trace off
                // 10. Casts shadows onto invisible surfaces
                foreach (var texture in material.Textures)
                {
                    var texturePath = DataDir != null ? Path.Combine(DataDir.FullName, texture.Path) : texture.Path;
                    texturePath = !TiffTextures ? texturePath.Replace(".tif", ".dds") : texturePath.Replace(".dds", ".tif");
                    texturePath = texturePath.Replace(@"/", @"\");
                    switch (texture.Map)
                    {
                        case TextureMap.Diffuse:
                            w.WriteLine("map_Kd {0}", texturePath);
                            break;
                        case TextureMap.Specular:
                            w.WriteLine("map_Ks {0}", texturePath);
                            w.WriteLine("map_Ns {0}", texturePath);
                            break;
                        case TextureMap.Bumpmap:
                        case TextureMap.Detail:
                            // <Texture Map="Detail" File="textures/unified_detail/metal/metal_scratches_a_detail.tif" />
                            w.WriteLine("map_bump {0}", texturePath);
                            break;
                        case TextureMap.Heightmap:
                            // <Texture Map="Heightmap" File="objects/spaceships/ships/aegs/gladius/textures/aegs_switches_buttons_disp.tif"/>
                            w.WriteLine("disp {0}", texturePath);
                            break;
                        case TextureMap.Decal:
                            // <Texture Map="Decal" File="objects/spaceships/ships/aegs/textures/interior/metal/aegs_int_metal_alum_bare_diff.tif"/>
                            w.WriteLine("decal {0}", texturePath);
                            break;
                        case TextureMap.SubSurface:
                            // <Texture Map="SubSurface" File="objects/spaceships/ships/aegs/textures/interior/atlas/aegs_int_atlas_retaliator_spec.tif"/>
                            w.WriteLine("map_Ns {0}", texturePath);
                            break;
                        case TextureMap.Custom:
                            // <Texture Map="Custom" File="objects/spaceships/ships/aegs/textures/interior/metal/aegs_int_metal_painted_red_ddna.tif"/>
                            // file.WriteLine("decal {0}", textureFile);
                            break;
                        case TextureMap.BlendDetail:
                            // <Texture Map="BlendDetail" File="textures/unified_detail/metal/metal_scratches-01_detail.tif">
                            break;
                        case TextureMap.Opacity:
                            // <Texture Map="Opacity" File="objects/spaceships/ships/aegs/textures/interior/blend/interior_blnd_a_diff.tif"/>
                            w.WriteLine("map_d {0}", texturePath);
                            break;
                        case TextureMap.Environment:
                            // <Texture Map="Environment" File="nearest_cubemap" TexType="7"/>
                            break;
                        default: break;
                    }
                }
                w.WriteLine();
            }
        }
    }
}