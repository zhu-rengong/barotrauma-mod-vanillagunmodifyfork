using AutoGenerateXML.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AutoGenerateXML.ItemXMLExtensions;

namespace AutoGenerateXML.Items.Accessories
{
    public class ACOGScope : AimingDeviceXMLGenerator
    {
        public ACOGScope() : base()
        {
            Name = nameof(ACOGScope);
            Identifier = Identifiers.VGM_ACOGScope;

            SpreadRecoveryMultiplier = 0.9f;
            MinimumSpreadOnRecoveringMultiplier = 0.25f;
            SpreadChangesOnAimDownSightMultiplier = 1.7f;
            ObstructVisionAmount = 0.8f;
            CameraAimOffset = 450;
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
        <RequiredItem identifier=""quartz"" />
        <RequiredItem identifier=""fpgacircuit"" />
    </Fabricate>
    {GenerateItemSpriteXMLString(
        texture: "%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/GunMods/GunMods.png",
        sourceRect: [384, 64, 41, 26],
        depth: 0.555f,
        origin: [0.5f, 0.5f]
    )}
    {GenerateRectangleBodyXMLString(width: 37, height: 24, density: 15)}
    {GenerateAccessoryThrowableXMLsString()}
</Item>";

            return xmlString;
        }
    }
}