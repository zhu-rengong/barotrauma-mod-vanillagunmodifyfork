using AutoGenerateXML.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AutoGenerateXML.ItemXMLExtensions;

namespace AutoGenerateXML.Items.Accessories
{
    public class LongSuppressorMuzzle : MuzzleXMLGenerator
    {
        public LongSuppressorMuzzle() : base()
        {
            Name = nameof(LongSuppressorMuzzle);
            Identifier = Identifiers.VGM_LongSuppressorMuzzle;

            BarrelLength = 60;
            WeaponDamageMultiplier = 1.2f;
            FlashScaleMultiplier = [1.15f, 1.15f];
            IsSuppressor = true;
        }

        public override string Generate()
        {
            string xmlString =
$@"<Item name=""{Name.FollowedByModPrefix()}"" identifier=""{Identifier}"" category=""{Category}"" subcategory=""{SubCategory}""
    tags=""{ConcatValues(SelfTags)}""
    cargocontaineridentifier=""metalcrate"" scale=""{Scale}"" impactsoundtag=""impact_metal_light"">
    {GenerateAccessoryPreferredContainerXMLsString()}
    <Fabricate suitablefabricators=""VGM_Fabricator"" requiredtime=""30"">
        <RequiredSkill identifier=""weapons"" level=""70"" />
        <RequiredItem identifier=""titaniumaluminiumalloy"" />
        <RequiredItem identifier=""aluminium"" amount=""2"" />
        <RequiredItem identifier=""fulgurium"" />
    </Fabricate>
    {GenerateItemSpriteXMLString(
        texture: "%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/GunMods/GunMods.png",
        sourceRect: [256, 0, 60, 12],
        depth: 0.551f,
        origin: [0.5f, 0.5f]
    )}
    {GenerateRectangleBodyXMLString(width: 56, height: 11, density: 15)}
    {GenerateAccessoryThrowableXMLsString()}
</Item>";

            return xmlString;
        }
    }
}