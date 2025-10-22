using System.Collections.Immutable;
using System.Text;

namespace AutoGenerateXML
{
    public class AccessoryMuzzle
    {
        public static ImmutableDictionary<string, AccessoryMuzzle> Stats;

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
        public float Scale = 0.5f;

        static AccessoryMuzzle()
        {
            Dictionary<string, AccessoryMuzzle> stats = new();

            stats[Identifiers.VGM_LongSuppressorMuzzle] = new()
            {
                BarrelLength = 60,
                WeaponDamageMultiplier = 1.2f,
                FlashScaleMultiplier = [1.15f, 1.15f],
                IsSuppressor = true,
            };

            stats[Identifiers.VGM_ShortSuppressorMuzzle] = new()
            {
                BarrelLength = 33,
                WeaponDamageMultiplier = 1.1f,
                FlashScaleMultiplier = [1.1f, 1.1f],
                IsSuppressor = true,
            };

            stats[Identifiers.VGM_SimpleSuppressorMuzzle] = new()
            {
                BarrelLength = 64,
                IsSuppressor = true,
            };

            stats[Identifiers.VGM_LongBarrelMuzzle] = new()
            {
                BarrelLength = 64,
                WeaponDamageMultiplier = 1.2f,
                PenetrationModifier = 0.1f,
                FlashScaleMultiplier = [1.25f, 1.25f],
            };

            stats[Identifiers.VGM_FlashHiderMuzzle] = new()
            {
                BarrelLength = 16,
                SpreadChangesOnShootMultiplier = 0.8f,
                FlashAlphaMultiplier = 0.8f,
                FlashScaleMultiplier = [0.5f, 0.5f],
                IsFlashHider = true,
            };
            
            stats[Identifiers.VGM_ChokeTubeMuzzle] = new()
            {
                BarrelLength = 20,
                BarrelEmbeddedDepth = 8,
                SpreadChoke = 8,
                SpreadChokeLimit = 2,
            };

            Stats = stats.ToImmutableDictionary();
        }
    }
}
