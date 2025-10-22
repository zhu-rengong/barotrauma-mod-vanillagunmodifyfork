
using System.Collections.Immutable;
using System.Text;

namespace AutoGenerateXML
{
    public class AccessoryGrip
    {
        public static ImmutableDictionary<string, AccessoryGrip> Stats;

        public float? SpreadRecoveryMultiplier;
        public float? MinimumSpreadOnRecoveringMultiplier;
        public float? SpreadChangesOnAimDownSightMultiplier;
        public float? HoldAngle;

        static AccessoryGrip()
        {
            Dictionary<string, AccessoryGrip> stats = new();

            stats[Identifiers.VGM_AngledForeGrip] = new()
            {
                SpreadRecoveryMultiplier = 1.2f,
                SpreadChangesOnAimDownSightMultiplier = 0.3f,
                HoldAngle = 20,
            };

            stats[Identifiers.VGM_VerticalGrip] = new()
            {
                SpreadRecoveryMultiplier = 1.45f,
                SpreadChangesOnAimDownSightMultiplier = 0.9f,
                HoldAngle = -15,
            };
            
            stats[Identifiers.VGM_BipodGrip] = new()
            {
                SpreadRecoveryMultiplier = 1.7f,
                SpreadChangesOnAimDownSightMultiplier = 2.5f,
                MinimumSpreadOnRecoveringMultiplier = 0.5f,
                HoldAngle = -50,
            };

            Stats = stats.ToImmutableDictionary();
        }
    }
}
