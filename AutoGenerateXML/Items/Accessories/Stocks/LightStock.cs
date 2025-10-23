using AutoGenerateXML.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AutoGenerateXML.ItemXMLExtensions;

namespace AutoGenerateXML.Items.Accessories
{
    public class LightStock : StockXMLGenerator
    {
        public LightStock() : base()
        {
            Name = nameof(LightStock);
            Identifier = Identifiers.VGM_LightStock;

            RecoilReduction = 65;
            StocklessBasedSpeedMultiplier = 0.9f;
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
        sourceRect: [128, 0, 61, 30],
        depth: 0.554f,
        origin: [0.5f, 0.5f]
    )}
    {GenerateRectangleBodyXMLString(width: 57, height: 28, density: 15)}
    {GenerateAccessoryThrowableXMLsString()}
</Item>";

            return xmlString;
        }
    }
}