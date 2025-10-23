
using System.Collections.Immutable;
using System.Text;

namespace AutoGenerateXML
{
    public abstract class StockXMLGenerator : ItemXMLGenerator
    {
        public static Dictionary<string, StockXMLGenerator> All = new();

        public StockXMLGenerator()
        {
            OutputPath = Path.Combine("GunMods", "Stocks.xml");
            SelfTags.AddRange(["smallitem", "tool", Tags.VGM_Accessory, Tags.VGM_Stock]);
            Category = "Equipment";
        }

        public float? RecoilReduction = null;
        public static float CameraShakePerUnitRecoil = 0.2f;
        public float? StocklessBasedSpeedMultiplier = null;

        static StockXMLGenerator() { }
    }
}
