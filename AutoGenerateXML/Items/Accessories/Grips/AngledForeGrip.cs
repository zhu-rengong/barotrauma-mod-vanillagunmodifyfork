using AutoGenerateXML.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AutoGenerateXML.ItemXMLExtensions;

namespace AutoGenerateXML.Items.Accessories
{
    public class AngledForeGrip : GripXMLGenerator
    {
        public AngledForeGrip() : base()
        {
            Name = nameof(AngledForeGrip);
            Identifier = Identifiers.VGM_AngledForeGrip;

            SpreadRecoveryMultiplier = 1.2f;
            SpreadChangesOnAimDownSightMultiplier = 0.3f;
            HoldAngle = 20;
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
    </Fabricate>
    {GenerateItemSpriteXMLString(
        texture: "%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/GunMods/GunMods.png",
        sourceRect: [256, 256, 57, 24],
        depth: 0.553f,
        origin: [0.5f, 0.5f]
    )}
    {GenerateRectangleBodyXMLString(width: 53, height: 22, density: 15)}
    {GenerateAccessoryThrowableXMLsString()}
</Item>";

            return xmlString;
        }
    }
}