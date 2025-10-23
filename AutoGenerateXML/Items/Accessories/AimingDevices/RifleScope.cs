using AutoGenerateXML.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AutoGenerateXML.ItemXMLExtensions;

namespace AutoGenerateXML.Items.Accessories
{
    public class RifleScope : AimingDeviceXMLGenerator
    {
        public RifleScope() : base()
        {
            Name = nameof(RifleScope);
            Identifier = Identifiers.VGM_RifleScope;

            SpreadRecoveryMultiplier = 0.85f;
            MinimumSpreadOnRecoveringMultiplier = 0.1875f;
            SpreadChangesOnAimDownSightMultiplier = 1.8f;
            ObstructVisionAmount = 0.8f;
            CameraAimOffset = 625;
        }

        public override string Generate()
        {
            string xmlString =
$@"<Item name=""{Name.FollowedByModPrefix()}"" identifier=""{Identifier}"" category=""{Category}"" subcategory=""{SubCategory}""
    tags=""{ConcatValues(SelfTags)}""
    cargocontaineridentifier=""metalcrate"" scale=""{Scale}"" impactsoundtag=""impact_metal_light"">
    {GenerateAccessoryPreferredContainerXMLsString()}
    <Fabricate suitablefabricators=""VGM_Fabricator"" requiredtime=""30"">
        <RequiredSkill identifier=""weapons"" level=""40"" />
        <RequiredItem identifier=""iron"" />
        <RequiredItem identifier=""plastic"" />
        <RequiredItem identifier=""quartz"" />
    </Fabricate>
    {GenerateItemSpriteXMLString(
        texture: "%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/GunMods/GunMods.png",
        sourceRect: [384, 128, 66, 20],
        depth: 0.555f,
        origin: [0.5f, 0.5f]
    )}
    {GenerateRectangleBodyXMLString(width: 62, height: 18, density: 15)}
    {GenerateAccessoryThrowableXMLsString()}
</Item>";

            return xmlString;
        }
    }
}