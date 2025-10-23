using AutoGenerateXML.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AutoGenerateXML.ItemXMLExtensions;

namespace AutoGenerateXML.Items.Accessories
{
    public class AssaultRifleStock : StockXMLGenerator
    {
        public AssaultRifleStock() : base()
        {
            Name = nameof(AssaultRifleStock);
            Identifier = Identifiers.VGM_AssaultRifleStock;

            RecoilReduction = 80;
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
        sourceRect: [0, 256, 79, 46],
        depth: 0.554f,
        origin: [0.5f, 0.5f]
    )}
    {GenerateRectangleBodyXMLString(width: 75, height: 42, density: 15)}
    {GenerateAccessoryThrowableXMLsString()}
</Item>";

            return xmlString;
        }
    }
}