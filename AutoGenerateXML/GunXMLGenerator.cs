

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using static AutoGenerateXML.ItemXMLExtensions;

namespace AutoGenerateXML
{
    public abstract class GunXMLGenerator : ItemXMLGenerator
    {
        public abstract string GunName { get; }

        public abstract string Identifier { get; }
        public abstract string SelfTags { get; }
        public virtual string Category => "Weapon";
        public virtual string SubCategory => UserDefinedGlobal.ModName;

        public virtual float Scale { get; } = 0.5f;

        public virtual int StockSlotIndex { get; } = -1;
        public virtual int MuzzleSlotIndex { get; } = -1;
        public virtual int ScannerSlotIndex { get; } = -1;
        public virtual int UpperAccessorySlotIndex { get; } = -1;
        public virtual int LowerAccessorySlotIndex { get; } = -1;

        public virtual float? HoldAngle { get; } = null;

        public abstract float[] BarrelPos { get; }
        public abstract float WeaponDamageModifier { get; }
        public abstract float Penetration { get; }
        public abstract float RequiredWeaponsSkill { get; }

        public abstract float Reload { get; }
        public virtual float? ReloadSkillRequirement { get; } = null;
        public virtual float? ReloadNoSkill { get; } = null;
        public abstract float CombatPriority { get; }
        public abstract float MinimumSpread { get; }
        public abstract float MinimumUnskilledSpread { get; }
        public abstract float SpreadChangesOnAimDownSight { get; }
        public abstract float SpreadRecovery { get; }
        public abstract float SpreadChangesOnShoot { get; }
        public abstract float SpreadLimit { get; }
        public abstract float Recoil { get; }
        public virtual float StockRecoilReductionEfficiency { get; } = 1.0f;
        public abstract float StocklessSpeedMultiplier { get; }
        public virtual ContainableGrip[]? CompatibleGrips { get; } = null;
        public virtual ContainableStock[]? CompatibleStocks { get; } = null;
        public virtual ContainableMuzzle[]? CompatibleMuzzles { get; } = null;
        public virtual ContainableAimingDevice[]? CompatibleAimingDevices { get; } = null;

        public virtual float NormalSoundRangeOnShoot => 3000;
        public virtual float SuppressedSoundRangeOnShoot => 800;
        public virtual float FlashHiddenSightRangeOnShoot => 500;

        public virtual float CrosshairScale => 0.15f;

        public static readonly string[] AllMuzzles = [
            Identifiers.VGM_SimpleSuppressorMuzzle,
            Identifiers.VGM_ShortSuppressorMuzzle,
            Identifiers.VGM_LongSuppressorMuzzle,
            Identifiers.VGM_FlashHiderMuzzle,
            Identifiers.VGM_LongBarrelMuzzle
        ];

        public static readonly string[] AllMuzzlesAttrSuppressor = [
            Identifiers.VGM_SimpleSuppressorMuzzle,
            Identifiers.VGM_ShortSuppressorMuzzle,
            Identifiers.VGM_LongSuppressorMuzzle,
        ];

        public static readonly string[] AllMuzzlesAttrFlashHider = [
            Identifiers.VGM_FlashHiderMuzzle,
        ];

        public static readonly string[] AllGrips = [
            Identifiers.VGM_AngledForeGrip,
            Identifiers.VGM_VerticalGrip
        ];

        public static readonly string[] AllAimingDevices = [
            Identifiers.VGM_RedDotSight,
            Identifiers.VGM_HolographicSight,
            Identifiers.VGM_ACOGScope,
            Identifiers.VGM_RifleScope,
            Identifiers.VGM_SniperScope
        ];

        public static readonly float MaxSpreadPerTickOnShoot = 1.0f;

        public bool VerifyPotentialErrors()
        {
            if (NotAllSame(hasCalledGenerateFiringModeBurstForHoldableXMLsString, hasCalledGenerateFiringModeBurstForRangedWeaponXMLsString))
            {
                ThrowError("The functionality of FiringModeBurst is incomplete.");
            }

            if ((hasCalledGenerateScannerActivationXMLsString || hasCalledGenerateStatusHUDXMLsString || hasCalledGenerateScannerOnContainedXMLsString)
                && NotAllSame(generatedHotTagWasAiming, hasCalledGenerateScannerActivationXMLsString, hasCalledGenerateStatusHUDXMLsString, hasCalledGenerateScannerOnContainedXMLsString))
            {
                ThrowError("The functionality of Scanner is incomplete.");
            }

            if (NotAllSame(CompatibleGrips is not null,
                hasCalledGenerateGripModifySpreadChangesOnAimDownSightXMLsString,
                hasCalledGenerateGripSpreadRecoveryXMLsString,
                hasCalledGenerateGripOnContainedXMLsString))
            {
                ThrowError("The functionality of Grip is incomplete.");
            }

            if (NotAllSame(CompatibleStocks is not null,
                hasCalledGenerateStockModifySpeedMultiplierXMLsString,
                hasCalledGenerateStockSimulatedRecoilXMLsString,
                hasCalledGenerateStockOnContainedXMLsString))
            {
                ThrowError("The functionality of Stock is incomplete.");
            }

            if (NotAllSame(CompatibleMuzzles is not null,
                hasCalledGenerateMuzzleModifySpreadChangesOnShootXMLsString,
                hasCalledGenerateMuzzleOnContainedXMLsString))
            {
                ThrowError("The functionality of Muzzle is incomplete.");
            }

            if (NotAllSame(hasCalledGenerateFlashHiderMuzzleOnShootXMLsString,
                    CompatibleMuzzles is not null && CompatibleMuzzles.Any(muzzle => AllMuzzlesAttrFlashHider.Contains(muzzle.Identifier))))
            {
                ThrowError("The functionality of Flash Hider is incomplete.");
            }

            if (NotAllSame(CompatibleAimingDevices is not null,
                hasCalledGenerateAimingDeviceModifySpreadChangesOnAimDownSightXMLsString,
                hasCalledGenerateAimingDeviceSpreadRecoveryXMLsString,
                hasCalledGenerateAimingDeviceObstructVisionXMLsString,
                hasCalledGenerateAimingDeviceOnContainedXMLsString))
            {
                ThrowError("The functionality of Aiming Device is incomplete.");
            }

            string ThrowError(string message) => throw new Exception(message);

            bool NotAllSame(params bool[] values)
            {
                bool first = values[0];
                return !values.All(v => v == first);
            }

            return true;
        }

        public string GenerateBasicInfoXMLsString(float fabricateRequiredSkill)
        {
            return
$@"<PreferredContainer primary=""secarmcab"" secondary=""armcab,weaponholder"" />
<Deconstruct time=""5.0"" />
<Fabricate suitablefabricators=""fabricator"" requiredtime=""5"" requiresrecipe=""false"">
    <RequiredSkill identifier=""weapons"" level=""{fabricateRequiredSkill}"" />
    <RequiredItem identifier=""{GunName.ToLower(CultureInfo.InvariantCulture)}"" />
</Fabricate>";
        }

