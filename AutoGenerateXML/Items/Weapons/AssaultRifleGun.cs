using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static AutoGenerateXML.ItemXMLExtensions;

namespace AutoGenerateXML.Items.Weapons
{
    public class AssaultRifleGun : GunXMLGenerator
    {
        public AssaultRifleGun() : base()
        {
            Name = "AssaultRifle";
            OutputPath = Path.Combine("Guns", $@"{Name}.xml");
            Identifier = Identifiers.VGM_AssaultRifle;

            SelfTags.AddRange(["mediumitem", "gunsmith", Name]);

            LowerAccessorySlotIndex = 1;
            StockSlotIndex = 2;
            MuzzleSlotIndex = 3;
            UpperAccessorySlotIndex = 4;
            ScannerSlotIndex = 5;
        }

        public override float? HoldAngle => -30;

        public override float[] BarrelPos => [74, 18];
        public override float WeaponDamageModifier => 1.0f;
        public override float Penetration => 0.0f;
        public override float RequiredWeaponsSkill => 50;

        public override float Reload => 0.24f;
        public override float CombatPriority => 80;
        public override float MinimumSpread => 6;
        public override float MinimumUnskilledSpread => 20;
        public override float SpreadChangesOnAimDownSight => 17;
        public override float SpreadRecovery => 0.3f;
        public override float SpreadChangesOnShoot => 2.9f;
        public override float SpreadLimit => 10.0f;
        public override float Recoil => 350;
        public override float StockRecoilReductionEfficiency => 1.3f;
        public override float StocklessSpeedMultiplier => 0.95f;

        public override List<ContainableGrip> ContainableGrips => [
            new(GripXMLGenerator.All[Identifiers.VGM_AngledForeGrip], ItemPos: [19,-1]),
            new(GripXMLGenerator.All[Identifiers.VGM_VerticalGrip], ItemPos: [18,-2]),
            new(GripXMLGenerator.All[Identifiers.VGM_BipodGrip], ItemPos: [27,-9]),
        ];

        public override List<ContainableStock> ContainableStocks => [
            new(StockXMLGenerator.All[Identifiers.VGM_AssaultRifleStock], ItemPos: [-45,3]),
            new(StockXMLGenerator.All[Identifiers.VGM_HeavyStock], ItemPos: [-46,1]),
            new(StockXMLGenerator.All[Identifiers.VGM_HeavySniperStock], ItemPos: [-49,3]),
        ];

        public override List<ContainableMuzzle> ContainableMuzzles => [
            new(MuzzleXMLGenerator.All[Identifiers.VGM_SimpleSuppressorMuzzle]),
            new(MuzzleXMLGenerator.All[Identifiers.VGM_ShortSuppressorMuzzle]),
            new(MuzzleXMLGenerator.All[Identifiers.VGM_LongSuppressorMuzzle]),
            new(MuzzleXMLGenerator.All[Identifiers.VGM_FlashHiderMuzzle]),
            new(MuzzleXMLGenerator.All[Identifiers.VGM_LongBarrelMuzzle]),
        ];

        public override List<ContainableAimingDevice> ContainableAimingDevices => [
            new(AimingDeviceXMLGenerator.All[Identifiers.VGM_RedDotSight], ItemPos: [-14,17]),
            new(AimingDeviceXMLGenerator.All[Identifiers.VGM_HolographicSight], ItemPos: [-13,17]),
            new(AimingDeviceXMLGenerator.All[Identifiers.VGM_ACOGScope], ItemPos: [-17,20]),
            new(AimingDeviceXMLGenerator.All[Identifiers.VGM_RifleScope], ItemPos: [-14,16]),
            new(AimingDeviceXMLGenerator.All[Identifiers.VGM_SniperScope], ItemPos: [-14,20]),
        ];

        public override List<ContainableScanner> ContainableScanners => [
            new(ScannerXMLGenerator.All[Identifiers.VGM_HealthScanner], ItemPos: [1,12]),
            new(ScannerXMLGenerator.All[Identifiers.VGM_ThermalScanner], ItemPos: [1,12]),
        ];

