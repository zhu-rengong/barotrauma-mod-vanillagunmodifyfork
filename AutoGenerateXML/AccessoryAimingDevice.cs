
using System.Collections.Immutable;
using System.Text;

namespace AutoGenerateXML
{
    public class AccessoryAimingDevice
    {
        public static ImmutableDictionary<string, AccessoryAimingDevice> Stats;

        public float? SpreadRecoveryMultiplier;
        public float? MinimumSpreadOnRecoveringMultiplier;
        public float? SpreadChangesOnAimDownSightMultiplier;
        public float? ObstructVisionAmount;

        static AccessoryAimingDevice()
        {
            Dictionary<string, AccessoryAimingDevice> stats = new();

            stats[Identifiers.VGM_RedDotSight] = new()
            {
                SpreadRecoveryMultiplier = 1.05f,
                SpreadChangesOnAimDownSightMultiplier = 1.35f,
                ObstructVisionAmount = 0.2f,
            };

            stats[Identifiers.VGM_HolographicSight] = new()
            {
                SpreadRecoveryMultiplier = 1.05f,
                SpreadChangesOnAimDownSightMultiplier = 1.3f,
                ObstructVisionAmount = 0.3f,
            };

            stats[Identifiers.VGM_ACOGScope] = new()
            {
                SpreadRecoveryMultiplier = 0.9f,
                MinimumSpreadOnRecoveringMultiplier = 0.25f,
                SpreadChangesOnAimDownSightMultiplier = 1.7f,
                ObstructVisionAmount = 0.8f,
            };
            
            stats[Identifiers.VGM_RifleScope] = new()
            {
                SpreadRecoveryMultiplier = 0.85f,
                MinimumSpreadOnRecoveringMultiplier = 0.1875f,
                SpreadChangesOnAimDownSightMultiplier = 1.8f,
                ObstructVisionAmount = 0.8f,
            };

            stats[Identifiers.VGM_SniperScope] = new()
            {
                SpreadRecoveryMultiplier = 0.8f,
                MinimumSpreadOnRecoveringMultiplier = 0.125f,
                SpreadChangesOnAimDownSightMultiplier = 1.9f,
                ObstructVisionAmount = 0.8f,
            };

            Stats = stats.ToImmutableDictionary();
        }
    }
}
