using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static AutoGenerateXML.ItemXMLExtensions;

namespace AutoGenerateXML.Items.Weapons
{
    public class Shotgun : GunXMLGenerator
    {
        public override string OutputPath => Path.Combine("Guns", $@"{GunName}.xml");

        public override string GunName => nameof(Shotgun);

        public override string Identifier => Identifiers.VGM_Shotgun;
        public override string SelfTags => "mediumitem,weapon,gun,gunsmith,provocativetohumanai,mountableweapon,rifle";

        public override int StockSlotIndex => 2;
        public override int ScannerSlotIndex => 4;
        public override int UpperAccessorySlotIndex => 3;
        public override int LowerAccessorySlotIndex => 1;

        public override float? HoldAngle => -40;

        public override float[] BarrelPos => [66, 7];
        public override float WeaponDamageModifier => 1.0f;
        public override float Penetration => 0.0f;
        public override float RequiredWeaponsSkill => 50;

        public override float Reload => 1;
        public override float CombatPriority => 75;
        public override float MinimumSpread => 2;
        public override float MinimumUnskilledSpread => 6;
        public override float SpreadChangesOnAimDownSight => 5;
        public override float SpreadRecovery => 0.9f;
        public override float SpreadChangesOnShoot => 13f;
        public override float SpreadLimit => 15.0f;
        public override float Recoil => 500;
        public override float StockRecoilReductionEfficiency => 2.2f;
        public override float StocklessSpeedMultiplier => 1.1f;

        public override ContainableStock[] CompatibleStocks => [
            new(Identifiers.VGM_ShotgunStock, ItemPos: [-43,-8]),
            new(Identifiers.VGM_LightStock, ItemPos: [-45,-6]),
            new(Identifiers.VGM_LightSniperStock, ItemPos: [-45,-5]),
            new(Identifiers.VGM_LightWrenchStock, ItemPos: [-42,-4]),
        ];

        public override ContainableAimingDevice[] CompatibleAimingDevices => [
            new(Identifiers.VGM_RedDotSight, ItemPos: [0,9]),
            new(Identifiers.VGM_HolographicSight, ItemPos: [0,10]),
            new(Identifiers.VGM_ACOGScope, ItemPos: [-4,13]),
            new(Identifiers.VGM_RifleScope, ItemPos: [-2,9]),
            new(Identifiers.VGM_SniperScope, ItemPos: [-1,13]),
        ];

        public override string Generate()
        {
            string gunXmlString =
$@"
<Item {GenerateGunXMLAttributesString()} cargocontaineridentifier=""metalcrate"" impactsoundtag=""impact_metal_light"">
    {GenerateBasicInfoXMLsString(fabricateRequiredSkill: 55)}

    {GenerateItemSpriteXMLString(
        texture: "%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/Guns/Guns.png",
        sourceRect: [0, 352, 137, 32],
        depth: 0.52f,
        origin: [0.5f, 0.5f]
    )}

    {GenerateRectangleBodyXMLString(width: 133, height: 28, density: 25)}

    <Holdable {GenerateHoldableXMLAttributesString(
            slots: ["Any", "RightHand+LeftHand"],
            controlPos: true,
            holdPos: [50, -13],
            aimPos: [66, 6],
            handle1: [-63, -19],
            handle2: [-21, -1]
        )}>

        {GenerateGunModifySpeedMultiplierXMLsString()}
        {GenerateStockModifySpeedMultiplierXMLsString()}

        {GenerateGunSpreadChangesOnAimDownSightXMLsString()}
        {GenerateAimingDeviceModifySpreadChangesOnAimDownSightXMLsString()}

        {GenerateGunSpreadRecoveryXMLsString()}
        {GenerateAimingDeviceSpreadRecoveryXMLsString()}

        {GenerateAimingDeviceObstructVisionXMLsString()}
        
        {GenerateScannerActivationXMLsString()}

        {GenerateHotTagWasAimingXMLsString()}
        {GenerateHotTagPreventSpreadingOnADSXMLsString()}
    </Holdable>

    {GenerateStatusHUDXMLsString()}
    {GenerateAiTargetXMLsString()}

    <RangedWeapon {GenerateRangedWeaponXMLAttributesString()}>
        {GenerateDefaultCrosshairXMLsString()}
        <ParticleEmitter particle=""muzzleflash"" particleamount=""1"" velocitymin=""0"" velocitymax=""0"" />

        {GenerateGunfireOnShootXMLsString(
            normalSoundFiles: [
                @"Content/Items/Weapons/ShotgunShot1.ogg",
                @"Content/Items/Weapons/ShotgunShot2.ogg",
                @"Content/Items/Weapons/ShotgunShot3.ogg",
                @"Content/Items/Weapons/ShotgunShot4.ogg",
            ]
        )}

        {GenerateGunSpreadChangesOnShootXMLsString()}

        {GenerateGunSimulatedRecoilXMLsString()}
        {GenerateStockSimulatedRecoilXMLsString()}

        <StatusEffect type=""OnUse"" target=""This"" delay=""0.8"">
            <Sound file=""Content/Items/Weapons/ShotgunLoad1.ogg"" type=""OnUse"" range=""500"" selectionmode=""Random"" />
            <Sound file=""Content/Items/Weapons/ShotgunLoad2.ogg"" type=""OnUse"" range=""500"" />
            <Sound file=""Content/Items/Weapons/ShotgunLoad3.ogg"" type=""OnUse"" range=""500"" />
            <ParticleEmitter particle=""casingfirearm"" colormultiplier=""0.5,0.5,0.5,1"" ScaleMultiplier=""1.5,1.5"" particleamount=""1"" anglemin=""90"" anglemax=""150"" velocitymin=""50"" velocitymax=""250"" CopyEntityAngle=""true"" />
        </StatusEffect>

        <RequiredItems items=""shotgunammo"" type=""Contained"" targetslot=""0"" msg=""ItemMsgAmmoRequired"" />
        {GenerateMajorRequiredWeaponsSkillXMLsString()}
    </RangedWeapon>

    {GeneratePropulsionXMLsString()}

    <ItemContainer capacity=""1"" maxstacksize=""6"" hideitems=""false"" containedstateindicatorslot=""0"" containedstateindicatorstyle=""bullet"">
        <Containable items=""shotgunammo"" hide=""true"" />

        {GenerateSlotIconBulletsXMLString(0)}
        {GenerateSlotIconFlashlightXMLString(1)}
        {GenerateSlotIconStockXMLString(2)}
        {GenerateSlotIconAimingDeviceXMLString(3)}
        {GenerateSlotIconScannerXMLString(4)}

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateContainableGenericAccessories(itemPos: [14, 5])}
        </SubContainer>

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateStockOnContainedXMLsString()}
        </SubContainer>

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateAimingDeviceOnContainedXMLsString()}
            {GenerateContainableGenericAccessories(itemPos: [6, 5])}
        </SubContainer>

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateScannerOnContainedXMLsString([
                 new(Identifiers.VGM_HealthScanner, ItemPos: [5,5]),
                 new(Identifiers.VGM_ThermalScanner, ItemPos: [5,5]),
            ])}
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
