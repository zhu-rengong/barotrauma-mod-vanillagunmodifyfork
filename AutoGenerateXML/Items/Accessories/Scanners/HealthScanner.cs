using AutoGenerateXML.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AutoGenerateXML.ItemXMLExtensions;

namespace AutoGenerateXML.Items.Accessories
{
    public class HealthScanner : ScannerXMLGenerator
    {
        public HealthScanner() : base()
        {
            Name = nameof(HealthScanner);
            Identifier = Identifiers.VGM_HealthScanner;

            Range = 500;
            ThermalGoggles = false;
            ShowDeadCharacters = true;
            ShowTexts = true;
            OverlayColor = [72, 119, 72, 120];
        }

        public override string Generate()
        {
            string xmlString =
$@"<Item name=""{Name.FollowedByModPrefix()}"" identifier=""{Identifier}"" category=""{Category}"" subcategory=""{SubCategory}""
    tags=""{ConcatValues(SelfTags)}""
    cargocontaineridentifier=""metalcrate"" scale=""{Scale}"" impactsoundtag=""impact_metal_light"">
    {GenerateAccessoryPreferredContainerXMLsString()}
    <Deconstruct time=""20"">
        <Item identifier=""healthscanner"" />
    </Deconstruct>
    <Fabricate suitablefabricators=""VGM_Fabricator"" requiredtime=""10"">
        <RequiredSkill identifier=""mechanical"" level=""30"" />
        <RequiredSkill identifier=""medical"" level=""50"" />
        <RequiredItem identifier=""healthscanner"" />
        <RequiredItem identifier=""plastic"" />
    </Fabricate>
    {GenerateItemSpriteXMLString(
        texture: "%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/GunMods/GunMods.png",
        sourceRect: [384, 224, 25, 20],
        depth: 0.559f,
        origin: [0.5f, 0.5f]
    )}
    {GenerateRectangleBodyXMLString(width: 23, height: 18, density: 15)}
    {GenerateAccessoryThrowableXMLsString()}
</Item>";

            return xmlString;
        }
    }
}