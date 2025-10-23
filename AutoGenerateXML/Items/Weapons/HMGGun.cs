using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static AutoGenerateXML.ItemXMLExtensions;

namespace AutoGenerateXML.Items.Weapons
{
    public class HMGGun : GunXMLGenerator
    {
        public HMGGun() : base()
        {
            Name = "HMG";
            OutputPath = Path.Combine("Guns", $@"{Name}.xml");
            Identifier = Identifiers.VGM_HMG;

            SelfTags.AddRange(["mediumitem", "gunsmith", Name]);

            LowerAccessorySlotIndex = 1;
            StockSlotIndex = 2;
            MuzzleSlotIndex = 3;
            UpperAccessorySlotIndex = 4;
            ScannerSlotIndex = 5;
        }

        public override float? HoldAngle => -30;

        public override float[] BarrelPos => [87, 18];
        public override float WeaponDamageModifier => 1.0f;
        public override float Penetration => 0.0f;
        public override float RequiredWeaponsSkill => 90;

        public override float Reload => 0.1f;
        public override float CombatPriority => 80;
        public override float MinimumSpread => 8;
        public override float MinimumUnskilledSpread => 44;
        public override float SpreadChangesOnAimDownSight => 17;
        public override float SpreadRecovery => 0.3f;
        public override float SpreadChangesOnShoot => 2.9f;
        public override float SpreadLimit => 18.0f;
        public override float Recoil => 270;
        public override float StockRecoilReductionEfficiency => 0.75f;
        public override float StocklessSpeedMultiplier => 0.9f;

        public override List<ContainableGrip> ContainableGrips => [
            new(GripXMLGenerator.All[Identifiers.VGM_AngledForeGrip], ItemPos: [14,-2]),
            new(GripXMLGenerator.All[Identifiers.VGM_VerticalGrip], ItemPos: [14,-5]),
            new(GripXMLGenerator.All[Identifiers.VGM_BipodGrip], ItemPos: [32,-12]),
        ];

        public override List<ContainableStock> ContainableStocks => [
            new(StockXMLGenerator.All[Identifiers.VGM_HMGStock], ItemPos: [-48,2]),
            new(StockXMLGenerator.All[Identifiers.VGM_HeavyStock], ItemPos: [-47,2]),
            new(StockXMLGenerator.All[Identifiers.VGM_HeavySniperStock], ItemPos: [-52,3]),
        ];

        public override List<ContainableMuzzle> ContainableMuzzles => [
            new(MuzzleXMLGenerator.All[Identifiers.VGM_SimpleSuppressorMuzzle]),
            new(MuzzleXMLGenerator.All[Identifiers.VGM_ShortSuppressorMuzzle]),
            new(MuzzleXMLGenerator.All[Identifiers.VGM_LongSuppressorMuzzle]),
            new(MuzzleXMLGenerator.All[Identifiers.VGM_FlashHiderMuzzle]),
            new(MuzzleXMLGenerator.All[Identifiers.VGM_LongBarrelMuzzle]),
        ];

        public override List<ContainableAimingDevice> ContainableAimingDevices => [
            new(AimingDeviceXMLGenerator.All[Identifiers.VGM_RedDotSight], ItemPos: [-5,20]),
            new(AimingDeviceXMLGenerator.All[Identifiers.VGM_HolographicSight], ItemPos: [-3,22]),
            new(AimingDeviceXMLGenerator.All[Identifiers.VGM_ACOGScope], ItemPos: [-4,23]),
            new(AimingDeviceXMLGenerator.All[Identifiers.VGM_RifleScope], ItemPos: [-13,20]),
            new(AimingDeviceXMLGenerator.All[Identifiers.VGM_SniperScope], ItemPos: [1,23]),
        ];

        public override List<ContainableScanner> ContainableScanners => [
            new(ScannerXMLGenerator.All[Identifiers.VGM_HealthScanner], ItemPos: [8,16]),
            new(ScannerXMLGenerator.All[Identifiers.VGM_ThermalScanner], ItemPos: [8,16]),
        ];

