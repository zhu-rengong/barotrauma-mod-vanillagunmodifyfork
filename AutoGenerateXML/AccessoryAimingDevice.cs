
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
        public float? CameraAimOffset;

        static AccessoryAimingDevice()
        {
            Dictionary<string, AccessoryAimingDevice> stats = new();

            stats[Identifiers.VGM_RedDotSight] = new()
            {
                SpreadRecoveryMultiplier = 1.05f,
                SpreadChangesOnAimDownSightMultiplier = 1.35f,
                ObstructVisionAmount = 0.2f,
                CameraAimOffset = 350,
            };

            stats[Identifiers.VGM_HolographicSight] = new()
            {
                SpreadRecoveryMultiplier = 1.05f,
                SpreadChangesOnAimDownSightMultiplier = 1.3f,
                ObstructVisionAmount = 0.3f,
                CameraAimOffset = 350,
            };

            stats[Identifiers.VGM_ACOGScope] = new()
            {
                SpreadRecoveryMultiplier = 0.9f,
                MinimumSpreadOnRecoveringMultiplier = 0.25f,
                SpreadChangesOnAimDownSightMultiplier = 1.7f,
                ObstructVisionAmount = 0.8f,
                CameraAimOffset = 450,
            };

            stats[Identifiers.VGM_RifleScope] = new()
            {
                SpreadRecoveryMultiplier = 0.85f,
                MinimumSpreadOnRecoveringMultiplier = 0.1875f,
                SpreadChangesOnAimDownSightMultiplier = 1.8f,
                ObstructVisionAmount = 0.8f,
                CameraAimOffset = 625,
            };

            stats[Identifiers.VGM_SniperScope] = new()
            {
                SpreadRecoveryMultiplier = 0.8f,
                MinimumSpreadOnRecoveringMultiplier = 0.125f,
                SpreadChangesOnAimDownSightMultiplier = 1.9f,
                ObstructVisionAmount = 0.8f,
                CameraAimOffset = 800,
            };

            Stats = stats.ToImmutableDictionary();
        }
    }
}
