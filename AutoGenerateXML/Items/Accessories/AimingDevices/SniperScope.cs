using AutoGenerateXML.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AutoGenerateXML.ItemXMLExtensions;

namespace AutoGenerateXML.Items.Accessories
{
    public class SniperScope : AimingDeviceXMLGenerator
    {
        public SniperScope() : base()
        {
            Name = nameof(SniperScope);
            Identifier = Identifiers.VGM_SniperScope;

            Scale = 0.66f;

            SpreadRecoveryMultiplier = 0.8f;
            MinimumSpreadOnRecoveringMultiplier = 0.125f;
            SpreadChangesOnAimDownSightMultiplier = 1.9f;
            ObstructVisionAmount = 0.8f;
            CameraAimOffset = 800;
        }

        public override string Generate()
        {
            string xmlString =
$@"<Item name=""{Name.FollowedByModPrefix()}"" identifier=""{Identifier}"" category=""{Category}"" subcategory=""{SubCategory}""
    tags=""{ConcatValues(SelfTags)}""
    cargocontaineridentifier=""metalcrate"" scale=""{Scale}"" impactsoundtag=""impact_metal_light"">
    {GenerateAccessoryPreferredContainerXMLsString()}
    <Fabricate suitablefabricators=""VGM_Fabricator"" requiredtime=""30"">
        <RequiredSkill identifier=""weapons"" level=""50"" />
        <RequiredItem identifier=""aluminium"" />
        <RequiredItem identifier=""plastic"" />
        <RequiredItem identifier=""quartz"" amount=""2"" />
    </Fabricate>
    {GenerateItemSpriteXMLString(
        texture: "%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/GunMods/GunMods.png",
        sourceRect: [384, 96, 71, 19],
        depth: 0.555f,
        origin: [0.5f, 0.5f]
    )}
    {GenerateRectangleBodyXMLString(width: 67, height: 17, density: 15)}
    {GenerateAccessoryThrowableXMLsString()}
</Item>";

            return xmlString;
        }
    }
}