        public string GenerateSpawnOEMStockXMLsString()
        {
            if (CompatibleStocks is not null && CompatibleStocks.Any())
            {
                return
$@"<!-- [Stock] If the gun is initialized for the first time, it will come with an OEM stock. -->
<StatusEffect type=""OnSpawn"" target=""This"">
    <SpawnItem identifier=""{Identifier}Stock"" spawnposition=""ThisInventory"" SpawnIfCantBeContained=""false"" SpawnIfInventoryFull=""false"" />
    <Conditional HasBeenInstantiatedOnce=""false""/>
</StatusEffect>";
            }
            else
            {
                throw new Exception("The gun has no OEM stock.");
            }
        }

        public string GenerateGunXMLAttributesString()
        {
            return
$@"name=""VGM {GunName}""
identifier=""{Identifier}""
category=""{Category}""
subcategory=""{SubCategory}""
tags=""{SelfTags}""
scale=""{Scale}""";
        }

        public string GenerateHoldableXMLAttributesString(
            string[] slots,
            bool controlPos,
            float[] aimPos,
            float[] handle1,
            float[]? holdPos = null,
            float[]? handle2 = null)
        {
            return
$@"slots=""{ConcatValues(slots)}""
controlpos=""{controlPos}""
{(holdPos is not null ? $@"holdpos=""{ConcatValues(holdPos)}""" : string.Empty)}
aimpos=""{ConcatValues(aimPos)}""
handle1=""{ConcatValues(handle1)}""
{(handle2 is not null ? $@"handle2=""{ConcatValues(handle2)}""" : string.Empty)}
{(HoldAngle.HasValue ? $@"holdangle=""{HoldAngle}""" : string.Empty)}";
        }

        public string GenerateGunModifySpeedMultiplierXMLsString()
        {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine($@"<!-- [Gun] Movement speed modification -->");

            if (CompatibleStocks is not null)
            {
                stringBuilder.AppendLine(
$@"<StatusEffect type=""OnActive"" target=""Character"" speedmultiplier=""{StocklessSpeedMultiplier}"" setvalue=""true"">
    <RequiredItem tag=""{Tags.VGM_Accessory}"" excludedtag=""{Tags.VGM_Stock}"" type=""Contained"" targetslot=""{StockSlotIndex}"" matchonempty=""true"" />
</StatusEffect>");
            }
            else
            {
                stringBuilder.AppendLine(
$@"<StatusEffect type=""OnActive"" target=""Character"" speedmultiplier=""{StocklessSpeedMultiplier}"" setvalue=""true""/>");
            }

            return stringBuilder.ToString();
        }

        private bool hasCalledGenerateStockModifySpeedMultiplierXMLsString = false;
        public string GenerateStockModifySpeedMultiplierXMLsString()
        {
            hasCalledGenerateStockModifySpeedMultiplierXMLsString = true;

            if (CompatibleStocks is null) { throw new NullReferenceException($@"Unable to generate stock code because '{GunName}' has no stock defined."); }

            StringBuilder stringBuilder = new();

            bool hasModifier = false;

            CompatibleStocks.ForEach(containableStock =>
            {
                var stat = AccessoryStock.Stats[containableStock.Identifier];
                if (stat.StocklessBasedSpeedMultiplier.HasValue)
                {
                    float speedMultiplier = StocklessSpeedMultiplier * stat.StocklessBasedSpeedMultiplier.Value;
                    stringBuilder.AppendLine(
$@"<StatusEffect type=""OnActive"" target=""Character"" speedmultiplier=""{speedMultiplier}"" setvalue=""true"">
    <RequiredItem identifier=""{containableStock.Identifier}"" type=""Contained"" targetslot=""{StockSlotIndex}"" />
</StatusEffect>");
                    hasModifier = true;
                }
            });

            if (hasModifier)
            {
                stringBuilder.Insert(0, "<!-- [Stock] Movement speed modification -->\n");
            }

            return stringBuilder.ToString();
        }

        public string GenerateGunSpreadChangesOnAimDownSightXMLsString()
        {
            return
$@"<!-- [Gun] Reset spread for ADS -->
<StatusEffect type=""OnActive"" target=""This"" targetitemcomponent=""RangedWeapon"" spread=""{MinimumSpread + SpreadChangesOnAimDownSight}"" unskilledspread=""{MinimumUnskilledSpread + SpreadChangesOnAimDownSight}"" setvalue=""true"">
    <Conditional hasstatustag=""! {StatusEffectTags.PreventSpreadingOnADS}"" />
</StatusEffect>";
        }

        private bool hasCalledGenerateGripModifySpreadChangesOnAimDownSightXMLsString = false;
        public string GenerateGripModifySpreadChangesOnAimDownSightXMLsString()
        {
            hasCalledGenerateGripModifySpreadChangesOnAimDownSightXMLsString = true;

            if (CompatibleGrips is null) { throw new NullReferenceException($@"Unable to generate grip code because '{GunName}' has no grip defined."); }

            StringBuilder stringBuilder = new();

            bool hasModifier = false;

            CompatibleGrips.ForEach(containableGrip =>
            {
                var stat = AccessoryGrip.Stats[containableGrip.Identifier];

                if (!stat.SpreadChangesOnAimDownSightMultiplier.HasValue) { return; }

                float spreadModifierOnADS = SpreadChangesOnAimDownSight * (stat.SpreadChangesOnAimDownSightMultiplier.Value - 1.0f);

                stringBuilder.AppendLine(
$@"<StatusEffect type=""OnSecondaryUse"" target=""This"" targetitemcomponent=""RangedWeapon"" spread=""{spreadModifierOnADS}"" unskilledspread=""{spreadModifierOnADS}"" disabledeltatime=""true"">
    <Conditional hasstatustag=""! {StatusEffectTags.PreventSpreadingOnADS}"" />
    <RequiredItem identifier=""{containableGrip.Identifier}"" type=""Contained"" targetslot=""{LowerAccessorySlotIndex}"" excludebroken=""false"" />
</StatusEffect>");
                hasModifier = true;
            });

            if (hasModifier)
            {
                stringBuilder.Insert(0, "<!-- [Grip] Modify spread on ADS -->\n");
            }

            return stringBuilder.ToString();
        }

        private bool hasCalledGenerateAimingDeviceModifySpreadChangesOnAimDownSightXMLsString = false;
        public string GenerateAimingDeviceModifySpreadChangesOnAimDownSightXMLsString()
        {
            hasCalledGenerateAimingDeviceModifySpreadChangesOnAimDownSightXMLsString = true;

            if (CompatibleAimingDevices is null) { throw new NullReferenceException($@"Unable to generate aiming device code because '{GunName}' has no aiming device defined."); }

            StringBuilder stringBuilder = new();

            bool hasModifier = false;

            CompatibleAimingDevices.ForEach(containableAimingDevice =>
            {
                var stat = AccessoryAimingDevice.Stats[containableAimingDevice.Identifier];

                if (!stat.SpreadChangesOnAimDownSightMultiplier.HasValue) { return; }

                float spreadModifierOnADS = SpreadChangesOnAimDownSight * (stat.SpreadChangesOnAimDownSightMultiplier.Value - 1.0f);

                stringBuilder.AppendLine(
$@"<StatusEffect type=""OnSecondaryUse"" target=""This"" targetitemcomponent=""RangedWeapon"" spread=""{spreadModifierOnADS}"" unskilledspread=""{spreadModifierOnADS}"" disabledeltatime=""true"">
    <Conditional hasstatustag=""! {StatusEffectTags.PreventSpreadingOnADS}"" />
    <RequiredItem identifier=""{containableAimingDevice.Identifier}"" type=""Contained"" targetslot=""{UpperAccessorySlotIndex}"" excludebroken=""false"" />
</StatusEffect>");

                hasModifier = true;
            });

            if (hasModifier)
            {
                stringBuilder.Insert(0, "<!-- [AimingDevice] Modify spread on ADS -->\n");
            }

            return stringBuilder.ToString();
        }


        public string GenerateGunSpreadRecoveryXMLsString()
        {
            return
$@"<!-- [Gun] Spread recovery -->
<StatusEffect type=""OnSecondaryUse"" target=""This"" targetitemcomponent=""RangedWeapon"" spread=""{-SpreadRecovery}"" disabledeltatime=""true"">
    <Conditional targetitemcomponent=""RangedWeapon"" spread=""gt {MinimumSpread + SpreadRecovery / 2}"" />
</StatusEffect>
<StatusEffect type=""OnSecondaryUse"" target=""This"" targetitemcomponent=""RangedWeapon"" unskilledspread=""{-SpreadRecovery}"" disabledeltatime=""true"">
    <Conditional targetitemcomponent=""RangedWeapon"" unskilledspread=""gt {MinimumUnskilledSpread + SpreadRecovery / 2}"" />
</StatusEffect>";
        }

        private bool hasCalledGenerateGripSpreadRecoveryXMLsString = false;
        public string GenerateGripSpreadRecoveryXMLsString()
        {
            hasCalledGenerateGripSpreadRecoveryXMLsString = true;

            if (CompatibleGrips is null) { throw new NullReferenceException($@"Unable to generate grip code because '{GunName}' has no grip defined."); }

            StringBuilder stringBuilder = new();

            bool hasModifier = false;

            CompatibleGrips.ForEach(compatibleGrip =>
            {
                var stat = AccessoryGrip.Stats[compatibleGrip.Identifier];

                if (!stat.SpreadRecoveryMultiplier.HasValue) { return; }

                float extraSpreadRecovery = SpreadRecovery * (stat.SpreadRecoveryMultiplier.Value - 1.0f);
                stringBuilder.AppendLine(
$@"<StatusEffect type=""OnSecondaryUse"" target=""This"" targetitemcomponent=""RangedWeapon"" spread=""{-extraSpreadRecovery}"" disabledeltatime=""true"">
    <Conditional targetitemcomponent=""RangedWeapon"" spread=""gt {MinimumSpread + extraSpreadRecovery / 2}"" />
    <RequiredItem identifier=""{compatibleGrip.Identifier}"" type=""Contained"" targetslot=""{LowerAccessorySlotIndex}"" excludebroken=""false"" />
</StatusEffect>
<StatusEffect type=""OnSecondaryUse"" target=""This"" targetitemcomponent=""RangedWeapon"" unskilledspread=""{-extraSpreadRecovery}"" disabledeltatime=""true"">
    <Conditional targetitemcomponent=""RangedWeapon"" unskilledspread=""gt {MinimumUnskilledSpread + extraSpreadRecovery / 2}"" />
    <RequiredItem identifier=""{compatibleGrip.Identifier}"" type=""Contained"" targetslot=""{LowerAccessorySlotIndex}"" excludebroken=""false"" />
</StatusEffect>");
                hasModifier = true;
            });

            if (hasModifier)
            {
                stringBuilder.Insert(0, "<!-- [Grip] Spread recovery -->\n");
            }

            return stringBuilder.ToString();
        }

        private bool hasCalledGenerateAimingDeviceSpreadRecoveryXMLsString = false;
        public string GenerateAimingDeviceSpreadRecoveryXMLsString()
        {
            hasCalledGenerateAimingDeviceSpreadRecoveryXMLsString = true;

            if (CompatibleAimingDevices is null) { throw new NullReferenceException($@"Unable to generate aiming device code because '{GunName}' has no aiming device defined."); }

            StringBuilder stringBuilder = new();

            bool hasModifier = false;

            CompatibleAimingDevices.ForEach(containableAimingDevice =>
            {
                var stat = AccessoryAimingDevice.Stats[containableAimingDevice.Identifier];

                if (!stat.SpreadRecoveryMultiplier.HasValue) { return; }

                float extraSpreadRecovery = SpreadRecovery * (stat.SpreadRecoveryMultiplier.Value - 1.0f);
                float spreadRecoveryThreshold = MinimumSpread;
                if (stat.MinimumSpreadOnRecoveringMultiplier.HasValue) { spreadRecoveryThreshold *= stat.MinimumSpreadOnRecoveringMultiplier.Value; }
                float neutralSpreadModifier = spreadRecoveryThreshold - MinimumSpread;

                stringBuilder.AppendLine(
$@"<StatusEffect type=""OnSecondaryUse"" target=""This"" targetitemcomponent=""RangedWeapon"" spread=""{-extraSpreadRecovery}"" disabledeltatime=""true"">
    {(extraSpreadRecovery > 0.0f
        ? $@"<Conditional targetitemcomponent=""RangedWeapon"" spread=""gt {MinimumSpread + neutralSpreadModifier + extraSpreadRecovery / 2}"" />"
        : $@"<Conditional targetitemcomponent=""RangedWeapon"" spread=""gt {MinimumSpread + neutralSpreadModifier + SpreadRecovery / 2}"" />"
    )}
    <RequiredItem identifier=""{containableAimingDevice.Identifier}"" type=""Contained"" targetslot=""{UpperAccessorySlotIndex}"" excludebroken=""false"" />
</StatusEffect>
<StatusEffect type=""OnSecondaryUse"" target=""This"" targetitemcomponent=""RangedWeapon"" unskilledspread=""{-extraSpreadRecovery}"" disabledeltatime=""true"">
    {(extraSpreadRecovery > 0.0f
        ? $@"<Conditional targetitemcomponent=""RangedWeapon"" unskilledspread=""gt {MinimumUnskilledSpread + neutralSpreadModifier + extraSpreadRecovery / 2}"" />"
        : $@"<Conditional targetitemcomponent=""RangedWeapon"" unskilledspread=""gt {MinimumUnskilledSpread + neutralSpreadModifier + SpreadRecovery / 2}"" />"
    )}
    <RequiredItem identifier=""{containableAimingDevice.Identifier}"" type=""Contained"" targetslot=""{UpperAccessorySlotIndex}"" excludebroken=""false"" />
</StatusEffect>");

                hasModifier = true;

                if (neutralSpreadModifier < 0.0f)
                {
                    stringBuilder.AppendLine(
$@"<StatusEffect type=""OnSecondaryUse"" target=""This"" targetitemcomponent=""RangedWeapon"" spread=""{-SpreadRecovery}"" disabledeltatime=""true"" comparison=""And"">
    <Conditional targetitemcomponent=""RangedWeapon"" spread=""gt {MinimumSpread + neutralSpreadModifier + SpreadRecovery / 2}"" />
    <Conditional targetitemcomponent=""RangedWeapon"" spread=""lte {MinimumSpread + SpreadRecovery / 2}"" />
    <RequiredItem identifier=""{containableAimingDevice.Identifier}"" type=""Contained"" targetslot=""{UpperAccessorySlotIndex}"" excludebroken=""false"" />
</StatusEffect>
<StatusEffect type=""OnSecondaryUse"" target=""This"" targetitemcomponent=""RangedWeapon"" unskilledspread=""{-SpreadRecovery}"" disabledeltatime=""true"" comparison=""And"">
    <Conditional targetitemcomponent=""RangedWeapon"" unskilledspread=""gt {MinimumUnskilledSpread + neutralSpreadModifier + SpreadRecovery / 2}"" />
    <Conditional targetitemcomponent=""RangedWeapon"" unskilledspread=""lte {MinimumUnskilledSpread + SpreadRecovery / 2}"" />
    <RequiredItem identifier=""{containableAimingDevice.Identifier}"" type=""Contained"" targetslot=""{UpperAccessorySlotIndex}"" excludebroken=""false"" />
</StatusEffect>");
                }
            });

            if (hasModifier)
            {
                stringBuilder.Insert(0, "<!-- [AimingDevice] Spread recovery -->\n");
            }

            return stringBuilder.ToString();
        }

        private bool hasCalledGenerateAimingDeviceObstructVisionXMLsString = false;
        public string GenerateAimingDeviceObstructVisionXMLsString()
        {
            hasCalledGenerateAimingDeviceObstructVisionXMLsString = true;

            if (CompatibleAimingDevices is null) { throw new NullReferenceException($@"Unable to generate aiming device code because '{GunName}' has no aiming device defined."); }

            StringBuilder stringBuilder = new();

            bool hasModifier = false;

            CompatibleAimingDevices.ForEach(containableAimingDevice =>
            {
                var stat = AccessoryAimingDevice.Stats[containableAimingDevice.Identifier];
                if (stat.ObstructVisionAmount.HasValue)
                {
                    stringBuilder.AppendLine(
$@"<StatusEffect type=""OnSecondaryUse"" target=""Character"" obstructvisionamount=""{stat.ObstructVisionAmount.Value}"" setvalue=""true"" comparison=""And"">
    <Conditional islocalplayer=""true"" obstructvisionamount=""lt {stat.ObstructVisionAmount.Value}""/>
    <RequiredItem identifier=""{containableAimingDevice.Identifier}"" type=""Contained"" targetslot=""{UpperAccessorySlotIndex}"" excludebroken=""false"" />
</StatusEffect>");
                    hasModifier = true;
                }
            });

            if (hasModifier)
            {
                stringBuilder.Insert(0, "<!-- [AimingDevice] Obstruct vision -->\n");
            }

            return stringBuilder.ToString();
        }

        private bool hasCalledGenerateScannerActivationXMLsString = false;
        public string GenerateScannerActivationXMLsString()
        {
            hasCalledGenerateScannerActivationXMLsString = true;
            return
$@"<!-- [Scanner] Perform by aiming when the gun is equipped with a scanner. -->
<StatusEffect type=""OnSecondaryUse"" target=""This"" targetitemcomponent=""StatusHUD"" drawhudwhenequipped=""true"" isactive=""true"">
    <Conditional hasstatustag=""{StatusEffectTags.WasAiming}"" />
    <RequiredItem tag=""{Tags.VGM_Scanner}"" excludebroken=""false"" type=""Contained"" targetslot=""{ScannerSlotIndex}"" />
</StatusEffect>
<StatusEffect type=""OnActive"" target=""This"" targetitemcomponent=""StatusHUD"" drawhudwhenequipped=""false"" isactive=""false"">
    <Conditional hasstatustag=""! {StatusEffectTags.WasAiming}"" />
</StatusEffect>
";
        }

        private bool generatedHotTagWasAiming = false;
        public string GenerateHotTagWasAimingXMLsString()
        {
            generatedHotTagWasAiming = true;
            return
$@"<!-- [HotTag] Activated when aiming -->
<StatusEffect type=""OnSecondaryUse"" target=""This"" statuseffecttags=""{StatusEffectTags.WasAiming}"" duration=""{GetTickDurationString(2)}"" stackable=""false"" />";
        }

        public string GenerateHotTagPreventSpreadingOnADSXMLsString()
        {
            return
$@"<!-- [HotTag] Activated when aiming -->
<StatusEffect type=""OnSecondaryUse"" target=""This"" statuseffecttags=""{StatusEffectTags.PreventSpreadingOnADS}"" duration=""0.15"" stackable=""false"" />";
        }

        private bool hasCalledGenerateStatusHUDXMLsString = false;
        public string GenerateStatusHUDXMLsString()
        {
            hasCalledGenerateStatusHUDXMLsString = true;
            return $@"<StatusHUD drawhudwhenequipped=""false"" />";
        }

        public string GenerateAiTargetXMLsString(float sightRange = 2000, float fadeoutTime = 5)
        {
            return $@"<AiTarget sightrange=""{sightRange}"" soundrange=""{NormalSoundRangeOnShoot}"" fadeouttime=""{fadeoutTime}"" />";
        }

        public string GenerateRangedWeaponXMLAttributesString()
        {
            return
$@"weapondamagemodifier=""{WeaponDamageModifier}""
penetration=""{Penetration}""
reload=""{Reload}""
{(ReloadSkillRequirement.HasValue ? $@"reloadskillrequirement=""{ReloadSkillRequirement.Value}""" : string.Empty)}
{(ReloadNoSkill.HasValue ? $@"reloadnoskill=""{ReloadNoSkill.Value}""" : string.Empty)}
spread=""{MinimumSpread}""
unskilledspread=""{MinimumUnskilledSpread}""
barrelpos=""{ConcatValues(BarrelPos)}""
combatpriority=""{CombatPriority}""
holdtrigger=""true""
drawhudwhenequipped=""true""
crosshairscale=""{CrosshairScale}""";
        }

        public string GenerateMajorRequiredWeaponsSkillXMLsString()
        {
            return $@"<RequiredSkill identifier=""weapons"" level=""{RequiredWeaponsSkill}"" />";
        }

        public string GenerateMajorSkillRequirementHintXMLsString()
        {
            return $@"<SkillRequirementHint identifier=""weapons"" level=""{RequiredWeaponsSkill}"" />";
        }

        public string GenerateDefaultCrosshairXMLsString()
        {
            return
$@"<Crosshair texture=""Content/Items/Weapons/Crosshairs.png"" sourcerect=""0,256,256,256"" />
<CrosshairPointer texture=""Content/Items/Weapons/Crosshairs.png"" sourcerect=""256,256,256,256"" />";
        }

        public string GenerateGunfireOnShootXMLsString(string[] normalSoundFiles, string[]? suppressedSoundFiles = null)
        {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine($@"<!-- [Gun] Gunfire -->");

            bool isFirst = true;
            stringBuilder.AppendLine(
$@"<StatusEffect type=""OnUse"" target=""This"" forceplaysounds=""true"">
    {(
        string.Join(
            '\n',
            normalSoundFiles.Select(file =>
            {
                string xmlString = $@"<Sound file=""{file}"" range=""{NormalSoundRangeOnShoot}"" {(isFirst ? $@"selectionmode=""Random""" : string.Empty)}/>";
                isFirst = false;
                return xmlString;
            })
        )
    )}
    {(CompatibleMuzzles is not null
        ? $@"<RequiredItem tag=""{Tags.VGM_Accessory}"" excludedtag=""{Tags.VGM_MuzzleAttrSuppressor}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" matchonempty=""true"" />"
        : string.Empty)}
</StatusEffect>");

            if (CompatibleMuzzles is not null && CompatibleMuzzles.Any(muzzle => AllMuzzlesAttrSuppressor.Contains(muzzle.Identifier)))
            {
                suppressedSoundFiles = suppressedSoundFiles ?? [
                    @"%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/Sounds/weapon_fire_suppressed_1.ogg",
                    @"%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/Sounds/weapon_fire_suppressed_2.ogg",
                    @"%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/Sounds/weapon_fire_suppressed_3.ogg",
                    @"%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/Sounds/weapon_fire_suppressed_4.ogg",
                    @"%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/Sounds/weapon_fire_suppressed_5.ogg",
                ];
                isFirst = true;
                stringBuilder.AppendLine(
$@"<StatusEffect type=""OnUse"" target=""This"" soundrange=""{SuppressedSoundRangeOnShoot}"" setvalue=""true"" forceplaysounds=""true"">
    {(
        string.Join(
            '\n',
            suppressedSoundFiles.Select(file =>
            {
                string xmlString = $@"<Sound file=""{file}"" range=""{SuppressedSoundRangeOnShoot}"" {(isFirst ? $@"selectionmode=""Random""" : string.Empty)}/>";
                isFirst = false;
                return xmlString;
            })
        )
    )}
    <RequiredItem tag=""{Tags.VGM_MuzzleAttrSuppressor}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" />
</StatusEffect>");
            }

            return stringBuilder.ToString();
        }

        private bool hasCalledGenerateFlashHiderMuzzleOnShootXMLsString = false;
        public string GenerateFlashHiderMuzzleOnShootXMLsString()
        {
            hasCalledGenerateFlashHiderMuzzleOnShootXMLsString = true;

            return
$@"<!-- [Muzzle] Flash hider -->
<StatusEffect type=""OnUse"" target=""This"">
    <Explosion showeffects=""false"" flash=""true"" flashrange=""500"" />
    <RequiredItem tag=""{Tags.VGM_Muzzle}"" excludedtag=""{Tags.VGM_MuzzleAttrFlashHider}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" matchonempty=""true"" />
</StatusEffect>
<StatusEffect type=""OnUse"" target=""This"" sightrange=""500"" setvalue=""true"">
    <RequiredItem tag=""{Tags.VGM_MuzzleAttrFlashHider}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" />
</StatusEffect>";
        }

        public string GenerateGunSpreadChangesOnShootXMLsString()
        {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine("<!-- [Gun] Spread on shoot -->");
            float instantSpread = SpreadChangesOnShoot * 1.0f / 3.0f;
            float durationSpread = SpreadChangesOnShoot - instantSpread;
            int ticks = 1;
            float spreadPerTick;
            do
            {
                spreadPerTick = durationSpread / ++ticks;
            } while (spreadPerTick > MaxSpreadPerTickOnShoot);

            stringBuilder.AppendLine(
$@"<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""RangedWeapon"" spread=""{instantSpread}"" disabledeltatime=""true"">
    {(CompatibleMuzzles is not null
        ? $@"<RequiredItem tag=""{Tags.VGM_Accessory}"" excludedtag=""{Tags.VGM_MuzzleAttrReduceSpread}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" matchonempty=""true"" />"
        : string.Empty)}
</StatusEffect>
<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""RangedWeapon"" spread=""{spreadPerTick}"" stackable=""true"" duration=""{GetTickDurationString(ticks)}"" disabledeltatime=""true"" checkconditionalalways=""true"">
    <Conditional targetitemcomponent=""RangedWeapon"" spread=""lt {MinimumSpread + SpreadLimit - spreadPerTick / 2}"" />
    {(CompatibleMuzzles is not null
        ? $@"<RequiredItem tag=""{Tags.VGM_Accessory}"" excludedtag=""{Tags.VGM_MuzzleAttrReduceSpread}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" matchonempty=""true"" />"
        : string.Empty)}
</StatusEffect>
<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""RangedWeapon"" unskilledspread=""{instantSpread}"" disabledeltatime=""true"">
    {(CompatibleMuzzles is not null
        ? $@"<RequiredItem tag=""{Tags.VGM_Accessory}"" excludedtag=""{Tags.VGM_MuzzleAttrReduceSpread}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" matchonempty=""true"" />"
        : string.Empty)}
</StatusEffect>
<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""RangedWeapon"" unskilledspread=""{spreadPerTick}"" stackable=""true"" duration=""{GetTickDurationString(ticks)}"" disabledeltatime=""true"" checkconditionalalways=""true"">
    <Conditional targetitemcomponent=""RangedWeapon"" unskilledspread=""lt {MinimumUnskilledSpread + SpreadLimit - spreadPerTick / 2}"" />
    {(CompatibleMuzzles is not null
        ? $@"<RequiredItem tag=""{Tags.VGM_Accessory}"" excludedtag=""{Tags.VGM_MuzzleAttrReduceSpread}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" matchonempty=""true"" />"
        : string.Empty)}
</StatusEffect>");

            return stringBuilder.ToString();
        }

        private bool hasCalledGenerateMuzzleModifySpreadChangesOnShootXMLsString = false;
        public string GenerateMuzzleModifySpreadChangesOnShootXMLsString()
        {
            hasCalledGenerateMuzzleModifySpreadChangesOnShootXMLsString = true;

            if (CompatibleMuzzles is null) { throw new NullReferenceException($@"Unable to generate muzzle code because '{GunName}' has no muzzle defined."); }

            StringBuilder stringBuilder = new();

            bool hasModifier = false;

            CompatibleMuzzles.ForEach(muzzle =>
            {
                var stat = AccessoryMuzzle.Stats[muzzle.Identifier];
                if (stat.SpreadChangesOnShootMultiplier.HasValue)
                {
                    float spreadChangesOnShoot = SpreadChangesOnShoot * stat.SpreadChangesOnShootMultiplier.Value;

                    float instantSpread = spreadChangesOnShoot * 1.0f / 3.0f;
                    float durationSpread = spreadChangesOnShoot - instantSpread;
                    int ticks = 1;
                    float spreadPerTick;
                    do
                    {
                        spreadPerTick = durationSpread / ++ticks;
                    } while (spreadPerTick > MaxSpreadPerTickOnShoot);

                    stringBuilder.AppendLine(
$@"<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""RangedWeapon"" spread=""{instantSpread}"" disabledeltatime=""true"">
    <RequiredItem identifier=""{muzzle.Identifier}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" />
</StatusEffect>
<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""RangedWeapon"" spread=""{spreadPerTick}"" stackable=""true"" duration=""{GetTickDurationString(ticks)}"" disabledeltatime=""true"" checkconditionalalways=""true"">
    <Conditional targetitemcomponent=""RangedWeapon"" spread=""lt {MinimumSpread + SpreadLimit - spreadPerTick / 2}"" />
    <RequiredItem identifier=""{muzzle.Identifier}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" />
</StatusEffect>
<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""RangedWeapon"" unskilledspread=""{instantSpread}"" disabledeltatime=""true"">
    <RequiredItem identifier=""{muzzle.Identifier}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" />
</StatusEffect>
<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""RangedWeapon"" unskilledspread=""{spreadPerTick}"" stackable=""true"" duration=""{GetTickDurationString(ticks)}"" disabledeltatime=""true"" checkconditionalalways=""true"">
    <Conditional targetitemcomponent=""RangedWeapon"" unskilledspread=""lt {MinimumUnskilledSpread + SpreadLimit - spreadPerTick / 2}"" />
    <RequiredItem identifier=""{muzzle.Identifier}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" />
</StatusEffect>");
                    hasModifier = true;
                }
            });

            if (hasModifier)
            {
                stringBuilder.Insert(0, "<!-- [Muzzle] Modify spread on shoot -->\n");
            }

            return stringBuilder.ToString();
        }

        private const float recoilFeelMultiplier = 0.85f;
        private const float recoilFeelNoSkillMultiplier = 1.15f;
        private const float recoilFeelCausesBlunttraumaThresholdMin = 300;
        private const float recoilFeelCausesBlunttraumaThresholdMax = 500;
        private string GenerateRecoilFeelCausesBlunttraumaXMLsString(float recoilFeel)
        {

            return
$@"<Affliction identifier=""blunttrauma""
strength=""{MathUtils.Remap(recoilFeel, recoilFeelCausesBlunttraumaThresholdMin, recoilFeelCausesBlunttraumaThresholdMax, 2, 12)}""
penetration=""0.5"" dividebylimbcount=""true"" />";
        }

        public string GenerateGunSimulatedRecoilXMLsString()
        {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine($@"<!-- [Gun] Simulated recoil -->");
            stringBuilder.AppendLine($@"<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""Propulsion"" usablein=""Both"" />");

            float recoilFeel = Recoil * recoilFeelMultiplier;
            float recoilFeelNoSkill = Recoil * recoilFeelNoSkillMultiplier;

            if (CompatibleStocks is not null)
            {
                stringBuilder.AppendLine(
$@"<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""Propulsion"" force=""{-Recoil}"" setvalue=""true"">
    <RequiredItem tag=""{Tags.VGM_Accessory}"" excludedtag=""{Tags.VGM_Stock}"" type=""Contained"" targetslot=""{StockSlotIndex}"" matchonempty=""true"" />
</StatusEffect>
<StatusEffect type=""OnUse"" target=""Character"" camerashake=""{Recoil * AccessoryStock.CameraShakePerUnitRecoil}"" setvalue=""true"">
    <RequiredItem tag=""{Tags.VGM_Accessory}"" excludedtag=""{Tags.VGM_Stock}"" type=""Contained"" targetslot=""{StockSlotIndex}"" matchonempty=""true"" />
</StatusEffect>
{(recoilFeel >= recoilFeelCausesBlunttraumaThresholdMin
? $@"<StatusEffect type=""OnUse"" target=""Character"" targetlimbs=""RightArm,LeftArm"" disabledeltatime=""true"">
    {GenerateRecoilFeelCausesBlunttraumaXMLsString(recoilFeel)}
    <Conditional skillrequirement=""true"" weapons=""gte {RequiredWeaponsSkill}"" />
    <RequiredItem tag=""{Tags.VGM_Accessory}"" excludedtag=""{Tags.VGM_Stock}"" type=""Contained"" targetslot=""{StockSlotIndex}"" matchonempty=""true"" />
</StatusEffect>"
: string.Empty)}
{(recoilFeelNoSkill >= recoilFeelCausesBlunttraumaThresholdMin
? $@"<StatusEffect type=""OnUse"" target=""Character"" targetlimbs=""RightArm,LeftArm"" disabledeltatime=""true"">
    {GenerateRecoilFeelCausesBlunttraumaXMLsString(recoilFeelNoSkill)}
    <Conditional skillrequirement=""true"" weapons=""lt {RequiredWeaponsSkill}"" />
    <RequiredItem tag=""{Tags.VGM_Accessory}"" excludedtag=""{Tags.VGM_Stock}"" type=""Contained"" targetslot=""{StockSlotIndex}"" matchonempty=""true"" />
</StatusEffect>"
: string.Empty)}");
            }
            else
            {
                stringBuilder.AppendLine(
$@"<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""Propulsion"" force=""{-Recoil}"" setvalue=""true"" />
<StatusEffect type=""OnUse"" target=""Character"" camerashake=""{Recoil * AccessoryStock.CameraShakePerUnitRecoil}"" setvalue=""true"" />
{(recoilFeel >= recoilFeelCausesBlunttraumaThresholdMin
? $@"<StatusEffect type=""OnUse"" target=""Character"" targetlimbs=""RightArm,LeftArm"" disabledeltatime=""true"">
    {GenerateRecoilFeelCausesBlunttraumaXMLsString(recoilFeel)}
    <Conditional skillrequirement=""true"" weapons=""gte {RequiredWeaponsSkill}"" />
</StatusEffect>"
: string.Empty)}
{(recoilFeelNoSkill >= recoilFeelCausesBlunttraumaThresholdMin
? $@"<StatusEffect type=""OnUse"" target=""Character"" targetlimbs=""RightArm,LeftArm"" disabledeltatime=""true"">
    {GenerateRecoilFeelCausesBlunttraumaXMLsString(recoilFeelNoSkill)}
    <Conditional skillrequirement=""true"" weapons=""lt {RequiredWeaponsSkill}"" />
</StatusEffect>"
: string.Empty)}");
            }

            return stringBuilder.ToString();
        }

        private bool hasCalledGenerateStockSimulatedRecoilXMLsString = false;
        public string GenerateStockSimulatedRecoilXMLsString()
        {
            hasCalledGenerateStockSimulatedRecoilXMLsString = true;

            if (CompatibleStocks is null) { throw new NullReferenceException($@"Unable to generate stock code because '{GunName}' has no stock defined."); }

            StringBuilder stringBuilder = new();

            bool hasModifier = false;

            CompatibleStocks.ForEach(compatibleStock =>
            {
                var stat = AccessoryStock.Stats[compatibleStock.Identifier];

                if (!stat.RecoilReduction.HasValue) { return; }

                float recoil = MathF.Max(0.0f, Recoil - stat.RecoilReduction.Value * StockRecoilReductionEfficiency);
                float cameraShake = recoil * AccessoryStock.CameraShakePerUnitRecoil;
                float recoilFeel = recoil * recoilFeelMultiplier;
                float recoilFeelNoSkill = recoil * recoilFeelNoSkillMultiplier;

                stringBuilder.AppendLine(
$@"<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""Propulsion"" force=""{-recoil}"" setvalue=""true"">
    <RequiredItem identifier=""{compatibleStock.Identifier}"" type=""Contained"" targetslot=""{StockSlotIndex}"" />
</StatusEffect>
<StatusEffect type=""OnUse"" target=""Character"" camerashake=""{recoil * AccessoryStock.CameraShakePerUnitRecoil}"" setvalue=""true"">
    <RequiredItem identifier=""{compatibleStock.Identifier}"" type=""Contained"" targetslot=""{StockSlotIndex}"" />
</StatusEffect>
{(recoilFeel >= recoilFeelCausesBlunttraumaThresholdMin
? $@"<StatusEffect type=""OnUse"" target=""Character"" targetlimbs=""RightArm,LeftArm"" disabledeltatime=""true"">
    {GenerateRecoilFeelCausesBlunttraumaXMLsString(recoilFeel)}
    <Conditional skillrequirement=""true"" weapons=""gte {RequiredWeaponsSkill}"" />
    <RequiredItem identifier=""{compatibleStock.Identifier}"" type=""Contained"" targetslot=""{StockSlotIndex}"" />
</StatusEffect>"
: string.Empty)}
{(recoilFeelNoSkill >= recoilFeelCausesBlunttraumaThresholdMin
? $@"<StatusEffect type=""OnUse"" target=""Character"" targetlimbs=""RightArm,LeftArm"" disabledeltatime=""true"">
    {GenerateRecoilFeelCausesBlunttraumaXMLsString(recoilFeelNoSkill)}
    <Conditional skillrequirement=""true"" weapons=""lt {RequiredWeaponsSkill}"" />
    <RequiredItem identifier=""{compatibleStock.Identifier}"" type=""Contained"" targetslot=""{StockSlotIndex}"" />
</StatusEffect>"
: string.Empty)}");
                hasModifier = true;
            });

            if (hasModifier)
            {
                stringBuilder.Insert(0, "<!-- [Stock] Simulated recoil -->\n");
            }

            return stringBuilder.ToString();
        }

        public string GeneratePropulsionXMLsString()
        {
            return
$@"<Propulsion usablein=""None"" applytohands=""true"">
    <StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""Propulsion"" usablein=""None"" isactive=""false"" />
</Propulsion>";
        }

        private bool hasCalledGenerateStockOnContainedXMLsString = false;
        public record struct ContainableStock(string Identifier, float[] ItemPos);
        public string GenerateStockOnContainedXMLsString()
        {
            hasCalledGenerateStockOnContainedXMLsString = true;

            if (CompatibleStocks is null) { throw new NullReferenceException($@"Unable to generate stock code because '{GunName}' has no stock defined."); }

            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine("<!-- [Stock] -->");

            CompatibleStocks.ForEach(containableStock =>
            {
                stringBuilder.AppendLine($@"<Containable identifier=""{containableStock.Identifier}"" hide=""false"" itempos=""{ConcatValues(containableStock.ItemPos)}"" />");
            });

            stringBuilder.AppendLine($@"<Containable tag=""{Tags.VGM_Stock}Attr{GunName}Compatible"" hide=""false"" />");

            return stringBuilder.ToString();
        }

        public string GenerateContainableGenericAccessories(float[] itemPos)
        {
            return
$@"<Containable items=""{Identifiers.VGM_RGBLaserPointer},flashlight,glowstick,flare,alienflare"" hide=""false"" itempos=""{ConcatValues(itemPos)}"" setactive=""true"" />";
        }

        private bool hasCalledGenerateGripOnContainedXMLsString = false;
        public record struct ContainableGrip(string Identifier, float[] ItemPos);
        public string GenerateGripOnContainedXMLsString()
        {
            hasCalledGenerateGripOnContainedXMLsString = true;

            if (CompatibleGrips is null) { throw new NullReferenceException($@"Unable to generate grip code because '{GunName}' has no grip defined."); }

            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine(
$@"<!-- [Grip] Changes the gun's properties through the use of sub-items,
and sets the sub-item's condition to full on round loaded to prevent the accessory mod effects from being reset. -->");

            CompatibleGrips.ForEach(grip =>
            {
                stringBuilder.AppendLine($@"<Containable identifier=""{grip.Identifier}"" hide=""false"" itempos=""{ConcatValues(grip.ItemPos)}"" />");
            });

            stringBuilder.AppendLine(
$@"<Containable tag=""{Tags.VGM_Grip}Attr{GunName}Compatible"" excludebroken=""false"" excludefullcondition=""false"" hide=""false"">
    <StatusEffect type=""OnContaining"" target=""Contained"" condition=""3.402823466E+38"" setvalue=""true"" oneshot=""true"" />
    <StatusEffect type=""OnRemoved"" target=""Contained"" condition=""3.402823466E+38"" setvalue=""true"" />
    <StatusEffect type=""OnRemoved"" target=""This"" targetitemcomponent=""Holdable"" holdangle=""{HoldAngle}"" setvalue=""true"" />
</Containable>
<Containable tag=""{Tags.VGM_Grip}Attr{GunName}Compatible"" excludebroken=""true"" excludefullcondition=""false"" hide=""false"">
    <StatusEffect type=""OnContaining"" target=""Contained"" condition=""0.0"" setvalue=""true"">
        <Use />
    </StatusEffect>
</Containable>");

            return stringBuilder.ToString();
        }

        private bool hasCalledGenerateAimingDeviceOnContainedXMLsString = false;
        public record struct ContainableAimingDevice(string Identifier, float[] ItemPos);
        public string GenerateAimingDeviceOnContainedXMLsString()
        {
            hasCalledGenerateAimingDeviceOnContainedXMLsString = true;

            if (CompatibleAimingDevices is null) { throw new NullReferenceException($@"Unable to generate aiming device code because '{GunName}' has no aiming device defined."); }

            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine($@"<!-- [AimingDevice] -->");

            CompatibleAimingDevices.ForEach(aimingDevice =>
            {
                stringBuilder.AppendLine(
$@"<Containable identifier=""{aimingDevice.Identifier}"" hide=""false"" itempos=""{ConcatValues(aimingDevice.ItemPos)}"" />");
            });

            stringBuilder.AppendLine(
$@"<Containable tag=""{Tags.VGM_AimingDevice}Attr{GunName}Compatible"" excludebroken=""false"" excludefullcondition=""false"" hide=""false"">
    <StatusEffect type=""OnContaining"" target=""Contained"" condition=""3.402823466E+38"" setvalue=""true"" oneshot=""true"" />
    <StatusEffect type=""OnRemoved"" target=""Contained"" condition=""3.402823466E+38"" setvalue=""true"" />
    <StatusEffect type=""OnRemoved"" target=""This"" targetitemcomponent=""Holdable"" cameraaimoffset=""0.0"" setvalue=""true"" />
    <StatusEffect type=""OnRemoved"" target=""This"" targetitemcomponent=""RangedWeapon"" crosshairscale=""{CrosshairScale}"" setvalue=""true"" />
</Containable>
<Containable tag=""{Tags.VGM_AimingDevice}Attr{GunName}Compatible"" excludebroken=""true"" excludefullcondition=""false"" hide=""false"">
    <StatusEffect type=""OnContaining"" target=""Contained"" condition=""0.0"" setvalue=""true"">
        <Use />
    </StatusEffect>
</Containable>");

            return stringBuilder.ToString();
        }

        public record struct ContainableMuzzle
        {
            public string Identifier;
            public float?[] ItemPos;
            public float?[] BarrelPos;

            public ContainableMuzzle(string identifier, float?[]? itemPos = null, float?[]? barrelPos = null)
            {
                Identifier = identifier;
                ItemPos = itemPos ?? [null, null];
                BarrelPos = barrelPos ?? [null, null];
            }
        }

        private bool hasCalledGenerateMuzzleOnContainedXMLsString = false;
        public string GenerateMuzzleOnContainedXMLsString()
        {
            hasCalledGenerateMuzzleOnContainedXMLsString = true;

            if (CompatibleMuzzles is null) { throw new NullReferenceException($@"Unable to generate muzzle code because '{GunName}' has no muzzle defined."); }

            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine("<!-- [Muzzle] When inserted, modifies the gun's weapon damage (serializable) and continuously sets the gun's barrelpos (non-serializable). -->");

            CompatibleMuzzles.ForEach(muzzle =>
            {
                var stat = AccessoryMuzzle.Stats[muzzle.Identifier];

                if (muzzle.ItemPos is null) { muzzle.ItemPos = []; }
                if (muzzle.ItemPos.Length < 1 || !muzzle.ItemPos[0].HasValue) { muzzle.ItemPos[0] = MathF.Floor(BarrelPos[0] * Scale + stat.BarrelLength * Scale / 2); }
                if (muzzle.ItemPos.Length < 2 || !muzzle.ItemPos[1].HasValue) { muzzle.ItemPos[1] = MathF.Round(BarrelPos[1] * Scale); }

                if (muzzle.BarrelPos is null) { muzzle.BarrelPos = []; }
                if (muzzle.BarrelPos.Length < 1 || !muzzle.BarrelPos[0].HasValue) { muzzle.BarrelPos[0] = MathF.Floor(BarrelPos[0] + stat.BarrelLength); }
                if (muzzle.BarrelPos.Length < 2 || !muzzle.BarrelPos[1].HasValue) { muzzle.BarrelPos[1] = BarrelPos[1]; }

                stringBuilder.AppendLine(
$@"<Containable identifier=""{muzzle.Identifier}"" hide=""false"" itempos=""{ConcatValues(muzzle.ItemPos)}"">
    {(stat.WeaponDamageMultiplier.HasValue || stat.PenetrationModifier.HasValue
        ? $@"<StatusEffect type=""OnInserted"" target=""This"" targetitemcomponent=""RangedWeapon""
            {(stat.WeaponDamageMultiplier.HasValue ? $@"weapondamagemodifier=""{stat.WeaponDamageMultiplier.Value * WeaponDamageModifier}""" : string.Empty)}
            {(stat.PenetrationModifier.HasValue ? $@"penetration=""{Penetration + stat.PenetrationModifier.Value}""" : string.Empty)}
            setvalue=""true"" delay=""{GetTickDurationString(1)}"" />"
        : string.Empty
    )}
    <StatusEffect type=""OnContaining"" target=""This"" targetitemcomponent=""RangedWeapon"" barrelpos=""{ConcatValues(muzzle.BarrelPos)}"" setvalue=""true"" />
</Containable>");

            });

            stringBuilder.AppendLine(
$@"<Containable tag=""{Tags.VGM_Muzzle}Attr{GunName}Compatible"" hide=""false"">
    <StatusEffect type=""OnRemoved"" target=""This"" weapondamagemodifier=""{WeaponDamageModifier}"" penetration=""{Penetration}"" barrelpos=""{ConcatValues(BarrelPos)}"" setvalue=""true"" disabledeltatime=""true"" />
</Containable>");

            return stringBuilder.ToString();
        }

        private bool hasCalledGenerateScannerOnContainedXMLsString = false;
        public record struct ContainableScanner(string Identifier, float[] ItemPos);
        public string GenerateScannerOnContainedXMLsString(ContainableScanner[] containableScanners)
        {
            hasCalledGenerateScannerOnContainedXMLsString = true;

            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine(
$@"<!-- [Scanner] -->");

            containableScanners.ForEach(scanner =>
            {
                stringBuilder.AppendLine($@"<Containable identifier=""{scanner.Identifier}"" hide=""false"" itempos=""{ConcatValues(scanner.ItemPos)}"" />");
            });

            stringBuilder.AppendLine(
$@"<Containable tag=""{Tags.VGM_Scanner}"" excludebroken=""false"" excludefullcondition=""false"" hide=""false"">
    <StatusEffect type=""OnContaining"" target=""Contained"" condition=""3.402823466E+38"" setvalue=""true"" oneshot=""true"" />
    <StatusEffect type=""OnRemoved"" target=""Contained"" condition=""3.402823466E+38"" setvalue=""true"" />
</Containable>
<Containable tag=""{Tags.VGM_Scanner}"" excludebroken=""true"" excludefullcondition=""false"" hide=""false"">
    <StatusEffect type=""OnContaining"" target=""Contained"" condition=""0.0"" setvalue=""true"">
        <Use />
    </StatusEffect>
</Containable>");

            return stringBuilder.ToString();
        }

        private bool hasCalledGenerateFiringModeBurstForHoldableXMLsString = false;
        public string GenerateFiringModeBurstForHoldableXMLsString()
        {
            hasCalledGenerateFiringModeBurstForHoldableXMLsString = true;

            return
$@"<!-- [FiringModeBurst] When the gun is loaded and not in bursting state, it is ready  -->
<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""RangedWeapon"" tags=""firingmodeburst_ready"" duration=""{GetTickDurationString(1)}"" comparison=""And"">
    <Conditional targetitemcomponent=""RangedWeapon"" isactive=""false"" />
    <Conditional targetitemcomponent=""RangedWeapon"" hasstatustag=""! firingmodeburst_active"" />
</StatusEffect>
<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""RangedWeapon"" reloadtimer=""0.0"" isactive=""false"" setvalue=""true"" comparison=""And"">
    <Conditional targetitemcomponent=""RangedWeapon"" hasstatustag=""firingmodeburst_active"" />
    <Conditional targetitemcomponent=""RangedWeapon"" hasstatustag=""! firingmodeburst_reloading"" />
</StatusEffect>";
        }

        private bool hasCalledGenerateFiringModeBurstForRangedWeaponXMLsString = false;
        public string GenerateFiringModeBurstForRangedWeaponXMLsString(int numberOfRounds, float burstingReload)
        {
            hasCalledGenerateFiringModeBurstForRangedWeaponXMLsString = true;

            if (numberOfRounds < 2 || numberOfRounds > 5) { throw new Exception($@"Argument {nameof(numberOfRounds)} should be between 2 and 5."); }

            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine($@"<!-- [FiringModeBurst] The number of shots fired during the gun's burst process is recorded, and a firing cooldown is applied. -->");

            int reloadInTicks = (int)Math.Round(burstingReload * 60, 0);
            int keepBurstingDuration = reloadInTicks * 2 - 1;

            while (--numberOfRounds > 0)
            {
                if (numberOfRounds > 1)
                {
                    stringBuilder.AppendLine(
$@"<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""RangedWeapon"" tags=""firingmodeburst_{numberOfRounds},firingmodeburst_active"" duration=""{GetTickDurationString(keepBurstingDuration)}"">
    <Conditional targetitemcomponent=""RangedWeapon"" hasstatustag=""firingmodeburst_{numberOfRounds - 1}"" />
</StatusEffect>");
                }
                else
                {
                    stringBuilder.AppendLine(
$@"<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""RangedWeapon"" tags=""firingmodeburst_{numberOfRounds},firingmodeburst_active"" duration=""{GetTickDurationString(keepBurstingDuration)}"">
    <Conditional targetitemcomponent=""RangedWeapon"" hasstatustag=""firingmodeburst_ready"" />
</StatusEffect>");
                }
            }

            stringBuilder.AppendLine($@"<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""RangedWeapon"" tags=""firingmodeburst_reloading"" duration=""{GetTickDurationString(reloadInTicks)}"" />");

            return stringBuilder.ToString();
        }
    }
}