        public override string Generate()
        {
            string gunXmlString =
$@"
<Item {GenerateGunXMLAttributesString()} cargocontaineridentifier=""metalcrate"" impactsoundtag=""impact_metal_light"">
    {GenerateBasicInfoXMLsString(fabricateRequiredSkill: 65)}

    {GenerateItemSpriteXMLString(
        texture: "%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/Guns/Guns.png",
        sourceRect: [0, 480, 178, 71],
        depth: 0.52f,
        origin: [0.5f, 0.5f]
    )}

    {GenerateRectangleBodyXMLString(width: 172, height: 67, density: 25)}

    <Holdable {GenerateHoldableXMLAttributesString(
            slots: ["RightHand+LeftHand"],
            controlPos: true,
            holdPos: [55, -22],
            aimPos: [70, -7],
            handle1: [-60, -10],
            handle2: [4, 12]
        )}>

        {GenerateGunModifySpeedMultiplierXMLsString()}
        {GenerateStockModifySpeedMultiplierXMLsString()}

        {GenerateGunSpreadChangesOnAimDownSightXMLsString()}
        {GenerateGripModifySpreadChangesOnAimDownSightXMLsString()}
        {GenerateAimingDeviceModifySpreadChangesOnAimDownSightXMLsString()}

        {GenerateGripSpreadRecoveryXMLsString()}
        {GenerateAimingDeviceSpreadRecoveryXMLsString()}
        {GenerateGunSpreadRecoveryXMLsString()}

        {GenerateAimingDeviceObstructVisionXMLsString()}
        
        {GenerateScannerActivationXMLsString()}

        {GenerateHotTagWasAimingXMLsString()}
        {GenerateHotTagPreventSpreadingOnADSXMLsString()}
    </Holdable>

    <Wearable slots=""Bag"" canbeselected=""false"" canbepicked=""true"" pickkey=""Select"" msg=""ItemMsgEquipSelect"">
      <sprite name=""VGM HMG Worn"" texture=""%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/Guns/Guns.png"" canbehiddenbyotherwearables=""false"" rotation=""90"" depth=""0.6"" sourcerect=""0,480,178,71"" limb=""Torso"" depthlimb=""LeftArm"" scale=""0.5"" origin=""0.5,0.8"" />
    </Wearable>

    {GenerateStatusHUDXMLsString()}
    {GenerateAiTargetXMLsString()}

    <RangedWeapon {GenerateRangedWeaponXMLAttributesString()}>
        {GenerateDefaultCrosshairXMLsString()}
        
        {GenerateMuzzleFlashXMLsString()}

        <StatusEffect type=""OnUse"" target=""This"">
          <ParticleEmitter particle=""casingfirearm"" particleamount=""1"" anglemin=""90"" anglemax=""150"" velocitymin=""50"" velocitymax=""250"" copyentityangle=""true"" />
        </StatusEffect>

        {GenerateGunfireOnShootXMLsString(
            normalSoundFiles: [
                @"Content/Items/JobGear/Security/WEAPON_hmgShot1.ogg",
                @"Content/Items/JobGear/Security/WEAPON_hmgShot2.ogg",
                @"Content/Items/JobGear/Security/WEAPON_hmgShot3.ogg",
                @"Content/Items/JobGear/Security/WEAPON_hmgShot4.ogg",
                @"Content/Items/JobGear/Security/WEAPON_hmgShot5.ogg",
                @"Content/Items/JobGear/Security/WEAPON_hmgShot6.ogg"
            ]
        )}

        {GenerateFlashHiderMuzzleOnShootXMLsString()}

        {GenerateGunSpreadChangesOnShootXMLsString()}
        {GenerateMuzzleModifySpreadChangesOnShootXMLsString()}

        {GenerateGunSimulatedRecoilXMLsString()}
        {GenerateStockSimulatedRecoilXMLsString()}

        <StatusEffect type=""OnUse"" target=""Character"" targetLimbs=""RightArm"" disabledeltatime=""true"" comparison=""And"">
            <Affliction identifier=""blunttrauma"" strength=""0.75"" />
            <Conditional skillrequirement=""true"" weapons=""lt {RequiredWeaponsSkill}"" />
            <Conditional recoilstabilized=""lte 0"" />
        </StatusEffect>
        <StatusEffect type=""OnUse"" target=""Character"" targetLimbs=""RightArm"" disabledeltatime=""true"" comparison=""And"">
            <Affliction identifier=""blunttrauma"" strength=""0.75"" />
            <Conditional skillrequirement=""true"" weapons=""lt {RequiredWeaponsSkill * 0.75f}"" />
            <Conditional recoilstabilized=""lte 0"" />
        </StatusEffect>
        <StatusEffect type=""OnUse"" target=""Character"" targetLimbs=""RightArm"" disabledeltatime=""true"" comparison=""And"">
            <Affliction identifier=""blunttrauma"" strength=""0.75"" />
            <Conditional skillrequirement=""true"" weapons=""lt {RequiredWeaponsSkill * 0.5f}"" />
            <Conditional recoilstabilized=""lte 0"" />
        </StatusEffect>

        <StatusEffect type=""OnUse"" target=""Contained"" targetslot=""0"">
            <Use />
        </StatusEffect>

        <RequiredItems items=""VGM_HMGAmmo,hmgammo"" type=""Contained"" targetslot=""0"" msg=""ItemMsgAmmoRequired"" />
        {GenerateMajorRequiredWeaponsSkillXMLsString()}
    </RangedWeapon>

    {GeneratePropulsionXMLsString()}

    <ItemContainer capacity=""1"" maxstacksize=""1"" hideitems=""false"" containedstateindicatorslot=""0"" containedstateindicatorstyle=""bullet"">
        <Containable identifier=""VGM_HMGDrumMagazine"" hide=""false"" itempos=""-9,-10"" />
        <Containable tag=""hmgmagazine"" hide=""false"" itempos=""-13,-10"" />
        {GenerateSlotIconBulletsXMLString(0)}
        {GenerateSlotIconFlashlightXMLString(1)}
        {GenerateSlotIconStockXMLString(2)}
        {GenerateSlotIconMuzzleXMLString(3)}
        {GenerateSlotIconAimingDeviceXMLString(4)}
        {GenerateSlotIconScannerXMLString(5)}

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateGripOnContainedXMLsString()}
            {GenerateContainableGenericAccessories(itemPos: [23, -1])}
        </SubContainer>

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateStockOnContainedXMLsString()}
        </SubContainer>

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateMuzzleOnContainedXMLsString()}
        </SubContainer>

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateAimingDeviceOnContainedXMLsString()}
            {GenerateContainableGenericAccessories(itemPos: [-1, 17])}
        </SubContainer>

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateScannerOnContainedXMLsString()}
        </SubContainer>

        {GenerateSpawnOEMStockXMLsString()}
    </ItemContainer>

    <Quality>
        <QualityStat stattype=""FirepowerMultiplier"" value=""0.1"" />
    </Quality>

    {GenerateMajorSkillRequirementHintXMLsString()}
</Item>
";

            return gunXmlString;
        }
    }
}
