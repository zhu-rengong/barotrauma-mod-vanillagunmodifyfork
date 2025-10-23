using AutoGenerateXML.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AutoGenerateXML.ItemXMLExtensions;

namespace AutoGenerateXML.Items.Accessories
{
    public class ThermalScanner : ScannerXMLGenerator
    {
        public ThermalScanner() : base()
        {
            Name = nameof(ThermalScanner);
            Identifier = Identifiers.VGM_ThermalScanner;

            Range = 3000;
            ThermalGoggles = true;
            ShowDeadCharacters = false;
            ShowTexts = false;
            OverlayColor = [176, 0, 0, 120];
        }

        public override string Generate()
        {
            string xmlString =
$@"<Item name=""{Name.FollowedByModPrefix()}"" identifier=""{Identifier}"" category=""{Category}"" subcategory=""{SubCategory}""
    tags=""{ConcatValues(SelfTags)}""
    cargocontaineridentifier=""metalcrate"" scale=""{Scale}"" impactsoundtag=""impact_metal_light"">
    {GenerateAccessoryPreferredContainerXMLsString()}
    <Deconstruct time=""20"">
        <Item identifier=""thermalgoggles"" />
    </Deconstruct>
    <Fabricate suitablefabricators=""VGM_Fabricator"" requiredtime=""10"">
        <RequiredSkill identifier=""mechanical"" level=""30"" />
        <RequiredSkill identifier=""medical"" level=""50"" />
        <RequiredItem identifier=""thermalgoggles"" />
        <RequiredItem identifier=""plastic"" />
    </Fabricate>
    {GenerateItemSpriteXMLString(
        texture: "%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/GunMods/GunMods.png",
        sourceRect: [384, 192, 24, 20],
        depth: 0.559f,
        origin: [0.5f, 0.5f]
    )}
    {GenerateRectangleBodyXMLString(width: 22, height: 18, density: 15)}
    {GenerateAccessoryThrowableXMLsString()}
</Item>";

            return xmlString;
        }
    }
}