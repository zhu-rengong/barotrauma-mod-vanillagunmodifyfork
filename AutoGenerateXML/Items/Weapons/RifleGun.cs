using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static AutoGenerateXML.ItemXMLExtensions;

namespace AutoGenerateXML.Items.Weapons
{
    public class RifleGun : GunXMLGenerator
    {
        public RifleGun() : base()
        {
            Name = "Rifle";
            OutputPath = Path.Combine("Guns", $@"{Name}.xml");
            Identifier = Identifiers.VGM_Rifle;

            SelfTags.AddRange(["mediumitem", "gunsmith", Name]);

            LowerAccessorySlotIndex = 1;
            StockSlotIndex = 2;
            MuzzleSlotIndex = 3;
            UpperAccessorySlotIndex = 4;
            ScannerSlotIndex = 5;
        }

        public override float? HoldAngle => -40;

        public override float[] BarrelPos => [72, 6];
        public override float WeaponDamageModifier => 1.0f;
        public override float Penetration => 0.0f;
        public override float RequiredWeaponsSkill => 60;

        public override float Reload => 2;
        public override float? ReloadSkillRequirement => 50;
        public override float? ReloadNoSkill => 5;
        public override float CombatPriority => 70;
        public override float MinimumSpread => 0.5f;
        public override float MinimumUnskilledSpread => 3.5f;
        public override float SpreadChangesOnAimDownSight => 12;
        public override float SpreadRecovery => 0.5f;
        public override float SpreadChangesOnShoot => 9.7f;
        public override float SpreadLimit => 15.0f;
        public override float Recoil => 400;
        public override float StockRecoilReductionEfficiency => 1.75f;
        public override float StocklessSpeedMultiplier => 1.1f;

        public override List<ContainableGrip> ContainableGrips => [
            new(GripXMLGenerator.All[Identifiers.VGM_AngledForeGrip], ItemPos: [10,-4]),
            new(GripXMLGenerator.All[Identifiers.VGM_VerticalGrip], ItemPos: [11,-7]),
            new(GripXMLGenerator.All[Identifiers.VGM_BipodGrip], ItemPos: [27,-14]),
        ];

        public override List<ContainableStock> ContainableStocks => [
            new(StockXMLGenerator.All[Identifiers.VGM_RifleStock], ItemPos: [-47,-7]),
            new(StockXMLGenerator.All[Identifiers.VGM_LightStock], ItemPos: [-41,-3]),
            new(StockXMLGenerator.All[Identifiers.VGM_LightSniperStock], ItemPos: [-47,-4]),
            new(StockXMLGenerator.All[Identifiers.VGM_LightWrenchStock], ItemPos: [-46,-5]),
        ];

        public override List<ContainableMuzzle> ContainableMuzzles => [
            new(MuzzleXMLGenerator.All[Identifiers.VGM_SimpleSuppressorMuzzle]),
            new(MuzzleXMLGenerator.All[Identifiers.VGM_ShortSuppressorMuzzle]),
            new(MuzzleXMLGenerator.All[Identifiers.VGM_LongSuppressorMuzzle]),
            new(MuzzleXMLGenerator.All[Identifiers.VGM_FlashHiderMuzzle]),
            new(MuzzleXMLGenerator.All[Identifiers.VGM_LongBarrelMuzzle]),
        ];

        public override List<ContainableAimingDevice> ContainableAimingDevices => [
            new(AimingDeviceXMLGenerator.All[Identifiers.VGM_RedDotSight], ItemPos: [-18,9]),
            new(AimingDeviceXMLGenerator.All[Identifiers.VGM_HolographicSight], ItemPos: [-15,10]),
            new(AimingDeviceXMLGenerator.All[Identifiers.VGM_ACOGScope], ItemPos: [-17,12]),
            new(AimingDeviceXMLGenerator.All[Identifiers.VGM_RifleScope], ItemPos: [-15,9]),
            new(AimingDeviceXMLGenerator.All[Identifiers.VGM_SniperScope], ItemPos: [-6,11]),
        ];

        public override List<ContainableScanner> ContainableScanners => [
            new(ScannerXMLGenerator.All[Identifiers.VGM_HealthScanner], ItemPos: [-15,4]),
            new(ScannerXMLGenerator.All[Identifiers.VGM_ThermalScanner], ItemPos: [-15,4]),
        ];

