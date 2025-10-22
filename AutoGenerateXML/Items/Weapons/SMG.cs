using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static AutoGenerateXML.ItemXMLExtensions;

namespace AutoGenerateXML.Items.Weapons
{
    public class SMG : GunXMLGenerator
    {
        public override string OutputPath => Path.Combine("Guns", $@"{GunName}.xml");

        public override string GunName => nameof(SMG);

        public override string Identifier => Identifiers.VGM_SMG;
        public override string SelfTags => "smallautoweapon,smallitem,weapon,gun,gunsmith,provocativetohumanai,mountableweapon,smg";

        public override int MuzzleSlotIndex => 2;
        public override int ScannerSlotIndex => 4;
        public override int UpperAccessorySlotIndex => 3;
        public override int LowerAccessorySlotIndex => 1;

        public override float? HoldAngle => -35;

        public override float[] BarrelPos => [71, 10];
        public override float WeaponDamageModifier => 1.3f;
        public override float Penetration => 0.15f;
        public override float RequiredWeaponsSkill => 50;

        public override float Reload => 0.19f;
        public override float CombatPriority => 80;
        public override float MinimumSpread => 10;
        public override float MinimumUnskilledSpread => 16;
        public override float SpreadChangesOnAimDownSight => 6;
        public override float SpreadRecovery => 0.3f;
        public override float SpreadChangesOnShoot => 6.5f;
        public override float SpreadLimit => 13.0f;
        public override float Recoil => 90;
        public override float StocklessSpeedMultiplier => 1.2f;

        public override ContainableGrip[] CompatibleGrips => [
            new (Identifiers.VGM_AngledForeGrip, ItemPos: [20, -5]),
            new (Identifiers.VGM_VerticalGrip, ItemPos: [20, -7]),
        ];

        public override ContainableMuzzle[] CompatibleMuzzles => [
            new(Identifiers.VGM_SimpleSuppressorMuzzle),
            new(Identifiers.VGM_ShortSuppressorMuzzle),
            new(Identifiers.VGM_FlashHiderMuzzle),
        ];

        public override ContainableAimingDevice[] CompatibleAimingDevices => [
            new(Identifiers.VGM_RedDotSight, ItemPos: [10,14]),
            new(Identifiers.VGM_HolographicSight, ItemPos: [11,16]),
            new(Identifiers.VGM_ACOGScope, ItemPos: [8,18]),
        ];

        public override ContainableScanner[] CompatibleScanners => [
            new(Identifiers.VGM_HealthScanner, ItemPos: [19,10]),
            new(Identifiers.VGM_ThermalScanner, ItemPos: [19,10]),
        ];

        public override string Generate()
        {
            string gunXmlString =
$@"
<Item {GenerateGunXMLAttributesString()} cargocontaineridentifier=""metalcrate"" impactsoundtag=""impact_metal_light"">
    {GenerateBasicInfoXMLsString(fabricateRequiredSkill: 55)}

    {GenerateItemSpriteXMLString(
        texture: "%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/Guns/Guns.png",
        sourceRect: [0, 64, 144, 52],
        depth: 0.52f,
        origin: [0.5f, 0.5f]
    )}

    {GenerateRectangleBodyXMLString(width: 140, height: 50, density: 25)}

    <Holdable {GenerateHoldableXMLAttributesString(
            slots: ["Any", "RightHand+LeftHand"],
            controlPos: true,
            holdPos: [40, -10],
            aimPos: [43, -9],
            handle1: [-26, -14],
            handle2: [21, -8]
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

        <StatusEffect type=""OnUse"" target=""This"">
          <ParticleEmitter particle=""casingfirearm"" particleamount=""1"" anglemin=""90"" anglemax=""150"" velocitymin=""50"" velocitymax=""250"" copyentityangle=""true"" />
        </StatusEffect>

        {GenerateGunfireOnShootXMLsString(
            normalSoundFiles: [
                @"Content/Items/Weapons/SMGsingleShot1.ogg",
                @"Content/Items/Weapons/SMGsingleShot2.ogg",
                @"Content/Items/Weapons/SMGsingleShot3.ogg",
                @"Content/Items/Weapons/SMGsingleShot4.ogg",
                @"Content/Items/Weapons/SMGsingleShot5.ogg",
                @"Content/Items/Weapons/SMGsingleShot6.ogg"
            ]
        )}

        {GenerateFlashHiderMuzzleOnShootXMLsString()}

        {GenerateGunSpreadChangesOnShootXMLsString()}
        {GenerateMuzzleModifySpreadChangesOnShootXMLsString()}

        {GenerateGunSimulatedRecoilXMLsString()}

        <StatusEffect type=""OnUse"" target=""Contained"" targetslot=""0"">
            <Use />
        </StatusEffect>

        <RequiredItems items=""VGM_SMGAmmo,smgammo"" type=""Contained"" targetslot=""0"" msg=""ItemMsgAmmoRequired"" />
        {GenerateMajorRequiredWeaponsSkillXMLsString()}
    </RangedWeapon>

    {GeneratePropulsionXMLsString()}

    <ItemContainer capacity=""1"" maxstacksize=""1"" hideitems=""false"" containedstateindicatorslot=""0"" containedstateindicatorstyle=""bullet"">
        <Containable identifier=""VGM_SMGLongMagazine"" hide=""false"" itempos=""4,-10"" rotation=""-30"" />
        <Containable identifier=""VGM_SMGDrumMagazine"" hide=""false"" itempos=""4,-10"" rotation=""-30"" />
        <Containable tag=""smgammo"" hide=""false"" itempos=""4,-10"" rotation=""-30"" />
        {GenerateSlotIconBulletsXMLString(0)}
        {GenerateSlotIconFlashlightXMLString(1)}
        {GenerateSlotIconMuzzleXMLString(2)}
        {GenerateSlotIconAimingDeviceXMLString(3)}
        {GenerateSlotIconScannerXMLString(4)}

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateGripOnContainedXMLsString()}
            {GenerateContainableGenericAccessories(itemPos: [20, 1])}
        </SubContainer>

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateMuzzleOnContainedXMLsString()}
        </SubContainer>

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateAimingDeviceOnContainedXMLsString()}
            {GenerateContainableGenericAccessories(itemPos: [10, 11])}
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
