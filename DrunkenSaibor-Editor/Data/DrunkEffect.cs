using UnityEngine;

namespace DrunkenSaibor.Data
{
    public class DrunkEffect : MonoBehaviour
    {
        public string referenceName = "";

        new public string name = "";
        public string author = "";
        public string description = "";

        public bool isScreenSpace = true;
        public Texture2D previewImage;

        public Material material;
    }
}