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
            };

            stats[Identifiers.VGM_ShortSuppressorMuzzle] = new()
            {
                BarrelLength = 33,
                WeaponDamageMultiplier = 1.1f,
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
            };

            stats[Identifiers.VGM_FlashHiderMuzzle] = new()
            {
                BarrelLength = 16,
                SpreadChangesOnShootMultiplier = 0.8f,
            };

            Stats = stats.ToImmutableDictionary();
        }
    }
}
