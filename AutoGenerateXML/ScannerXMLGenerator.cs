
using System.Collections.Immutable;
using System.Text;

namespace AutoGenerateXML
{
    public abstract class ScannerXMLGenerator : ItemXMLGenerator
    {
        public static Dictionary<string, ScannerXMLGenerator> All = new();

        public ScannerXMLGenerator()
        {
            OutputPath = Path.Combine("GunMods", "Scanners.xml");
            SelfTags.AddRange(["smallitem", "tool", Tags.VGM_Accessory, Tags.VGM_Scanner]);
            Category = "Equipment";
        }

        public float Range;
        public bool ThermalGoggles;
        public bool ShowDeadCharacters;
        public bool ShowTexts;
        public float[] OverlayColor = [0, 0, 0, 0];

        static ScannerXMLGenerator() { }
    }
}
