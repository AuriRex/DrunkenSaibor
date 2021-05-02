using UnityEngine;

namespace DrunkenSaibor.Data
{
    public class DrunkEffectData
    {
        public bool ShouldRender { get; set; } = true;
        public bool EnabledByDefault { get; set; } = true;

        public DrunkEffectData(ShaderEffect sfx)
        {
            ReferenceName = sfx.referenceName;
            Name = sfx.name;
            Author = sfx.author;
            Description = sfx.description;
            IsScreenSpace = sfx.isScreenSpace;
            PreviewImage = sfx.previewImage;
            Material = sfx.material;
        }

        public DrunkEffectData(string referenceName, string name, string author, string description, bool isScreenSpace, Texture2D previewImage, Material material)
        {
            ReferenceName = referenceName;
            Name = name;
            Author = author;
            Description = description;
            IsScreenSpace = isScreenSpace;
            PreviewImage = previewImage;
            Material = material;
        }

        public string ReferenceName { get; private set; } = string.Empty;

        public string Name { get; private set; } = "";
        public string Author { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;

        public bool IsScreenSpace { get; private set; } = true;
        public Texture2D PreviewImage { get; private set; }
        public Material Material { get; private set; }
    }
}
