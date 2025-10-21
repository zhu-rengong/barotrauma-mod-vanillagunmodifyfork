using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static AutoGenerateXML.ItemXMLExtensions;

namespace AutoGenerateXML.Items.Weapons
{
    public class Revolver : GunXMLGenerator
    {
        public override string OutputPath => Path.Combine("Guns", $@"{GunName}.xml");

        public override string GunName => nameof(Revolver);

        public override string Identifier => Identifiers.VGM_Revolver;
        public override string SelfTags => "smallitem,weapon,gun,pistolitem,provocativetohumanai,gunsmith,mountableweapon,revolver";

        public override int LowerAccessorySlotIndex => 1;
        public override int UpperAccessorySlotIndex => 2;

        public override float[] BarrelPos => [36, 13];
        public override float WeaponDamageModifier => 1.0f;
        public override float Penetration => 0.0f;
        public override float RequiredWeaponsSkill => 40;

        public override float Reload => 0.5f;
        public override float CombatPriority => 70;
        public override float MinimumSpread => 0.1f;
        public override float MinimumUnskilledSpread => 6;
        public override float SpreadChangesOnAimDownSight => 4;
        public override float SpreadRecovery => 1.3f;
        public override float SpreadChangesOnShoot => 8f;
        public override float SpreadLimit => 19.0f;
        public override float Recoil => 220;
        public override float StocklessSpeedMultiplier => 1.1f;

        public override ContainableAimingDevice[] CompatibleAimingDevices => [
            new(Identifiers.VGM_RedDotSight, ItemPos: [2,12]),
            new(Identifiers.VGM_HolographicSight, ItemPos: [0,12]),
            new(Identifiers.VGM_ACOGScope, ItemPos: [-2,15]),
            new(Identifiers.VGM_RifleScope, ItemPos: [0,12]),
            new(Identifiers.VGM_SniperScope, ItemPos: [1,15]),
        ];

        public override string Generate()
        {
            string gunXmlString =
$@"
<Item {GenerateGunXMLAttributesString()} cargocontaineridentifier=""metalcrate"" impactsoundtag=""impact_metal_light"">
    {GenerateBasicInfoXMLsString(fabricateRequiredSkill: 60)}

    {GenerateItemSpriteXMLString(
        texture: "%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/Guns/Guns.png",
        sourceRect: [0, 864, 75, 41],
        depth: 0.52f,
        origin: [0.5f, 0.5f]
    )}

    {GenerateRectangleBodyXMLString(width: 71, height: 37, density: 25)}

    <Holdable {GenerateHoldableXMLAttributesString(
            slots: ["Any", "RightHand", "LeftHand"],
            controlPos: true,
            aimPos: [72, 8],
            handle1: [-26, -10]
        )}>

        {GenerateGunModifySpeedMultiplierXMLsString()}

        {GenerateGunSpreadChangesOnAimDownSightXMLsString()}
        {GenerateAimingDeviceModifySpreadChangesOnAimDownSightXMLsString()}

        {GenerateGunSpreadRecoveryXMLsString()}
        {GenerateAimingDeviceSpreadRecoveryXMLsString()}

        {GenerateAimingDeviceObstructVisionXMLsString()}

        {GenerateHotTagPreventSpreadingOnADSXMLsString()}
    </Holdable>

    {GenerateAiTargetXMLsString()}

    <RangedWeapon {GenerateRangedWeaponXMLAttributesString()} DualWieldReloadTimePenaltyMultiplier=""1.75"" DualWieldAccuracyPenalty=""8"">
        {GenerateDefaultCrosshairXMLsString()}
        
        {GenerateMuzzleFlashXMLsString()}

        <StatusEffect type=""OnUse"" target=""This"">
          <ParticleEmitter particle=""casingfirearm"" particleamount=""1"" anglemin=""90"" anglemax=""150"" velocitymin=""50"" velocitymax=""250"" copyentityangle=""true"" />
        </StatusEffect>

        {GenerateGunfireOnShootXMLsString(
            normalSoundFiles: [
                @"Content/Items/Weapons/Revolver1.ogg",
                @"Content/Items/Weapons/Revolver2.ogg",
                @"Content/Items/Weapons/Revolver3.ogg",
            ]
        )}

        {GenerateGunSpreadChangesOnShootXMLsString()}

        {GenerateGunSimulatedRecoilXMLsString()}

        <RequiredItems items=""revolverammo"" type=""Contained"" targetslot=""0"" msg=""ItemMsgAmmoRequired"" />
        {GenerateMajorRequiredWeaponsSkillXMLsString()}
    </RangedWeapon>

    {GeneratePropulsionXMLsString()}

    <ItemContainer capacity=""1"" maxstacksize=""6"" hideitems=""false"" containedstateindicatorslot=""0"" containedstateindicatorstyle=""bullet"">
        <Containable items=""revolverammo"" hide=""true"" />

        {GenerateSlotIconBulletsXMLString(0)}
        {GenerateSlotIconFlashlightXMLString(1)}
        {GenerateSlotIconAimingDeviceXMLString(2)}

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateContainableGenericAccessories(itemPos: [13, -1])}
        </SubContainer>

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateAimingDeviceOnContainedXMLsString()}
            {GenerateContainableGenericAccessories(itemPos: [1, 9])}
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
