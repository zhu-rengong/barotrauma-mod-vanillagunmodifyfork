
using System.Collections.Immutable;
using System.Text;

namespace AutoGenerateXML
{
    public abstract class AimingDeviceXMLGenerator : ItemXMLGenerator
    {
        public static Dictionary<string, AimingDeviceXMLGenerator> All = new();

        public AimingDeviceXMLGenerator()
        {
            OutputPath = Path.Combine("GunMods", "AimingDevices.xml");
            SelfTags.AddRange(["smallitem", "tool", Tags.VGM_Accessory, Tags.VGM_AimingDevice]);
            Category = "Equipment";
        }

        public float? SpreadRecoveryMultiplier;
        public float? MinimumSpreadOnRecoveringMultiplier;
        public float? SpreadChangesOnAimDownSightMultiplier;
        public float? ObstructVisionAmount;
        public float? CameraAimOffset;

        static AimingDeviceXMLGenerator() { }
    }
}
