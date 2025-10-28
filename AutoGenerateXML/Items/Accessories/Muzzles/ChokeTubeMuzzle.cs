using AutoGenerateXML.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AutoGenerateXML.GunXMLGenerator;
using static AutoGenerateXML.ItemXMLExtensions;

namespace AutoGenerateXML.Items.Accessories
{
    public class ChokeTubeMuzzle : MuzzleXMLGenerator
    {
        public ChokeTubeMuzzle() : base()
        {
            Name = nameof(ChokeTubeMuzzle);
            Identifier = Identifiers.VGM_ChokeTubeMuzzle;

            BarrelLength = 20;
            BarrelEmbeddedDepth = 7;
            SpreadChoke = 8;
            SpreadChokeLimit = 2;
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
        sourceRect: [384, 256, 20, 8],
        depth: 0.551f,
        origin: [0.5f, 0.5f]
    )}
    {GenerateRectangleBodyXMLString(width: 18, height: 7, density: 15)}
    {GenerateAccessoryThrowableXMLsString()}
</Item>";

            return xmlString;
        }

        public static string GenerateShotgunTubeExtenderFunctionalityAtSlefContainerXMLsString()
        {
            StringBuilder codePart1 = new();
            StringBuilder codePart2 = new();
            StringBuilder codePart3 = new();

            bool hasModifier = false;

            All.Values.ForEach(muzzle =>
            {
                if ((muzzle.SpreadChoke.HasValue && muzzle.SpreadChoke.Value != 0) || muzzle.ShotAmountModifierPerXShots is not null)
                {
                    codePart1.AppendLine(
$@"<StatusEffect type=""OnUse"" target=""Contained"" targetitemcomponent=""Projectile"" statuseffecttags=""WouldBeAppliedBy{muzzle.Name}Effect"" duration=""{GetTickDurationString(1)}"" comparison=""And"">
    <Conditional targetcontainer=""true"" targetitemcomponent=""ItemContainer"" hasstatustag=""WouldApply{muzzle.Name}EffectToAmmo"" />
</StatusEffect>");

                    if (muzzle.SpreadChoke.HasValue && muzzle.SpreadChoke.Value != 0)
                    {
                        bool isTightened = muzzle.SpreadChoke.Value > 0.0f;


                        if (isTightened)
                        {
                            codePart2.AppendLine(
$@"<StatusEffect type=""OnRemoved"" target=""Contained"" targetitemcomponent=""Projectile"" statuseffecttags=""{StatusEffectTags.Choked}"" duration=""{GetTickDurationString(1)}"" comparison=""And"">
    <Conditional targetitemcomponent=""Projectile"" user=""! null"" spread=""gt {muzzle.SpreadChokeLimit}"" />
    <Conditional targetitemcomponent=""Projectile"" hasstatustag=""WouldBeAppliedBy{muzzle.Name}Effect"" />
</StatusEffect>
<StatusEffect type=""OnRemoved"" target=""Contained"" targetitemcomponent=""Projectile"" spread=""{-muzzle.SpreadChoke}"" disabledeltatime=""true"" comparison=""And"">
    <Conditional targetitemcomponent=""Projectile"" hasstatustag=""{StatusEffectTags.Choked}"" />
    <Conditional targetitemcomponent=""Projectile"" hasstatustag=""WouldBeAppliedBy{muzzle.Name}Effect"" />
</StatusEffect>
<StatusEffect type=""OnRemoved"" target=""Contained"" targetitemcomponent=""Projectile"" spread=""{muzzle.SpreadChokeLimit}"" setvalue=""true"" comparison=""And"">
    <Conditional targetitemcomponent=""Projectile"" hasstatustag=""{StatusEffectTags.Choked}"" spread=""lt {muzzle.SpreadChokeLimit}"" />
    <Conditional targetitemcomponent=""Projectile"" hasstatustag=""WouldBeAppliedBy{muzzle.Name}Effect"" />
</StatusEffect>");
                        }
                        else
                        {
                            codePart2.AppendLine(
$@"<StatusEffect type=""OnRemoved"" target=""Contained"" targetitemcomponent=""Projectile"" spread=""{-muzzle.SpreadChoke}"" disabledeltatime=""true"" comparison=""And"">
    <Conditional targetitemcomponent=""Projectile"" user=""! null"" />
    <Conditional targetitemcomponent=""Projectile"" hasstatustag=""WouldBeAppliedBy{muzzle.Name}Effect"" />
</StatusEffect>");
                        }

                    }

                    if (muzzle.ShotAmountModifierPerXShots is not null)
                    {
                        int modifier = muzzle.ShotAmountModifierPerXShots[0];
                        int shotsNeeded = muzzle.ShotAmountModifierPerXShots[1];
                        int remainingModification = muzzle.ShotAmountModifiationTimes;

                        switch (Math.Sign(modifier))
                        {
                            case 1:
                                do
                                {
                                    int threshold = shotsNeeded * remainingModification;
                                    codePart3.AppendLine(
$@"<StatusEffect type=""OnRemoved"" target=""Contained"" targetitemcomponent=""Projectile"" hitscancount=""{modifier}"" disabledeltatime=""true"" comparison=""And"">
    <Conditional targetitemcomponent=""Projectile"" hitscancount=""gte {threshold}"" user=""! null"" />
    <Conditional targetitemcomponent=""Projectile"" hasstatustag=""WouldBeAppliedBy{muzzle.Name}Effect"" />
</StatusEffect>");
                                } while (--remainingModification > 0);
                                break;
                            default:
                                break;
                        }
                    }
                }

                hasModifier = true;
            });

            if (hasModifier)
            {
                return
$@"<!-- [Shotgun Tube Extender] Activate -->
{codePart1}
<Containable items=""shotgunammo"">
    {codePart2}
    {codePart3}
</Containable>";
            }

            return string.Empty;
        }
    }
}

namespace AutoGenerateXML
{
    public abstract partial class GunXMLGenerator
    {
        public int ShotgunTuberExtenderSlotIndex = -1;

        private bool hasCalledGenerateMuzzleSpreadChokeXMLsString = false;
        public string GenerateMuzzleSpreadChokeXMLsString()
        {
            hasCalledGenerateMuzzleSpreadChokeXMLsString = true;

            if (ContainableMuzzles is null) { throw new NullReferenceException($@"Unable to generate muzzle code because '{Name}' has no muzzle defined."); }

            StringBuilder stringBuilder = new();

            bool hasModifier = false;

            ContainableMuzzles.ForEach(containable =>
            {
                if (containable.Muzzle.SpreadChoke.HasValue && containable.Muzzle.SpreadChoke.Value != 0)
                {
                    bool isTightened = containable.Muzzle.SpreadChoke.Value > 0.0f;

                    if (isTightened)
                    {
                        stringBuilder.AppendLine(
$@"<StatusEffect type=""OnRemoved"" target=""Contained"" targetitemcomponent=""Projectile"" statuseffecttags=""{StatusEffectTags.Choked}"" duration=""{GetTickDurationString(1)}"" comparison=""And"">
    <Conditional targetitemcomponent=""Projectile"" user=""! null"" spread=""gt {containable.Muzzle.SpreadChokeLimit}"" />
    <RequiredItem identifier=""{containable.Muzzle.Identifier}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" />
</StatusEffect>
<StatusEffect type=""OnRemoved"" target=""Contained"" targetitemcomponent=""Projectile"" spread=""{-containable.Muzzle.SpreadChoke.Value}"" disabledeltatime=""true"">
    <Conditional targetitemcomponent=""Projectile"" hasstatustag=""{StatusEffectTags.Choked}"" />
    <RequiredItem identifier=""{containable.Muzzle.Identifier}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" />
</StatusEffect>
<StatusEffect type=""OnRemoved"" target=""Contained"" targetitemcomponent=""Projectile"" spread=""{containable.Muzzle.SpreadChokeLimit}"" setvalue=""true"" comparison=""And"">
    <Conditional targetitemcomponent=""Projectile"" hasstatustag=""{StatusEffectTags.Choked}"" spread=""lt {containable.Muzzle.SpreadChokeLimit}"" />
    <RequiredItem identifier=""{containable.Muzzle.Identifier}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" />
</StatusEffect>");
                    }
                    else
                    {
                        stringBuilder.AppendLine(
$@"<StatusEffect type=""OnRemoved"" target=""Contained"" targetitemcomponent=""Projectile"" spread=""{-containable.Muzzle.SpreadChoke.Value}"" disabledeltatime=""true"">
    <Conditional targetitemcomponent=""Projectile"" user=""! null"" />
    <RequiredItem identifier=""{containable.Muzzle.Identifier}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" />
</StatusEffect>");
                    }

                    hasModifier = true;
                }
            });

            if (hasModifier)
            {
                stringBuilder.Insert(0, "<!-- [Muzzle] Spread choke -->\n");
            }

            return stringBuilder.ToString();
        }


        private bool hasCalledGenerateMuzzleShotAmountModificationXMLsString = false;
        public string GenerateMuzzleShotAmountModificationXMLsString()
        {
            hasCalledGenerateMuzzleShotAmountModificationXMLsString = true;

            if (ContainableMuzzles is null) { throw new NullReferenceException($@"Unable to generate muzzle code because '{Name}' has no muzzle defined."); }

            StringBuilder stringBuilder = new();

            bool hasModifier = false;

            ContainableMuzzles.ForEach(containable =>
            {
                if (containable.Muzzle.ShotAmountModifierPerXShots is not null)
                {
                    int modifier = containable.Muzzle.ShotAmountModifierPerXShots[0];
                    int shotsNeeded = containable.Muzzle.ShotAmountModifierPerXShots[1];
                    int remainingModification = containable.Muzzle.ShotAmountModifiationTimes;

                    switch (Math.Sign(modifier))
                    {
                        case 1:
                            do
                            {
                                int threshold = shotsNeeded * remainingModification;
                                stringBuilder.AppendLine(
$@"<StatusEffect type=""OnRemoved"" target=""Contained"" targetitemcomponent=""Projectile"" hitscancount=""{modifier}"" disabledeltatime=""true"" comparison=""And"">
    <Conditional targetitemcomponent=""Projectile"" hitscancount=""gte {threshold}"" user=""! null"" />
    <RequiredItem identifier=""{containable.Muzzle.Identifier}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" />
</StatusEffect>");
                            } while (--remainingModification > 0);

                            hasModifier = true;
                            break;
                        default:
                            break;
                    }

                    hasModifier = true;
                }
            });

            if (hasModifier)
            {
                stringBuilder.Insert(0, "<!-- [Muzzle] Shot amount modification -->\n");
            }

            return stringBuilder.ToString();
        }

        private bool hasCalledGenerateShotgunTubeExtenderFunctionalityAtParentHoldableXMLsString = false;
        public string GenerateShotgunTubeExtenderFunctionalityAtParentHoldableXMLsString()
        {
            hasCalledGenerateShotgunTubeExtenderFunctionalityAtParentHoldableXMLsString = true;

            if (ContainableMuzzles is null) { throw new NullReferenceException($@"Unable to generate muzzle code because '{Name}' has no muzzle defined."); }

            StringBuilder stringBuilder = new();

            bool hasModifier = false;

            ContainableMuzzles.ForEach(containable =>
            {
                if ((containable.Muzzle.SpreadChoke.HasValue && containable.Muzzle.SpreadChoke.Value != 0) || containable.Muzzle.ShotAmountModifierPerXShots is not null)
                {
                    stringBuilder.AppendLine(
$@"<StatusEffect type=""OnUse"" target=""Contained"" targetslot=""{ShotgunTuberExtenderSlotIndex}"" statuseffecttags=""WouldApply{containable.Muzzle.Name}EffectToAmmo"" duration=""{GetTickDurationString(1)}"" comparison=""And"">
    <Conditional targetcontainer=""true"" targetitemcomponent=""RangedWeapon"" isactive=""false"" reloadtimer=""0.0"" />
    <RequiredItem identifier=""{Identifiers.VGM_ShotgunTubeExtender}"" type=""Contained"" targetslot=""{ShotgunTuberExtenderSlotIndex}"" />
    <RequiredItem identifier=""{containable.Muzzle.Identifier}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" />
</StatusEffect>
<StatusEffect type=""OnUse"" target=""Contained"" targetslot=""{ShotgunTuberExtenderSlotIndex}"" comparison=""And"">
    <Use />
    <Conditional targetcontainer=""true"" targetitemcomponent=""RangedWeapon"" isactive=""false"" reloadtimer=""0.0"" />
    <RequiredItem identifier=""{Identifiers.VGM_ShotgunTubeExtender}"" type=""Contained"" targetslot=""{ShotgunTuberExtenderSlotIndex}"" />
    <RequiredItem identifier=""{containable.Muzzle.Identifier}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" />
</StatusEffect>");

                    hasModifier = true;
                }
            });

            if (hasModifier)
            {
                stringBuilder.Insert(0, "<!-- [Shotgun Tube Extender] Activate -->\n");
            }

            return stringBuilder.ToString();
        }
    }
}