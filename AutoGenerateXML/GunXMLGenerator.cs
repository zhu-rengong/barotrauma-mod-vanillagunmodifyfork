

using AutoGenerateXML.Items.Accessories;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using static AutoGenerateXML.GunXMLGenerator;
using static AutoGenerateXML.ItemXMLExtensions;

namespace AutoGenerateXML
{
    public abstract partial class GunXMLGenerator : ItemXMLGenerator
    {
        public static Dictionary<string, GunXMLGenerator> All = new();

        protected GunXMLGenerator()
        {
            OutputPath = Path.Combine("Guns", "Guns.xml");
            Category = "Weapon";
            SelfTags.AddRange(["weapon", "gun", "provocativetohumanai", "mountableweapon"]);
        }

        public int LowerAccessorySlotIndex = -1;
        public int StockSlotIndex = -1;
        public int MuzzleSlotIndex = -1;
        public int UpperAccessorySlotIndex = -1;
        public int ScannerSlotIndex = -1;

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
        public virtual List<ContainableGrip>? ContainableGrips { get; } = null;
        public virtual List<ContainableStock>? ContainableStocks { get; } = null;
        public virtual List<ContainableMuzzle>? ContainableMuzzles { get; } = null;
        public virtual List<ContainableAimingDevice>? ContainableAimingDevices { get; } = null;
        public virtual List<ContainableScanner>? ContainableScanners { get; } = null;

        public virtual float NormalSoundRangeOnShoot => 3000;
        public virtual float SuppressedSoundRangeOnShoot => 800;
        public virtual float FlashHiddenSightRangeOnShoot => 500;

        public virtual float CrosshairScale => 0.15f;

        public bool CompatibleWithAnyMuzzleAttrSuppressor => ContainableMuzzles is not null
                && ContainableMuzzles.Any(v => v.Muzzle.IsSuppressor);

        public bool CompatibleWithAnyMuzzleAttrFlashHider => ContainableMuzzles is not null
                && ContainableMuzzles.Any(v => v.Muzzle.IsFlashHider);

        public bool CompatibleWithAnyMuzzleAttrOverrideSpreadChangesOnShoot => ContainableMuzzles is not null
                && ContainableMuzzles.Any(v => v.Muzzle.SpreadChangesOnShootMultiplier.HasValue);

        public bool CompatibleWithAnyMuzzleAttrChoke => ContainableMuzzles is not null
                && ContainableMuzzles.Any(v => v.Muzzle.SpreadChoke.HasValue);
        
        public bool CompatibleWithAnyMuzzleAttrShotAmountModification => ContainableMuzzles is not null
                && ContainableMuzzles.Any(v => v.Muzzle.ShotAmountModifierPerXShots is not null);

        public static readonly float MaxSpreadPerTickOnShoot = 1.0f;

