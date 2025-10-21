using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static AutoGenerateXML.ItemXMLExtensions;

namespace AutoGenerateXML.Items.Weapons
{
    public class SMGUnique : GunXMLGenerator
    {
        public override string OutputPath => Path.Combine("Guns", $@"{GunName}.xml");

        public override string GunName => nameof(SMGUnique);

        public override string Identifier => Identifiers.VGM_SMGUnique;
        public override string SelfTags => "smallautoweapon,smallitem,weapon,gun,provocativetohumanai,mountableweapon,smgunique";

        public override int StockSlotIndex => 2;
        public override int MuzzleSlotIndex => 3;
        public override int ScannerSlotIndex => 5;
        public override int UpperAccessorySlotIndex => 4;
        public override int LowerAccessorySlotIndex => 1;

        public override float? HoldAngle => -35;

        public override float[] BarrelPos => [57, 18];
        public override float WeaponDamageModifier => 1.5f;
        public override float Penetration => 0.3f;
        public override float RequiredWeaponsSkill => 50;

        public override float Reload => 0.7f;
        public override float CombatPriority => 80;
        public override float MinimumSpread => 4;
        public override float MinimumUnskilledSpread => 12;
        public override float SpreadChangesOnAimDownSight => 8;
        public override float SpreadRecovery => 0.35f;
        public override float SpreadChangesOnShoot => 4.2f;
        public override float SpreadLimit => 11.0f;
        public override float Recoil => 120;
        public override float StockRecoilReductionEfficiency => 0.5f;
        public override float StocklessSpeedMultiplier => 1.1f;

        public override ContainableGrip[] CompatibleGrips => [
            new(Identifiers.VGM_AngledForeGrip, ItemPos: [10,-2]),
            new(Identifiers.VGM_VerticalGrip, ItemPos: [11,-4]),
        ];

        public override ContainableStock[] CompatibleStocks => [
            new(Identifiers.VGM_SMGUniqueStock, ItemPos: [-34,4]),
            new(Identifiers.VGM_LightStock, ItemPos: [-41,4]),
            new(Identifiers.VGM_LightSniperStock, ItemPos: [-39,5]),
            new(Identifiers.VGM_LightWrenchStock, ItemPos: [-39,4]),
        ];

        public override ContainableMuzzle[] CompatibleMuzzles => [
            new(Identifiers.VGM_SimpleSuppressorMuzzle),
            new(Identifiers.VGM_ShortSuppressorMuzzle),
            new(Identifiers.VGM_LongSuppressorMuzzle),
            new(Identifiers.VGM_FlashHiderMuzzle),
            new(Identifiers.VGM_LongBarrelMuzzle),
        ];

        public override ContainableAimingDevice[] CompatibleAimingDevices => [
            new(Identifiers.VGM_RedDotSight, ItemPos: [-13,16]),
            new(Identifiers.VGM_HolographicSight, ItemPos: [-9,17]),
            new(Identifiers.VGM_ACOGScope, ItemPos: [-13,19]),
            new(Identifiers.VGM_RifleScope, ItemPos: [-10,16]),
            new(Identifiers.VGM_SniperScope, ItemPos: [-9,18]),
        ];

        public override ContainableScanner[] CompatibleScanners => [
            new(Identifiers.VGM_HealthScanner, ItemPos: [3,11]),
            new(Identifiers.VGM_ThermalScanner, ItemPos: [3,11]),
        ];

