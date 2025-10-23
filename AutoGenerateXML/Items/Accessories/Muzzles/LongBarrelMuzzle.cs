using AutoGenerateXML.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AutoGenerateXML.ItemXMLExtensions;

namespace AutoGenerateXML.Items.Accessories
{
    public class LongBarrelMuzzle : MuzzleXMLGenerator
    {
        public LongBarrelMuzzle() : base()
        {
            Name = nameof(LongBarrelMuzzle);
            Identifier = Identifiers.VGM_LongBarrelMuzzle;

            BarrelLength = 64;
            WeaponDamageMultiplier = 1.2f;
            PenetrationModifier = 0.1f;
            FlashScaleMultiplier = [1.25f, 1.25f];
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
        <RequiredItem identifier=""aluminium"" amount=""2"" />
        <RequiredItem identifier=""steel"" amount=""2"" />
    </Fabricate>
    {GenerateItemSpriteXMLString(
        texture: "%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/GunMods/GunMods.png",
        sourceRect: [256, 128, 64, 9],
        depth: 0.551f,
        origin: [0.5f, 0.5f]
    )}
    {GenerateRectangleBodyXMLString(width: 60, height: 8, density: 15)}
    {GenerateAccessoryThrowableXMLsString()}
</Item>";

            return xmlString;
        }
    }
}