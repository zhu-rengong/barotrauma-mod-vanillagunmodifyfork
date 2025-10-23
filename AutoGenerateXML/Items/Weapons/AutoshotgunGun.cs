using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static AutoGenerateXML.ItemXMLExtensions;

namespace AutoGenerateXML.Items.Weapons
{
    public class Autoshotgun : GunXMLGenerator
    {
        public Autoshotgun() : base()
        {
            Name = "Autoshotgun";
            OutputPath = Path.Combine("Guns", $@"{Name}.xml");
            Identifier = Identifiers.VGM_Autoshotgun;

            SelfTags.AddRange(["mediumitem", "gunsmith", Name]);

            LowerAccessorySlotIndex = 2;
            MuzzleSlotIndex = 3;
            UpperAccessorySlotIndex = 4;
            ScannerSlotIndex = 5;

            Scale = 0.62f;
        }

        public override float? HoldAngle => -20;

        public override float[] BarrelPos => [74, 8];
        public override float WeaponDamageModifier => 0.9f;
        public override float Penetration => 0.0f;
        public override float RequiredWeaponsSkill => 75;

        public override float Reload => 0.45f;
        public override float CombatPriority => 80;
        public override float MinimumSpread => 5;
        public override float MinimumUnskilledSpread => 18;
        public override float SpreadChangesOnAimDownSight => 7;
        public override float SpreadRecovery => 1.0f;
        public override float SpreadChangesOnShoot => 10;
        public override float SpreadLimit => 18.0f;
        public override float Recoil => 300;
        public override float StocklessSpeedMultiplier => 0.9f;

        public override List<ContainableGrip> ContainableGrips => [
            new(GripXMLGenerator.All[Identifiers.VGM_AngledForeGrip], ItemPos: [30,-2]),
            new(GripXMLGenerator.All[Identifiers.VGM_VerticalGrip], ItemPos: [28,-8]),
            new(GripXMLGenerator.All[Identifiers.VGM_BipodGrip], ItemPos: [28,-15]),
        ];

        public override List<ContainableMuzzle> ContainableMuzzles => [
            new(MuzzleXMLGenerator.All[Identifiers.VGM_ChokeTubeMuzzle]),
        ];

        public override List<ContainableAimingDevice> ContainableAimingDevices => [
            new(AimingDeviceXMLGenerator.All[Identifiers.VGM_RedDotSight], ItemPos: [15,15]),
            new(AimingDeviceXMLGenerator.All[Identifiers.VGM_HolographicSight], ItemPos: [15,17]),
            new(AimingDeviceXMLGenerator.All[Identifiers.VGM_ACOGScope], ItemPos: [13,18]),
            new(AimingDeviceXMLGenerator.All[Identifiers.VGM_RifleScope], ItemPos: [12,14]),
            new(AimingDeviceXMLGenerator.All[Identifiers.VGM_SniperScope], ItemPos: [16,18]),
        ];

        public override List<ContainableScanner> ContainableScanners => [
            new(ScannerXMLGenerator.All[Identifiers.VGM_HealthScanner], ItemPos: [20,11]),
            new(ScannerXMLGenerator.All[Identifiers.VGM_ThermalScanner], ItemPos: [20,11]),
        ];

        public override string Generate()
        {
            string gunXmlString =
$@"
<Item {GenerateGunXMLAttributesString()} cargocontaineridentifier=""metalcrate"" impactsoundtag=""impact_metal_light"">
    {GenerateBasicInfoXMLsString(fabricateRequiredSkill: 55)}

    {GenerateItemSpriteXMLString(
        texture: "%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/Guns/Guns.png",
        sourceRect: [256, 0, 151, 61],
        depth: 0.52f,
        origin: [0.5f, 0.5f]
    )}

    {GenerateRectangleBodyXMLString(width: 147, height: 57, density: 25)}

    <Holdable {GenerateHoldableXMLAttributesString(
            slots: ["Any", "RightHand+LeftHand"],
            controlPos: true,
            holdPos: [36, -17],
            aimPos: [52, -5],
            handle1: [-14, -17],
            handle2: [7, -3]
        )}>

        {GenerateGunModifySpeedMultiplierXMLsString()}

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

    {GenerateStatusHUDXMLsString()}
    {GenerateAiTargetXMLsString()}

    <RangedWeapon {GenerateRangedWeaponXMLAttributesString()}>
        {GenerateDefaultCrosshairXMLsString()}

        {GenerateMuzzleFlashXMLsString()}

        {GenerateGunfireOnShootXMLsString(
            normalSoundFiles: [
                @"Content/Items/JobGear/Security/WEAPONS_autoShotgun.ogg",
                @"Content/Items/JobGear/Security/WEAPONS_autoShotgun_1.ogg",
                @"Content/Items/JobGear/Security/WEAPONS_autoShotgun_2.ogg",
                @"Content/Items/JobGear/Security/WEAPONS_autoShotgun_3.ogg",
            ]
        )}

        <StatusEffect type=""OnUse"" target=""This"">
            <ParticleEmitter particle=""casingfirearm"" colormultiplier=""0.5,0.5,0.5,1"" ScaleMultiplier=""1.5,1.5"" particleamount=""1"" anglemin=""90"" anglemax=""150"" velocitymin=""50"" velocitymax=""250"" CopyEntityAngle=""true"" />
        </StatusEffect>

        {GenerateGunSpreadChangesOnShootXMLsString()}
        {GenerateMuzzleModifySpreadChangesOnShootXMLsString()}

        {GenerateGunSimulatedRecoilXMLsString()}

        <RequiredItems items=""shotgunammo"" type=""Contained"" msg=""ItemMsgAmmoRequired"" />
        {GenerateMajorRequiredWeaponsSkillXMLsString()}
    </RangedWeapon>

    {GeneratePropulsionXMLsString()}

    <ItemContainer capacity=""2"" maxstacksize=""12"" hideitems=""false"" ShowTotalStackCapacityInContainedStateIndicator=""true"" containedstateindicatorslot=""0"" containedstateindicatorstyle=""bullet"">
        <Containable items=""shotgunammo"" hide=""true"">
            {GenerateMuzzleSpreadChokeXMLsString()}
        </Containable>

        {GenerateSlotIconBulletsXMLString(0)}
        {GenerateSlotIconBulletsXMLString(1)}
        {GenerateSlotIconFlashlightXMLString(2)}
        {GenerateSlotIconMuzzleXMLString(3)}
        {GenerateSlotIconAimingDeviceXMLString(4)}
        {GenerateSlotIconScannerXMLString(5)}

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateGripOnContainedXMLsString()}
            {GenerateContainableGenericAccessories(itemPos: [14, 5])}
        </SubContainer>

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateMuzzleOnContainedXMLsString()}
        </SubContainer>

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateAimingDeviceOnContainedXMLsString()}
            {GenerateContainableGenericAccessories(itemPos: [6, 5])}
        </SubContainer>

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateScannerOnContainedXMLsString()}
        </SubContainer>
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
