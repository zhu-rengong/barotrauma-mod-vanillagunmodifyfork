
using System.Collections.Immutable;
using System.Text;

namespace AutoGenerateXML
{
    public class AccessoryGrip
    {
        public static ImmutableDictionary<string, AccessoryGrip> Stats;

        public float? SpreadRecoveryMultiplier;
        public float? SpreadChangesOnAimDownSightMultiplier;

        static AccessoryGrip()
        {
            Dictionary<string, AccessoryGrip> stats = new();

            stats[Identifiers.VGM_AngledForeGrip] = new()
            {
                SpreadRecoveryMultiplier = 1.2f,
                SpreadChangesOnAimDownSightMultiplier = 0.3f
            };

            stats[Identifiers.VGM_VerticalGrip] = new()
            {
                SpreadRecoveryMultiplier = 1.45f,
                SpreadChangesOnAimDownSightMultiplier = 0.9f
            };

            Stats = stats.ToImmutableDictionary();
        }
    }
}
