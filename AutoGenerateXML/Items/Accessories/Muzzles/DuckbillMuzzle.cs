using AutoGenerateXML.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AutoGenerateXML.ItemXMLExtensions;

namespace AutoGenerateXML.Items.Accessories
{
    public class DuckbillMuzzle : MuzzleXMLGenerator
    {
        public DuckbillMuzzle() : base()
        {
            Name = nameof(DuckbillMuzzle);
            Identifier = Identifiers.VGM_DuckbillMuzzle;

            BarrelLength = 28;
            BarrelEmbeddedDepth = 3;
            SpreadChoke = -15;
            ShotAmountModifierPerXShots = [1, 4];
            ShotAmountModifiationTimes = 3;
        }

        public override string Generate()
        {
            string xmlString =
$@"<Item name=""{Name.FollowedByModPrefix()}"" identifier=""{Identifier}"" category=""{Category}"" subcategory=""{SubCategory}""
    tags=""{ConcatValues(SelfTags)}""
    cargocontaineridentifier=""metalcrate"" scale=""{Scale}"" impactsoundtag=""impact_metal_light"">
    {GenerateAccessoryPreferredContainerXMLsString()}
    <Fabricate suitablefabricators=""VGM_Fabricator"" requiredtime=""30"">
        <RequiredSkill identifier=""weapons"" level=""60"" />
        <RequiredItem identifier=""aluminium"" />
        <RequiredItem identifier=""steel"" amount=""2"" />
    </Fabricate>
    {GenerateItemSpriteXMLString(
        texture: "%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/GunMods/GunMods.png",
        sourceRect: [256, 384, 28, 11],
        depth: 0.518f,
        origin: [0.5f, 0.5f]
    )}
    {GenerateRectangleBodyXMLString(width: 26, height: 10, density: 15)}
    {GenerateAccessoryThrowableXMLsString()}
</Item>";

            return xmlString;
        }
    }
}