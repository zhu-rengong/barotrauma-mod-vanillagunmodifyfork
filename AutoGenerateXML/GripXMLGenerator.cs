
using AutoGenerateXML.Items.Weapons;
using System.Collections.Immutable;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace AutoGenerateXML
{
    public abstract class GripXMLGenerator : ItemXMLGenerator
    {
        public static Dictionary<string, GripXMLGenerator> All = new();

        public GripXMLGenerator()
        {
            OutputPath = Path.Combine("GunMods", "Grips.xml");
            SelfTags.AddRange(["smallitem", "tool", Tags.VGM_Accessory, Tags.VGM_Grip]);
            Category = "Equipment";
        }

        public float? SpreadRecoveryMultiplier;
        public float? MinimumSpreadOnRecoveringMultiplier;
        public float? SpreadChangesOnAimDownSightMultiplier;
        public float? HoldAngle;

        static GripXMLGenerator() { }
    }
}
