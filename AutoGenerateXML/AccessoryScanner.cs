
using System.Collections.Immutable;
using System.Text;

namespace AutoGenerateXML
{
    public class AccessoryScanner
    {
        public static ImmutableDictionary<string, AccessoryScanner> Stats;

        public float Range;
        public bool ThermalGoggles;
        public bool ShowDeadCharacters;
        public bool ShowTexts;
        public float[] OverlayColor = [0, 0, 0, 0];

        static AccessoryScanner()
        {
            Dictionary<string, AccessoryScanner> stats = new();

            stats[Identifiers.VGM_HealthScanner] = new()
            {
                Range = 500,
                ThermalGoggles = false,
                ShowDeadCharacters = true,
                ShowTexts = true,
                OverlayColor = [72, 119, 72, 120],
            };

            stats[Identifiers.VGM_ThermalScanner] = new()
            {
                Range = 3000,
                ThermalGoggles = true,
                ShowDeadCharacters = false,
                ShowTexts = false,
                OverlayColor = [176, 0, 0, 120],
            };

            Stats = stats.ToImmutableDictionary();
        }
    }
}