        public override string Generate()
        {
            string gunXmlString =
$@"
<Item {GenerateGunXMLAttributesString()} cargocontaineridentifier=""metalcrate"" impactsoundtag=""impact_metal_light"">
    {GenerateBasicInfoXMLsString(fabricateRequiredSkill: 55)}

    {GenerateItemSpriteXMLString(
        texture: "%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/Guns/Guns.png",
        sourceRect: [0, 0, 150, 31],
        depth: 0.52f,
        origin: [0.5f, 0.5f]
    )}

    {GenerateRectangleBodyXMLString(width: 144, height: 29, density: 25)}

    <Holdable {GenerateHoldableXMLAttributesString(
            slots: ["Any", "RightHand+LeftHand"],
            controlPos: true,
            holdPos: [50, -13],
            aimPos: [65, 0],
            handle1: [-52, -13],
            handle2: [0, 5]
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

    {GenerateStatusHUDXMLsString()}
    {GenerateAiTargetXMLsString()}

    <RangedWeapon {GenerateRangedWeaponXMLAttributesString()}>
        {GenerateDefaultCrosshairXMLsString()}
        
        {GenerateMuzzleFlashXMLsString()}

        {GenerateGunfireOnShootXMLsString(
            normalSoundFiles: [
                @"Content/Items/Weapons/WEAPON_rifleShot1.ogg",
                @"Content/Items/Weapons/WEAPON_rifleShot2.ogg",
                @"Content/Items/Weapons/WEAPON_rifleShot3.ogg",
            ]
        )}

        {GenerateFlashHiderMuzzleOnShootXMLsString()}

        {GenerateGunSpreadChangesOnShootXMLsString()}
        {GenerateMuzzleModifySpreadChangesOnShootXMLsString()}

        {GenerateGunSimulatedRecoilXMLsString()}
        {GenerateStockSimulatedRecoilXMLsString()}

        <StatusEffect type=""OnUse"" delay=""1"" target=""Character"" forceplaysounds=""true"">
            <Conditional skillrequirement=""true"" weapons=""gte {RequiredWeaponsSkill - 10}"" />
            <ParticleEmitter particle=""casingfirearm"" particleamount=""1"" anglemin=""90"" anglemax=""150"" velocitymin=""50"" velocitymax=""250"" CopyEntityAngle=""true"" />
            <Sound file=""Content/Items/Weapons/WEAPON_rifleReload50-_1.ogg"" type=""OnUse"" range=""500"" selectionmode=""Random"" />
            <Sound file=""Content/Items/Weapons/WEAPON_rifleReload50-_2.ogg"" type=""OnUse"" range=""500"" />
        </StatusEffect>
        <StatusEffect type=""OnUse"" delay=""1"" target=""Character"" comparison=""and"" forceplaysounds=""true"">
            <Conditional skillrequirement=""true"" weapons=""lt {RequiredWeaponsSkill - 10}"" />
            <Conditional skillrequirement=""true"" weapons=""gte {RequiredWeaponsSkill - 40}"" />
            <ParticleEmitter particle=""casingfirearm"" particleamount=""1"" anglemin=""90"" anglemax=""150"" velocitymin=""50"" velocitymax=""250"" CopyEntityAngle=""true"" />
            <Sound file=""Content/Items/Weapons/WEAPON_rifleReload20-50_1.ogg"" type=""OnUse"" range=""500"" selectionmode=""Random"" />
            <Sound file=""Content/Items/Weapons/WEAPON_rifleReload20-50_2.ogg"" type=""OnUse"" range=""500"" />
        </StatusEffect>
        <StatusEffect type=""OnUse"" delay=""1"" target=""Character"" forceplaysounds=""true"">
            <Conditional skillrequirement=""true"" weapons=""lt {RequiredWeaponsSkill - 40}"" />
            <ParticleEmitter particle=""casingfirearm"" particleamount=""1"" anglemin=""90"" anglemax=""150"" velocitymin=""50"" velocitymax=""250"" CopyEntityAngle=""true"" />
            <Sound file=""Content/Items/Weapons/WEAPON_rifleReload0-20_1.ogg"" type=""OnUse"" range=""500"" selectionmode=""Random"" />
            <Sound file=""Content/Items/Weapons/WEAPON_rifleReload0-20_2.ogg"" type=""OnUse"" range=""500"" />
        </StatusEffect>
        <RequiredItems items=""rifleammo"" type=""Contained"" targetslot=""0"" msg=""ItemMsgAmmoRequired"" />
        {GenerateMajorRequiredWeaponsSkillXMLsString()}
    </RangedWeapon>

    {GeneratePropulsionXMLsString()}

    <ItemContainer capacity=""1"" maxstacksize=""6"" hideitems=""false"" containedstateindicatorslot=""0"" containedstateindicatorstyle=""bullet"">
        <Containable items=""rifleammo"" hide=""true"" />

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
