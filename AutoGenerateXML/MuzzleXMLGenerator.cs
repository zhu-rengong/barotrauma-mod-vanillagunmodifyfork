using System.Collections.Immutable;
using System.Text;

namespace AutoGenerateXML
{
    public abstract class MuzzleXMLGenerator : ItemXMLGenerator
    {
        public static Dictionary<string, MuzzleXMLGenerator> All = new();

        public MuzzleXMLGenerator()
        {
            OutputPath = Path.Combine("GunMods", "Muzzles.xml");
            SelfTags.AddRange(["smallitem", "tool", Tags.VGM_Accessory, Tags.VGM_Muzzle]);
            Category = "Equipment";
        }

        public float? WeaponDamageMultiplier;
        public float? PenetrationModifier;
        public float? SpreadChangesOnShootMultiplier;
        public float? SpreadChoke;
        public float SpreadChokeLimit = 0.0f;
        public string? FlashOverrideParticle;
        public float[]? FlashScaleMultiplier;
        public float? FlashAlphaMultiplier;
        public bool IsFlashHider = false;
        public bool IsSuppressor = false;

        // unit: pixel
        public float BarrelEmbeddedDepth = 0.0f;
        public float BarrelLength = 0.0f;

        static MuzzleXMLGenerator() { }
    }
}
