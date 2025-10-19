
using System.Collections.Immutable;
using System.Text;

namespace AutoGenerateXML
{
    public class AccessoryStock
    {
        public static ImmutableDictionary<string, AccessoryStock> Stats;

        public float? RecoilReduction = null;
        public static float CameraShakePerUnitRecoil = 0.2f;
        public float? StocklessBasedSpeedMultiplier = null;

        static AccessoryStock()
        {
            Dictionary<string, AccessoryStock> stats = new();

            //RecoilReduction:
            //  RifleStock x1.3-> LightStock x1.5-> LightSniperStock
            //  HMGStock x1.3-> HeavyStock x1.5-> HeavySniperStock
            //StocklessBasedSpeedMultiplier:
            //  RifleStock -0.02> LightStock -0.05-> LightSniperStock
            //  HMGStock -0.02-> HeavyStock -0.05-> HeavySniperStock
            stats[Identifiers.VGM_RifleStock] = new()
            {
                RecoilReduction = 50,
                StocklessBasedSpeedMultiplier = 0.91f,
            };

            stats[Identifiers.VGM_SMGUniqueStock] = new()
            {
                RecoilReduction = 45,
                StocklessBasedSpeedMultiplier = 0.9f,
            };

            stats[Identifiers.VGM_AssaultRifleStock] = new()
            {
                RecoilReduction = 80,
            };

            stats[Identifiers.VGM_HMGStock] = new()
            {
                RecoilReduction = 130,
                StocklessBasedSpeedMultiplier = 0.83f,
            };

            stats[Identifiers.VGM_ShotgunStock] = new()
            {
                RecoilReduction = 50,
                StocklessBasedSpeedMultiplier = 0.9f,
            };

            stats[Identifiers.VGM_ShotgunUniqueStock] = new()
            {
                RecoilReduction = 50,
                StocklessBasedSpeedMultiplier = 0.9f,
            };

            stats[Identifiers.VGM_LightStock] = new()
            {
                RecoilReduction = 65,
                StocklessBasedSpeedMultiplier = 0.9f,
            };

            stats[Identifiers.VGM_LightSniperStock] = new()
            {
                RecoilReduction = 97.5f,
                StocklessBasedSpeedMultiplier = 0.85f,
            };

            stats[Identifiers.VGM_LightWrenchStock] = new()
            {
                RecoilReduction = 30.0f,
            };

            stats[Identifiers.VGM_HeavyStock] = new()
            {
                RecoilReduction = 169,
                StocklessBasedSpeedMultiplier = 0.81f,
            };

            stats[Identifiers.VGM_HeavySniperStock] = new()
            {
                RecoilReduction = 253.5f,
                StocklessBasedSpeedMultiplier = 0.76f,
            };

            Stats = stats.ToImmutableDictionary();
        }
    }
}
