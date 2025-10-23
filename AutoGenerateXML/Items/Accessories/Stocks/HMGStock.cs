using AutoGenerateXML.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AutoGenerateXML.ItemXMLExtensions;

namespace AutoGenerateXML.Items.Accessories
{
    public class HMGStock : StockXMLGenerator
    {
        public HMGStock() : base()
        {
            Name = nameof(HMGStock);
            Identifier = Identifiers.VGM_HMGStock;

            RecoilReduction = 130;
            StocklessBasedSpeedMultiplier = 0.83f;
        }

        public override string Generate()
        {
            string xmlString =
$@"<Item name=""{Name.FollowedByModPrefix()}"" identifier=""{Identifier}"" category=""{Category}"" subcategory=""{SubCategory}""
    tags=""{ConcatValues(SelfTags)}""
    cargocontaineridentifier=""metalcrate"" scale=""{Scale}"" impactsoundtag=""impact_metal_light"">
    {GenerateAccessoryPreferredContainerXMLsString()}
    {GenerateItemSpriteXMLString(
        texture: "%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/GunMods/GunMods.png",
        sourceRect: [0, 320, 85, 51],
        depth: 0.554f,
        origin: [0.5f, 0.5f]
    )}
    {GenerateRectangleBodyXMLString(width: 81, height: 47, density: 15)}
    {GenerateAccessoryThrowableXMLsString()}
</Item>";

            return xmlString;
        }
    }
}