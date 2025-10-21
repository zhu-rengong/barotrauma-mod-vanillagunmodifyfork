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
        public string? FlashOverrideParticle;
        public float[]? FlashScaleMultiplier;
        public float? FlashAlphaMultiplier;

        // unit: pixel
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
            };

            stats[Identifiers.VGM_ShortSuppressorMuzzle] = new()
            {
                BarrelLength = 33,
                WeaponDamageMultiplier = 1.1f,
                FlashScaleMultiplier = [1.1f, 1.1f],
            };

            stats[Identifiers.VGM_SimpleSuppressorMuzzle] = new()
            {
                BarrelLength = 64,
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
            };

            Stats = stats.ToImmutableDictionary();
        }
    }
}