        public override string Generate()
        {
            string gunXmlString =
$@"
<Item {GenerateGunXMLAttributesString()} cargocontaineridentifier=""metalcrate"" allowasextracargo=""true"" impactsoundtag=""impact_metal_light"">
    {GenerateBasicInfoXMLsString(fabricateRequiredSkill: 65)}

    {GenerateItemSpriteXMLString(
        texture: "%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/Guns/Guns.png",
        sourceRect: [0, 128, 115, 56],
        depth: 0.52f,
        origin: [0.5f, 0.5f]
    )}

    {GenerateRectangleBodyXMLString(width: 111, height: 52, density: 25)}

    <Holdable {GenerateHoldableXMLAttributesString(
            slots: ["Any", "RightHand+LeftHand"],
            controlPos: true,
            holdPos: [47, -16],
            aimPos: [49, -10],
            handle1: [-42, -8],
            handle2: [9, 3]
        )}>

        {GenerateGunModifySpeedMultiplierXMLsString()}
        {GenerateStockModifySpeedMultiplierXMLsString()}

        {GenerateGunSpreadChangesOnAimDownSightXMLsString()}
        {GenerateGripModifySpreadChangesOnAimDownSightXMLsString()}
        {GenerateAimingDeviceModifySpreadChangesOnAimDownSightXMLsString()}

        {GenerateGunSpreadRecoveryXMLsString()}
        {GenerateGripSpreadRecoveryXMLsString()}
        {GenerateAimingDeviceSpreadRecoveryXMLsString()}

        {GenerateAimingDeviceObstructVisionXMLsString()}
        
        {GenerateScannerActivationXMLsString()}

        {GenerateHotTagWasAimingXMLsString()}
        {GenerateHotTagPreventSpreadingOnADSXMLsString()}

        {GenerateFiringModeBurstForHoldableXMLsString()}
    </Holdable>

    {GenerateStatusHUDXMLsString()}
    {GenerateAiTargetXMLsString()}

    <RangedWeapon {GenerateRangedWeaponXMLAttributesString()}>
        {GenerateDefaultCrosshairXMLsString()}
        {GenerateMuzzleFlashXMLsString()}

        <StatusEffect type=""OnUse"" target=""This"">
          <ParticleEmitter particle=""casingfirearm"" particleamount=""1"" anglemin=""90"" anglemax=""150"" velocitymin=""50"" velocitymax=""250"" copyentityangle=""true"" />
        </StatusEffect>

        {GenerateFiringModeBurstForRangedWeaponXMLsString(3, 0.1f)}

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
        {GenerateStockSimulatedRecoilXMLsString()}

        <StatusEffect type=""OnUse"" target=""Contained"" targetslot=""0"">
            <Use />
        </StatusEffect>

        <RequiredItems items=""VGM_SMGAmmo,smgammo"" type=""Contained"" targetslot=""0"" msg=""ItemMsgAmmoRequired"" />
        {GenerateMajorRequiredWeaponsSkillXMLsString()}
    </RangedWeapon>

    {GeneratePropulsionXMLsString()}

    <ItemContainer capacity=""1"" maxstacksize=""1"" hideitems=""false"" containedstateindicatorslot=""0"" containedstateindicatorstyle=""bullet"">
        <Containable identifier=""VGM_SMGLongMagazine"" hide=""false"" itempos=""-6,-5"" rotation=""-25"" />
        <Containable identifier=""VGM_SMGDrumMagazine"" hide=""false"" itempos=""-6,-5"" rotation=""-25"" />
        <Containable tag=""smgammo"" hide=""false"" itempos=""-6,-5"" rotation=""-25"" />

        {GenerateSlotIconBulletsXMLString(0)}
        {GenerateSlotIconFlashlightXMLString(1)}
        {GenerateSlotIconStockXMLString(2)}
        {GenerateSlotIconMuzzleXMLString(3)}
        {GenerateSlotIconAimingDeviceXMLString(4)}
        {GenerateSlotIconScannerXMLString(5)}

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateGripOnContainedXMLsString()}
            {GenerateContainableGenericAccessories(itemPos: [10, 5])}
        </SubContainer>

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateStockOnContainedXMLsString()}
        </SubContainer>

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateMuzzleOnContainedXMLsString()}
        </SubContainer>

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateAimingDeviceOnContainedXMLsString()}
            {GenerateContainableGenericAccessories(itemPos: [-10, 13])}
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