        public override string Generate()
        {
            string gunXmlString =
$@"
<Item {GenerateGunXMLAttributesString()} cargocontaineridentifier=""metalcrate"" impactsoundtag=""impact_metal_light"">
    {GenerateBasicInfoXMLsString(fabricateRequiredSkill: 65)}

    {GenerateItemSpriteXMLString(
        texture: "%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/Guns/Guns.png",
        sourceRect: [0, 192, 158, 60],
        depth: 0.52f,
        origin: [0.5f, 0.5f]
    )}

    {GenerateRectangleBodyXMLString(width: 154, height: 56, density: 25)}

    <Holdable {GenerateHoldableXMLAttributesString(
            slots: ["RightHand+LeftHand"],
            controlPos: true,
            holdPos: [55, -22],
            aimPos: [70, -7],
            handle1: [-62, -6],
            handle2: [-4, 13]
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
      <sprite name=""VGM Assault Rifle Worn"" texture=""%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/Guns/Guns.png"" canbehiddenbyotherwearables=""false"" rotation=""90"" depth=""0.6"" sourcerect=""0,192,158,60"" limb=""Torso"" depthlimb=""LeftArm"" scale=""0.5"" origin=""0.5,0.8"" />
    </Wearable>

    {GenerateStatusHUDXMLsString()}
    {GenerateAiTargetXMLsString()}

    <RangedWeapon {GenerateRangedWeaponXMLAttributesString()}>
        {GenerateDefaultCrosshairXMLsString()}
        
        {GenerateMuzzleFlashXMLsString("impactfirearm", amount: 6, scale: [2.0f, 4.0f], color: [0.95f, 1.00f, 0.65f, 0.34f])}

        <StatusEffect type=""OnUse"" target=""This"">
          <ParticleEmitter particle=""casingfirearm"" particleamount=""1"" anglemin=""90"" anglemax=""150"" velocitymin=""50"" velocitymax=""250"" copyentityangle=""true"" />
        </StatusEffect>

        {GenerateGunfireOnShootXMLsString(
            normalSoundFiles: [
                @"Content/Items/JobGear/Security/WEAPONS_assaultRifle_1.ogg",
                @"Content/Items/JobGear/Security/WEAPONS_assaultRifle_2.ogg",
                @"Content/Items/JobGear/Security/WEAPONS_assaultRifle_3.ogg",
                @"Content/Items/JobGear/Security/WEAPONS_assaultRifle_4.ogg",
                @"Content/Items/JobGear/Security/WEAPONS_assaultRifle_5.ogg",
            ]
        )}

        {GenerateFlashHiderMuzzleOnShootXMLsString()}

        {GenerateGunSpreadChangesOnShootXMLsString()}
        {GenerateMuzzleModifySpreadChangesOnShootXMLsString()}

        {GenerateGunSimulatedRecoilXMLsString()}
        {GenerateStockSimulatedRecoilXMLsString()}

        <StatusEffect type=""OnUse"" target=""Contained"" targetslot=""0"">
            <Use />
        </StatusEffect>

        <RequiredItems items=""VGM_AssaultRifleAmmo,assaultrifleammo"" type=""Contained"" targetslot=""0"" msg=""ItemMsgAmmoRequired"" />
        {GenerateMajorRequiredWeaponsSkillXMLsString()}
    </RangedWeapon>

    {GeneratePropulsionXMLsString()}

    <ItemContainer capacity=""1"" maxstacksize=""1"" hideitems=""false"" containedstateindicatorslot=""0"" containedstateindicatorstyle=""bullet"">
        <Containable identifier=""VGM_AssaultRifleLongMagazine"" hide=""false"" itempos=""-7,-13"" />
        <Containable identifier=""VGM_AssaultRifleDrumMagazine"" hide=""false"" itempos=""-7,-13"" />
        <Containable tag=""assaultrifleammo"" hide=""false"" itempos=""-6,-9"" />
        {GenerateSlotIconBulletsXMLString(0)}
        {GenerateSlotIconFlashlightXMLString(1)}
        {GenerateSlotIconStockXMLString(2)}
        {GenerateSlotIconMuzzleXMLString(3)}
        {GenerateSlotIconAimingDeviceXMLString(4)}
        {GenerateSlotIconScannerXMLString(5)}

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateGripOnContainedXMLsString()}
            {GenerateContainableGenericAccessories(itemPos: [20, 4])}
        </SubContainer>

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateStockOnContainedXMLsString()}
        </SubContainer>

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateMuzzleOnContainedXMLsString()}
        </SubContainer>

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateAimingDeviceOnContainedXMLsString()}
            {GenerateContainableGenericAccessories(itemPos: [-12, 12])}
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