        public bool VerifyPotentialErrors()
        {
            if (NotAllSame(hasCalledGenerateFiringModeBurstForHoldableXMLsString, hasCalledGenerateFiringModeBurstForRangedWeaponXMLsString))
            {
                ThrowError("The functionality of FiringModeBurst is incomplete.");
            }

            if ((ContainableScanners is not null
                || hasCalledGenerateScannerActivationXMLsString
                || hasCalledGenerateStatusHUDXMLsString
                || hasCalledGenerateScannerOnContainedXMLsString)
                && NotAllSame(
                    generatedHotTagWasAiming,
                    ContainableScanners is not null,
                    hasCalledGenerateScannerActivationXMLsString,
                    hasCalledGenerateStatusHUDXMLsString,
                    hasCalledGenerateScannerOnContainedXMLsString))
            {
                ThrowError("The functionality of Scanner is incomplete.");
            }

            if (NotAllSame(ContainableGrips is not null,
                hasCalledGenerateGripModifySpreadChangesOnAimDownSightXMLsString,
                hasCalledGenerateGripSpreadRecoveryXMLsString,
                hasCalledGenerateGripOnContainedXMLsString))
            {
                ThrowError("The functionality of Grip is incomplete.");
            }

            if (NotAllSame(ContainableStocks is not null,
                hasCalledGenerateStockModifySpeedMultiplierXMLsString,
                hasCalledGenerateStockSimulatedRecoilXMLsString,
                hasCalledGenerateStockOnContainedXMLsString))
            {
                ThrowError("The functionality of Stock is incomplete.");
            }

            if (NotAllSame(ContainableMuzzles is not null,
                hasCalledGenerateMuzzleModifySpreadChangesOnShootXMLsString,
                hasCalledGenerateMuzzleOnContainedXMLsString))
            {
                ThrowError("The functionality of Muzzle is incomplete.");
            }

            if (NotAllSame(hasCalledGenerateFlashHiderMuzzleOnShootXMLsString, CompatibleWithAnyMuzzleAttrFlashHider))
            {
                ThrowError("The functionality of Flash Hider is incomplete.");
            }

            if (NotAllSame(hasCalledGenerateMuzzleSpreadChokeXMLsString, CompatibleWithAnyMuzzleAttrChoke))
            {
                ThrowError("The functionality of Spread Choke is incomplete.");
            }
            
            if (NotAllSame(hasCalledGenerateMuzzleShotAmountModificationXMLsString, CompatibleWithAnyMuzzleAttrShotAmountModification))
            {
                ThrowError("The functionality of Shot amount modification is incomplete.");
            }

            if (NotAllSame(ContainableAimingDevices is not null,
                hasCalledGenerateAimingDeviceModifySpreadChangesOnAimDownSightXMLsString,
                hasCalledGenerateAimingDeviceSpreadRecoveryXMLsString,
                hasCalledGenerateAimingDeviceObstructVisionXMLsString,
                hasCalledGenerateAimingDeviceOnContainedXMLsString))
            {
                ThrowError("The functionality of Aiming Device is incomplete.");
            }

            if (ContainableGrips is not null && LowerAccessorySlotIndex < 0)
            {
                ThrowError("Grip is compatible but not define slot index for it.");
            }

            if (NotAllSame(ContainableStocks is not null, StockSlotIndex > -1))
            {
                ThrowError("Having compatible stocks defined and stock slot index defined must be both true or both false.");
            }

            if (NotAllSame(ContainableMuzzles is not null, MuzzleSlotIndex > -1))
            {
                ThrowError("Having compatible muzzles defined and muzzle slot index defined must be both true or both false.");
            }

            if (ContainableAimingDevices is not null && UpperAccessorySlotIndex < 0)
            {
                ThrowError("Aiming device is compatible but not define slot index for it.");
            }

            if (NotAllSame(ContainableScanners is not null, ScannerSlotIndex > -1))
            {
                ThrowError("Having compatible scanners defined and scanner slot index defined must be both true or both false.");
            }

            if (NotAllSame(ShotgunTuberExtenderSlotIndex > -1,
                hasCalledGenerateShotgunTubeExtenderFunctionalityAtParentHoldableXMLsString))
            {
                ThrowError("The functionality of Shotgun Tube Extender is incomplete.");
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
    <RequiredItem identifier=""{Name.ToLower(CultureInfo.InvariantCulture)}"" />
</Fabricate>";
        }

        public string GenerateSpawnOEMStockXMLsString()
        {
            if (ContainableStocks is not null && ContainableStocks.Any())
            {
                return
$@"<!-- [Stock] If the gun is initialized for the first time, it will come with an OEM stock. -->
<StatusEffect type=""OnSpawn"" target=""This"" statuseffecttags=""{StatusEffectTags.FirstInitialized}"" duration=""{GetTickDurationString(2)}"" evententitytag=""gun"">
    <Conditional HasBeenInstantiatedOnce=""false"" />
    <TriggerEvent>
        <ScriptedEvent identifier=""VGM_TrySpawn{Name}OEMStock"">
        <StatusEffectAction targettag=""gun"">
            <StatusEffect target=""This"">
                <SpawnItem identifier=""VGM_{Name}Stock"" spawnposition=""ThisInventory"" SpawnIfCantBeContained=""false"" SpawnIfInventoryFull=""false"" />
                <Conditional hasstatustag=""{StatusEffectTags.FirstInitialized}"" />
                <RequiredItem tag=""fabricator"" type=""Container"" />
            </StatusEffect>
        </StatusEffectAction>
        </ScriptedEvent>
    </TriggerEvent>
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
$@"name=""VGM {Name}""
identifier=""{Identifier}""
category=""{Category}""
subcategory=""{SubCategory}""
tags=""{ConcatValues(SelfTags)}""
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

            if (ContainableStocks is not null)
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

            if (ContainableStocks is null) { throw new NullReferenceException($@"Unable to generate stock code because '{Name}' has no stock defined."); }

            StringBuilder stringBuilder = new();

            bool hasModifier = false;

            ContainableStocks.ForEach(containable =>
            {
                if (containable.Stock.StocklessBasedSpeedMultiplier.HasValue)
                {
                    float speedMultiplier = StocklessSpeedMultiplier * containable.Stock.StocklessBasedSpeedMultiplier.Value;
                    stringBuilder.AppendLine(
$@"<StatusEffect type=""OnActive"" target=""Character"" speedmultiplier=""{speedMultiplier}"" setvalue=""true"">
    <RequiredItem identifier=""{containable.Stock.Identifier}"" type=""Contained"" targetslot=""{StockSlotIndex}"" />
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

            if (ContainableGrips is null) { throw new NullReferenceException($@"Unable to generate grip code because '{Name}' has no grip defined."); }

            StringBuilder stringBuilder = new();

            bool hasModifier = false;

            ContainableGrips.ForEach(containable =>
            {
                if (!containable.Grip.SpreadChangesOnAimDownSightMultiplier.HasValue) { return; }

                float spreadModifierOnADS = SpreadChangesOnAimDownSight * (containable.Grip.SpreadChangesOnAimDownSightMultiplier.Value - 1.0f);

                stringBuilder.AppendLine(
$@"<StatusEffect type=""OnSecondaryUse"" target=""This"" targetitemcomponent=""RangedWeapon"" spread=""{spreadModifierOnADS}"" unskilledspread=""{spreadModifierOnADS}"" disabledeltatime=""true"">
    <Conditional hasstatustag=""! {StatusEffectTags.PreventSpreadingOnADS}"" />
    <RequiredItem identifier=""{containable.Grip.Identifier}"" type=""Contained"" targetslot=""{LowerAccessorySlotIndex}"" />
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

            if (ContainableAimingDevices is null) { throw new NullReferenceException($@"Unable to generate aiming device code because '{Name}' has no aiming device defined."); }

            StringBuilder stringBuilder = new();

            bool hasModifier = false;

            ContainableAimingDevices.ForEach(containable =>
            {
                if (!containable.AimingDevice.SpreadChangesOnAimDownSightMultiplier.HasValue) { return; }

                float spreadModifierOnADS = SpreadChangesOnAimDownSight * (containable.AimingDevice.SpreadChangesOnAimDownSightMultiplier.Value - 1.0f);

                stringBuilder.AppendLine(
$@"<StatusEffect type=""OnSecondaryUse"" target=""This"" targetitemcomponent=""RangedWeapon"" spread=""{spreadModifierOnADS}"" unskilledspread=""{spreadModifierOnADS}"" disabledeltatime=""true"">
    <Conditional hasstatustag=""! {StatusEffectTags.PreventSpreadingOnADS}"" />
    <RequiredItem identifier=""{containable.AimingDevice.Identifier}"" type=""Contained"" targetslot=""{UpperAccessorySlotIndex}"" />
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
<StatusEffect type=""OnSecondaryUse"" target=""This"" targetitemcomponent=""RangedWeapon"" statuseffecttags=""{StatusEffectTags.AllowSpreadRecovery}"" duration=""{GetTickDurationString(1)}"">
    <Conditional targetitemcomponent=""RangedWeapon"" spread=""gt {MinimumSpread + SpreadRecovery / 2}"" />
</StatusEffect>
<StatusEffect type=""OnSecondaryUse"" target=""This"" targetitemcomponent=""RangedWeapon"" spread=""{-SpreadRecovery}"" disabledeltatime=""true"">
    <Conditional targetitemcomponent=""RangedWeapon"" hasstatustag=""{StatusEffectTags.AllowSpreadRecovery}"" />
</StatusEffect>
<StatusEffect type=""OnSecondaryUse"" target=""This"" targetitemcomponent=""RangedWeapon"" statuseffecttags=""{StatusEffectTags.AllowUnskilledSpreadRecovery}"" duration=""{GetTickDurationString(1)}"">
    <Conditional targetitemcomponent=""RangedWeapon"" unskilledspread=""gt {MinimumUnskilledSpread + SpreadRecovery / 2}"" />
</StatusEffect>
<StatusEffect type=""OnSecondaryUse"" target=""This"" targetitemcomponent=""RangedWeapon"" unskilledspread=""{-SpreadRecovery}"" disabledeltatime=""true"">
    <Conditional targetitemcomponent=""RangedWeapon"" hasstatustag=""{StatusEffectTags.AllowUnskilledSpreadRecovery}"" />
</StatusEffect>";
        }

        private bool hasCalledGenerateGripSpreadRecoveryXMLsString = false;
        public string GenerateGripSpreadRecoveryXMLsString()
        {
            hasCalledGenerateGripSpreadRecoveryXMLsString = true;

            if (ContainableGrips is null) { throw new NullReferenceException($@"Unable to generate grip code because '{Name}' has no grip defined."); }

            StringBuilder stringBuilder = new();

            bool hasModifier = false;

            ContainableGrips.ForEach(containable =>
            {
                if (!containable.Grip.SpreadRecoveryMultiplier.HasValue) { return; }

                float extraSpreadRecovery = SpreadRecovery * (containable.Grip.SpreadRecoveryMultiplier.Value - 1.0f);
                float spreadRecoveryThreshold = MinimumSpread;
                if (containable.Grip.MinimumSpreadOnRecoveringMultiplier.HasValue) { spreadRecoveryThreshold *= containable.Grip.MinimumSpreadOnRecoveringMultiplier.Value; }
                float neutralSpreadModifier = spreadRecoveryThreshold - MinimumSpread;

                stringBuilder.AppendLine(
$@"<StatusEffect type=""OnSecondaryUse"" target=""This"" targetitemcomponent=""RangedWeapon"" statuseffecttags=""{StatusEffectTags.AllowSpreadRecovery}"" duration=""{GetTickDurationString(1)}"">
    {(extraSpreadRecovery > 0.0f
        ? $@"<Conditional targetitemcomponent=""RangedWeapon"" spread=""gt {MinimumSpread + neutralSpreadModifier + extraSpreadRecovery / 2}"" />"
        : $@"<Conditional targetitemcomponent=""RangedWeapon"" spread=""gt {MinimumSpread + neutralSpreadModifier + SpreadRecovery / 2}"" />"
    )}
    <RequiredItem identifier=""{containable.Grip.Identifier}"" type=""Contained"" targetslot=""{LowerAccessorySlotIndex}"" />
</StatusEffect>
<StatusEffect type=""OnSecondaryUse"" target=""This"" targetitemcomponent=""RangedWeapon"" spread=""{-extraSpreadRecovery}"" disabledeltatime=""true"">
    <Conditional targetitemcomponent=""RangedWeapon"" hasstatustag=""{StatusEffectTags.AllowSpreadRecovery}"" />
    <RequiredItem identifier=""{containable.Grip.Identifier}"" type=""Contained"" targetslot=""{LowerAccessorySlotIndex}"" />
</StatusEffect>
<StatusEffect type=""OnSecondaryUse"" target=""This"" targetitemcomponent=""RangedWeapon"" statuseffecttags=""{StatusEffectTags.AllowUnskilledSpreadRecovery}"" duration=""{GetTickDurationString(1)}"">
    {(extraSpreadRecovery > 0.0f
        ? $@"<Conditional targetitemcomponent=""RangedWeapon"" unskilledspread=""gt {MinimumUnskilledSpread + neutralSpreadModifier + extraSpreadRecovery / 2}"" />"
        : $@"<Conditional targetitemcomponent=""RangedWeapon"" unskilledspread=""gt {MinimumUnskilledSpread + neutralSpreadModifier + SpreadRecovery / 2}"" />"
    )}
    <RequiredItem identifier=""{containable.Grip.Identifier}"" type=""Contained"" targetslot=""{LowerAccessorySlotIndex}"" />
</StatusEffect>
<StatusEffect type=""OnSecondaryUse"" target=""This"" targetitemcomponent=""RangedWeapon"" unskilledspread=""{-extraSpreadRecovery}"" disabledeltatime=""true"">
    <Conditional targetitemcomponent=""RangedWeapon"" hasstatustag=""{StatusEffectTags.AllowUnskilledSpreadRecovery}"" />
    <RequiredItem identifier=""{containable.Grip.Identifier}"" type=""Contained"" targetslot=""{LowerAccessorySlotIndex}"" />
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

            if (ContainableAimingDevices is null) { throw new NullReferenceException($@"Unable to generate aiming device code because '{Name}' has no aiming device defined."); }

            StringBuilder stringBuilder = new();

            bool hasModifier = false;

            ContainableAimingDevices.ForEach(containable =>
            {
                if (!containable.AimingDevice.SpreadRecoveryMultiplier.HasValue) { return; }

                float extraSpreadRecovery = SpreadRecovery * (containable.AimingDevice.SpreadRecoveryMultiplier.Value - 1.0f);
                float spreadRecoveryThreshold = MinimumSpread;
                if (containable.AimingDevice.MinimumSpreadOnRecoveringMultiplier.HasValue) { spreadRecoveryThreshold *= containable.AimingDevice.MinimumSpreadOnRecoveringMultiplier.Value; }
                float neutralSpreadModifier = spreadRecoveryThreshold - MinimumSpread;

                stringBuilder.AppendLine(
$@"<StatusEffect type=""OnSecondaryUse"" target=""This"" targetitemcomponent=""RangedWeapon"" statuseffecttags=""{StatusEffectTags.AllowSpreadRecovery}"" duration=""{GetTickDurationString(1)}"">
    {(extraSpreadRecovery > 0.0f
        ? $@"<Conditional targetitemcomponent=""RangedWeapon"" spread=""gt {MinimumSpread + neutralSpreadModifier + extraSpreadRecovery / 2}"" />"
        : $@"<Conditional targetitemcomponent=""RangedWeapon"" spread=""gt {MinimumSpread + neutralSpreadModifier + SpreadRecovery / 2}"" />"
    )}
    <RequiredItem identifier=""{containable.AimingDevice.Identifier}"" type=""Contained"" targetslot=""{UpperAccessorySlotIndex}"" />
</StatusEffect>
<StatusEffect type=""OnSecondaryUse"" target=""This"" targetitemcomponent=""RangedWeapon"" spread=""{-extraSpreadRecovery}"" disabledeltatime=""true"">
    <Conditional targetitemcomponent=""RangedWeapon"" hasstatustag=""{StatusEffectTags.AllowSpreadRecovery}"" />
    <RequiredItem identifier=""{containable.AimingDevice.Identifier}"" type=""Contained"" targetslot=""{UpperAccessorySlotIndex}"" />
</StatusEffect>
<StatusEffect type=""OnSecondaryUse"" target=""This"" targetitemcomponent=""RangedWeapon"" statuseffecttags=""{StatusEffectTags.AllowUnskilledSpreadRecovery}"" duration=""{GetTickDurationString(1)}"">
    {(extraSpreadRecovery > 0.0f
        ? $@"<Conditional targetitemcomponent=""RangedWeapon"" unskilledspread=""gt {MinimumUnskilledSpread + neutralSpreadModifier + extraSpreadRecovery / 2}"" />"
        : $@"<Conditional targetitemcomponent=""RangedWeapon"" unskilledspread=""gt {MinimumUnskilledSpread + neutralSpreadModifier + SpreadRecovery / 2}"" />"
    )}
    <RequiredItem identifier=""{containable.AimingDevice.Identifier}"" type=""Contained"" targetslot=""{UpperAccessorySlotIndex}"" />
</StatusEffect>
<StatusEffect type=""OnSecondaryUse"" target=""This"" targetitemcomponent=""RangedWeapon"" unskilledspread=""{-extraSpreadRecovery}"" disabledeltatime=""true"">
    <Conditional targetitemcomponent=""RangedWeapon"" hasstatustag=""{StatusEffectTags.AllowUnskilledSpreadRecovery}"" />
    <RequiredItem identifier=""{containable.AimingDevice.Identifier}"" type=""Contained"" targetslot=""{UpperAccessorySlotIndex}"" />
</StatusEffect>");

                hasModifier = true;
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

            if (ContainableAimingDevices is null) { throw new NullReferenceException($@"Unable to generate aiming device code because '{Name}' has no aiming device defined."); }

            StringBuilder stringBuilder = new();

            bool hasModifier = false;

            ContainableAimingDevices.ForEach(containable =>
            {
                if (containable.AimingDevice.ObstructVisionAmount.HasValue)
                {
                    stringBuilder.AppendLine(
$@"<StatusEffect type=""OnSecondaryUse"" target=""Character"" obstructvisionamount=""{containable.AimingDevice.ObstructVisionAmount.Value}"" setvalue=""true"" comparison=""And"">
    <Conditional islocalplayer=""true"" obstructvisionamount=""lt {containable.AimingDevice.ObstructVisionAmount.Value}""/>
    <RequiredItem identifier=""{containable.AimingDevice.Identifier}"" type=""Contained"" targetslot=""{UpperAccessorySlotIndex}"" />
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
    <RequiredItem tag=""{Tags.VGM_Scanner}"" type=""Contained"" targetslot=""{ScannerSlotIndex}"" />
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

        public string GenerateMuzzleFlashXMLsString(string particle = "muzzleflash", int amount = 6, float[]? scale = null, float[]? color = null)
        {
            scale ??= [0.7f, 1.4f];
            color ??= [0.95f, 1.00f, 0.65f, 0.34f];

            StringBuilder stringBuilder = new();

            stringBuilder.AppendLine($@"<!-- [Muzzle] Flash when fire -->");

            float emitDistance = BarrelPos[0] * Scale;

            stringBuilder.AppendLine(
$@"<StatusEffect type=""OnUse"" target=""This"" offset=""0,{BarrelPos[1] * Scale}"">
    <ParticleEmitter particle=""{particle}"" particleamount=""{amount}"" scalemin=""{scale[0]}"" scalemax=""{scale[1]}"" colormultiplier=""{ConcatValues(color)}""
        copyentityangle=""true"" distancemin=""{emitDistance}"" distancemax=""{emitDistance}"" />
    {(ContainableMuzzles is not null
        ? $@"<RequiredItem tag=""{Tags.VGM_Accessory}"" excludedtag=""{Tags.VGM_Muzzle}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" matchonempty=""true"" />"
        : string.Empty)}
</StatusEffect>");

            if (ContainableMuzzles is not null)
            {
                ContainableMuzzles.ForEach(containable =>
                {
                    float emitDistance = containable.Muzzle.BarrelLength * containable.Muzzle.Scale / 2;

                    stringBuilder.AppendLine(
$@"<StatusEffect type=""OnUse"" target=""Contained"" targetslot=""{MuzzleSlotIndex}"">
    <ParticleEmitter particle=""{(!string.IsNullOrEmpty(containable.Muzzle.FlashOverrideParticle) ? containable.Muzzle.FlashOverrideParticle : particle)}"" particleamount=""{amount}""
        scalemin=""{scale[0]}"" scalemax=""{scale[1]}""
        {(containable.Muzzle.FlashScaleMultiplier is not null ? $@"scalemultiplier=""{ConcatValues(containable.Muzzle.FlashScaleMultiplier)}""" : string.Empty)}
        colormultiplier=""{(ConcatValues(containable.Muzzle.FlashAlphaMultiplier.HasValue ? [color[0], color[1], color[2], color[3] * containable.Muzzle.FlashAlphaMultiplier.Value] : color))}""
        copyentityangle=""true"" distancemin=""{emitDistance}"" distancemax=""{emitDistance}"" />
    <RequiredItem identifier=""{containable.Muzzle.Identifier}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" />
</StatusEffect>");
                });
            }

            return stringBuilder.ToString();
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
    {(CompatibleWithAnyMuzzleAttrSuppressor
        ? $@"<RequiredItem tag=""{Tags.VGM_Accessory}"" excludedtag=""{Tags.VGM_MuzzleAttrSuppressor}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" matchonempty=""true"" />"
        : string.Empty)}
</StatusEffect>");

            if (CompatibleWithAnyMuzzleAttrSuppressor)
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
    {(CompatibleWithAnyMuzzleAttrOverrideSpreadChangesOnShoot
        ? $@"<RequiredItem tag=""{Tags.VGM_Accessory}"" excludedtag=""{Tags.VGM_MuzzleAttrOverrideSpreadChangesOnShoot}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" matchonempty=""true"" />"
        : string.Empty)}
</StatusEffect>
<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""RangedWeapon"" spread=""{spreadPerTick}"" stackable=""true"" duration=""{GetTickDurationString(ticks)}"" disabledeltatime=""true"" checkconditionalalways=""true"">
    <Conditional targetitemcomponent=""RangedWeapon"" spread=""lt {MinimumSpread + SpreadLimit - spreadPerTick / 2}"" />
    {(CompatibleWithAnyMuzzleAttrOverrideSpreadChangesOnShoot
        ? $@"<RequiredItem tag=""{Tags.VGM_Accessory}"" excludedtag=""{Tags.VGM_MuzzleAttrOverrideSpreadChangesOnShoot}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" matchonempty=""true"" />"
        : string.Empty)}
</StatusEffect>
<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""RangedWeapon"" unskilledspread=""{instantSpread}"" disabledeltatime=""true"">
    {(CompatibleWithAnyMuzzleAttrOverrideSpreadChangesOnShoot
        ? $@"<RequiredItem tag=""{Tags.VGM_Accessory}"" excludedtag=""{Tags.VGM_MuzzleAttrOverrideSpreadChangesOnShoot}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" matchonempty=""true"" />"
        : string.Empty)}
</StatusEffect>
<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""RangedWeapon"" unskilledspread=""{spreadPerTick}"" stackable=""true"" duration=""{GetTickDurationString(ticks)}"" disabledeltatime=""true"" checkconditionalalways=""true"">
    <Conditional targetitemcomponent=""RangedWeapon"" unskilledspread=""lt {MinimumUnskilledSpread + SpreadLimit - spreadPerTick / 2}"" />
    {(CompatibleWithAnyMuzzleAttrOverrideSpreadChangesOnShoot
        ? $@"<RequiredItem tag=""{Tags.VGM_Accessory}"" excludedtag=""{Tags.VGM_MuzzleAttrOverrideSpreadChangesOnShoot}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" matchonempty=""true"" />"
        : string.Empty)}
</StatusEffect>");

            return stringBuilder.ToString();
        }

        private bool hasCalledGenerateMuzzleModifySpreadChangesOnShootXMLsString = false;
        public string GenerateMuzzleModifySpreadChangesOnShootXMLsString()
        {
            hasCalledGenerateMuzzleModifySpreadChangesOnShootXMLsString = true;

            if (ContainableMuzzles is null) { throw new NullReferenceException($@"Unable to generate muzzle code because '{Name}' has no muzzle defined."); }

            StringBuilder stringBuilder = new();

            bool hasModifier = false;

            ContainableMuzzles.ForEach(containable =>
            {
                if (containable.Muzzle.SpreadChangesOnShootMultiplier.HasValue)
                {
                    float spreadChangesOnShoot = SpreadChangesOnShoot * containable.Muzzle.SpreadChangesOnShootMultiplier.Value;

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
    <RequiredItem identifier=""{containable.Muzzle.Identifier}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" />
</StatusEffect>
<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""RangedWeapon"" spread=""{spreadPerTick}"" stackable=""true"" duration=""{GetTickDurationString(ticks)}"" disabledeltatime=""true"" checkconditionalalways=""true"">
    <Conditional targetitemcomponent=""RangedWeapon"" spread=""lt {MinimumSpread + SpreadLimit - spreadPerTick / 2}"" />
    <RequiredItem identifier=""{containable.Muzzle.Identifier}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" />
</StatusEffect>
<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""RangedWeapon"" unskilledspread=""{instantSpread}"" disabledeltatime=""true"">
    <RequiredItem identifier=""{containable.Muzzle.Identifier}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" />
</StatusEffect>
<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""RangedWeapon"" unskilledspread=""{spreadPerTick}"" stackable=""true"" duration=""{GetTickDurationString(ticks)}"" disabledeltatime=""true"" checkconditionalalways=""true"">
    <Conditional targetitemcomponent=""RangedWeapon"" unskilledspread=""lt {MinimumUnskilledSpread + SpreadLimit - spreadPerTick / 2}"" />
    <RequiredItem identifier=""{containable.Muzzle.Identifier}"" type=""Contained"" targetslot=""{MuzzleSlotIndex}"" />
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

            if (ContainableStocks is not null)
            {
                stringBuilder.AppendLine(
$@"<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""Propulsion"" force=""{-Recoil}"" setvalue=""true"">
    <RequiredItem tag=""{Tags.VGM_Accessory}"" excludedtag=""{Tags.VGM_Stock}"" type=""Contained"" targetslot=""{StockSlotIndex}"" matchonempty=""true"" />
</StatusEffect>
<StatusEffect type=""OnUse"" target=""Character"" camerashake=""{Recoil * StockXMLGenerator.CameraShakePerUnitRecoil}"" setvalue=""true"">
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
<StatusEffect type=""OnUse"" target=""Character"" camerashake=""{Recoil * StockXMLGenerator.CameraShakePerUnitRecoil}"" setvalue=""true"" />
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

            if (ContainableStocks is null) { throw new NullReferenceException($@"Unable to generate stock code because '{Name}' has no stock defined."); }

            StringBuilder stringBuilder = new();

            bool hasModifier = false;

            ContainableStocks.ForEach(containable =>
            {
                if (!containable.Stock.RecoilReduction.HasValue) { return; }

                float recoil = MathF.Max(0.0f, Recoil - containable.Stock.RecoilReduction.Value * StockRecoilReductionEfficiency);
                float cameraShake = recoil * StockXMLGenerator.CameraShakePerUnitRecoil;
                float recoilFeel = recoil * recoilFeelMultiplier;
                float recoilFeelNoSkill = recoil * recoilFeelNoSkillMultiplier;

                stringBuilder.AppendLine(
$@"<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""Propulsion"" force=""{-recoil}"" setvalue=""true"">
    <RequiredItem identifier=""{containable.Stock.Identifier}"" type=""Contained"" targetslot=""{StockSlotIndex}"" />
</StatusEffect>
<StatusEffect type=""OnUse"" target=""Character"" camerashake=""{recoil * StockXMLGenerator.CameraShakePerUnitRecoil}"" setvalue=""true"">
    <RequiredItem identifier=""{containable.Stock.Identifier}"" type=""Contained"" targetslot=""{StockSlotIndex}"" />
</StatusEffect>
{(recoilFeel >= recoilFeelCausesBlunttraumaThresholdMin
? $@"<StatusEffect type=""OnUse"" target=""Character"" targetlimbs=""RightArm,LeftArm"" disabledeltatime=""true"">
    {GenerateRecoilFeelCausesBlunttraumaXMLsString(recoilFeel)}
    <Conditional skillrequirement=""true"" weapons=""gte {RequiredWeaponsSkill}"" />
    <RequiredItem identifier=""{containable.Stock.Identifier}"" type=""Contained"" targetslot=""{StockSlotIndex}"" />
</StatusEffect>"
: string.Empty)}
{(recoilFeelNoSkill >= recoilFeelCausesBlunttraumaThresholdMin
? $@"<StatusEffect type=""OnUse"" target=""Character"" targetlimbs=""RightArm,LeftArm"" disabledeltatime=""true"">
    {GenerateRecoilFeelCausesBlunttraumaXMLsString(recoilFeelNoSkill)}
    <Conditional skillrequirement=""true"" weapons=""lt {RequiredWeaponsSkill}"" />
    <RequiredItem identifier=""{containable.Stock.Identifier}"" type=""Contained"" targetslot=""{StockSlotIndex}"" />
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
        public record struct ContainableStock(StockXMLGenerator Stock, float[] ItemPos);
        public string GenerateStockOnContainedXMLsString()
        {
            hasCalledGenerateStockOnContainedXMLsString = true;

            if (ContainableStocks is null) { throw new NullReferenceException($@"Unable to generate stock code because '{Name}' has no stock defined."); }

            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine("<!-- [Stock] -->");

            ContainableStocks.ForEach(containable =>
            {
                stringBuilder.AppendLine($@"<Containable identifier=""{containable.Stock.Identifier}"" hide=""false"" itempos=""{ConcatValues(containable.ItemPos)}"" />");
            });

            stringBuilder.AppendLine($@"<Containable tag=""{Tags.VGM_Stock}Attr{Name}Compatible"" hide=""false"" />");

            return stringBuilder.ToString();
        }

        public string GenerateContainableGenericAccessories(float[] itemPos)
        {
            return
$@"<Containable items=""{Identifiers.VGM_RGBLaserPointer},flashlight,glowstick,flare,alienflare"" hide=""false"" itempos=""{ConcatValues(itemPos)}"" setactive=""true"" />";
        }

        private bool hasCalledGenerateGripOnContainedXMLsString = false;
        public record struct ContainableGrip(GripXMLGenerator Grip, float[] ItemPos);
        public string GenerateGripOnContainedXMLsString()
        {
            hasCalledGenerateGripOnContainedXMLsString = true;

            if (ContainableGrips is null) { throw new NullReferenceException($@"Unable to generate grip code because '{Name}' has no grip defined."); }

            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine(
$@"<!-- [Grip] Changes the gun's properties through the use of sub-items,
and sets the sub-item's condition to full on round loaded to prevent the accessory mod effects from being reset. -->");

            ContainableGrips.ForEach(containable =>
            {
                stringBuilder.AppendLine(
$@"<Containable identifier=""{containable.Grip.Identifier}"" hide=""false"" itempos=""{ConcatValues(containable.ItemPos)}"">
    {(containable.Grip.HoldAngle.HasValue
? $@"<StatusEffect type=""OnContaining"" target=""This"" targetitemcomponent=""Holdable"" holdangle=""{containable.Grip.HoldAngle.Value}"" setvalue=""true"" interval=""0.5"" />"
: string.Empty)}
</Containable>");
            });

            stringBuilder.AppendLine(
$@"<Containable tag=""{Tags.VGM_Grip}Attr{Name}Compatible"" hide=""false"">
    <StatusEffect type=""OnRemoved"" target=""This"" targetitemcomponent=""Holdable"" holdangle=""{HoldAngle}"" setvalue=""true"" />
</Containable>");

            return stringBuilder.ToString();
        }

        private bool hasCalledGenerateAimingDeviceOnContainedXMLsString = false;
        public record struct ContainableAimingDevice(AimingDeviceXMLGenerator AimingDevice, float[] ItemPos);
        public string GenerateAimingDeviceOnContainedXMLsString()
        {
            hasCalledGenerateAimingDeviceOnContainedXMLsString = true;

            if (ContainableAimingDevices is null) { throw new NullReferenceException($@"Unable to generate aiming device code because '{Name}' has no aiming device defined."); }

            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine($@"<!-- [AimingDevice] -->");

            ContainableAimingDevices.ForEach(containable =>
            {
                stringBuilder.AppendLine(
$@"<Containable identifier=""{containable.AimingDevice.Identifier}"" hide=""false"" itempos=""{ConcatValues(containable.ItemPos)}"">
    {(containable.AimingDevice.CameraAimOffset.HasValue
? $@"<StatusEffect type=""OnContaining"" target=""This"" targetitemcomponent=""Holdable"" cameraaimoffset=""{containable.AimingDevice.CameraAimOffset.Value}"" setvalue=""true"" interval=""0.5"" />
    <StatusEffect type=""OnContaining"" target=""This"" targetitemcomponent=""RangedWeapon"" crosshairscale=""{CrosshairScale * containable.AimingDevice.CameraAimOffset.Value / 240}"" setvalue=""true"" interval=""0.5"" />"
: string.Empty)}
</Containable>");
            });

            stringBuilder.AppendLine(
$@"<Containable tag=""{Tags.VGM_AimingDevice}Attr{Name}Compatible"" hide=""false"">
    <StatusEffect type=""OnRemoved"" target=""This"" targetitemcomponent=""Holdable"" cameraaimoffset=""0.0"" setvalue=""true"" />
    <StatusEffect type=""OnRemoved"" target=""This"" targetitemcomponent=""RangedWeapon"" crosshairscale=""{CrosshairScale}"" setvalue=""true"" />
</Containable>");

            return stringBuilder.ToString();
        }

        public record struct ContainableMuzzle
        {
            public MuzzleXMLGenerator Muzzle;
            public float?[] ItemPos;
            public float?[] BarrelPos;

            public ContainableMuzzle(MuzzleXMLGenerator muzzle, float?[]? itemPos = null, float?[]? barrelPos = null)
            {
                Muzzle = muzzle;
                ItemPos = itemPos ?? [null, null];
                BarrelPos = barrelPos ?? [null, null];
            }
        }

        private bool hasCalledGenerateMuzzleOnContainedXMLsString = false;
        public string GenerateMuzzleOnContainedXMLsString()
        {
            hasCalledGenerateMuzzleOnContainedXMLsString = true;

            if (ContainableMuzzles is null) { throw new NullReferenceException($@"Unable to generate muzzle code because '{Name}' has no muzzle defined."); }

            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine("<!-- [Muzzle] When inserted, modifies the gun's weapon damage (serializable) and continuously sets the gun's barrelpos (non-serializable). -->");

            ContainableMuzzles.ForEach(containable =>
            {
                if (containable.ItemPos is null) { containable.ItemPos = []; }
                if (containable.ItemPos.Length < 1 || !containable.ItemPos[0].HasValue) { containable.ItemPos[0] = MathF.Floor(BarrelPos[0] * Scale + containable.Muzzle.Scale * (containable.Muzzle.BarrelLength / 2 - containable.Muzzle.BarrelEmbeddedDepth)); }
                if (containable.ItemPos.Length < 2 || !containable.ItemPos[1].HasValue) { containable.ItemPos[1] = MathF.Round(BarrelPos[1] * Scale); }

                if (containable.BarrelPos is null) { containable.BarrelPos = []; }
                if (containable.BarrelPos.Length < 1 || !containable.BarrelPos[0].HasValue) { containable.BarrelPos[0] = MathF.Floor((BarrelPos[0] * Scale + containable.Muzzle.Scale * (containable.Muzzle.BarrelLength - containable.Muzzle.BarrelEmbeddedDepth)) / Scale); }
                if (containable.BarrelPos.Length < 2 || !containable.BarrelPos[1].HasValue) { containable.BarrelPos[1] = BarrelPos[1]; }

                stringBuilder.AppendLine(
$@"<Containable identifier=""{containable.Muzzle.Identifier}"" hide=""false"" itempos=""{ConcatValues(containable.ItemPos)}"">
    <StatusEffect type=""OnContaining"" target=""This"" targetitemcomponent=""RangedWeapon""
        barrelpos=""{ConcatValues(containable.BarrelPos)}""
        {(containable.Muzzle.WeaponDamageMultiplier.HasValue ? $@"weapondamagemodifier=""{containable.Muzzle.WeaponDamageMultiplier.Value * WeaponDamageModifier}""" : string.Empty)}
        {(containable.Muzzle.PenetrationModifier.HasValue ? $@"penetration=""{Penetration + containable.Muzzle.PenetrationModifier.Value}""" : string.Empty)}
        setvalue=""true"" />
</Containable>");

            });

            stringBuilder.AppendLine(
$@"<Containable tag=""{Tags.VGM_Muzzle}Attr{Name}Compatible"" hide=""false"">
    <StatusEffect type=""OnRemoved"" target=""This"" weapondamagemodifier=""{WeaponDamageModifier}"" penetration=""{Penetration}"" barrelpos=""{ConcatValues(BarrelPos)}"" setvalue=""true"" disabledeltatime=""true"" />
</Containable>");

            return stringBuilder.ToString();
        }

        private bool hasCalledGenerateScannerOnContainedXMLsString = false;
        public record struct ContainableScanner(ScannerXMLGenerator Scanner, float[] ItemPos);
        public string GenerateScannerOnContainedXMLsString()
        {
            hasCalledGenerateScannerOnContainedXMLsString = true;

            if (ContainableScanners is null) { throw new NullReferenceException($@"Unable to generate scanner code because '{Name}' has no scanner defined."); }

            StringBuilder stringBuilder = new();

            bool hasModifier = false;

            ContainableScanners.ForEach(containable =>
            {
                stringBuilder.AppendLine(
$@"<Containable identifier=""{containable.Scanner.Identifier}"" hide=""false"" itempos=""{ConcatValues(containable.ItemPos)}"">
    <StatusEffect type=""OnContaining"" target=""This"" targetitemcomponent=""StatusHUD"" 
        range=""{containable.Scanner.Range}""
        thermalgoggles=""{containable.Scanner.ThermalGoggles}""
        showdeadcharacters=""{containable.Scanner.ShowDeadCharacters}""
        showtexts=""{containable.Scanner.ShowTexts}""
        overlaycolor=""{ConcatValues(containable.Scanner.OverlayColor)}"" setvalue=""true"" interval=""0.5"" />
</Containable>");
                hasModifier = true;
            });

            if (hasModifier)
            {
                stringBuilder.Insert(0, "<!-- [Scanner] -->\n");
            }

            return stringBuilder.ToString();
        }

        private bool hasCalledGenerateFiringModeBurstForHoldableXMLsString = false;
        public string GenerateFiringModeBurstForHoldableXMLsString()
        {
            hasCalledGenerateFiringModeBurstForHoldableXMLsString = true;

            return
$@"<!-- [FiringModeBurst] When the gun is loaded and not in bursting state, it is ready  -->
<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""RangedWeapon"" tags=""FiringModeBurst_Ready"" duration=""{GetTickDurationString(1)}"" comparison=""And"">
    <Conditional targetitemcomponent=""RangedWeapon"" isactive=""false"" />
    <Conditional targetitemcomponent=""RangedWeapon"" hasstatustag=""! FiringModeBurst_Active"" />
</StatusEffect>
<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""RangedWeapon"" reloadtimer=""0.0"" isactive=""false"" setvalue=""true"" comparison=""And"">
    <Conditional targetitemcomponent=""RangedWeapon"" hasstatustag=""FiringModeBurst_Active"" />
    <Conditional targetitemcomponent=""RangedWeapon"" hasstatustag=""! FiringModeBurst_Reloading"" />
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
$@"<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""RangedWeapon"" tags=""FiringModeBurst_{numberOfRounds},FiringModeBurst_Active"" duration=""{GetTickDurationString(keepBurstingDuration)}"">
    <Conditional targetitemcomponent=""RangedWeapon"" hasstatustag=""FiringModeBurst_{numberOfRounds - 1}"" />
</StatusEffect>");
                }
                else
                {
                    stringBuilder.AppendLine(
$@"<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""RangedWeapon"" tags=""FiringModeBurst_{numberOfRounds},FiringModeBurst_Active"" duration=""{GetTickDurationString(keepBurstingDuration)}"">
    <Conditional targetitemcomponent=""RangedWeapon"" hasstatustag=""FiringModeBurst_Ready"" />
</StatusEffect>");
                }
            }

            stringBuilder.AppendLine($@"<StatusEffect type=""OnUse"" target=""This"" targetitemcomponent=""RangedWeapon"" tags=""FiringModeBurst_Reloading"" duration=""{GetTickDurationString(reloadInTicks)}"" />");

            return stringBuilder.ToString();
        }
    }
}

