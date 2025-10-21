using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static AutoGenerateXML.ItemXMLExtensions;

namespace AutoGenerateXML.Items.Weapons
{
    public class Handcannon : GunXMLGenerator
    {
        public override string OutputPath => Path.Combine("Guns", $@"{GunName}.xml");

        public override string GunName => nameof(Handcannon);

        public override string Identifier => Identifiers.VGM_Handcannon;
        public override string SelfTags => "smallitem,weapon,gun,pistolitem,mountableweapon,provocativetohumanai,handcannon";

        public override int LowerAccessorySlotIndex => 1;
        public override int UpperAccessorySlotIndex => 2;

        public override float[] BarrelPos => [54, 17];
        public override float WeaponDamageModifier => 1.0f;
        public override float Penetration => 0.0f;
        public override float RequiredWeaponsSkill => 50;

        public override float Reload => 1;
        public override float CombatPriority => 90;
        public override float MinimumSpread => 0.1f;
        public override float MinimumUnskilledSpread => 8;
        public override float SpreadChangesOnAimDownSight => 10;
        public override float SpreadRecovery => 1.2f;
        public override float SpreadChangesOnShoot => 18f;
        public override float SpreadLimit => 22.0f;
        public override float Recoil => 550;
        public override float StocklessSpeedMultiplier => 1.1f;

        public override ContainableAimingDevice[] CompatibleAimingDevices => [
            new(Identifiers.VGM_RedDotSight, ItemPos: [-3,14]),
            new(Identifiers.VGM_HolographicSight, ItemPos: [-2,15]),
            new(Identifiers.VGM_ACOGScope, ItemPos: [-6,17]),
            new(Identifiers.VGM_RifleScope, ItemPos: [-4,14]),
            new(Identifiers.VGM_SniperScope, ItemPos: [-5,17]),
        ];

        public override string Generate()
        {
            string gunXmlString =
$@"
<Item {GenerateGunXMLAttributesString()} cargocontaineridentifier=""metalcrate"" impactsoundtag=""impact_metal_light"">
    {GenerateBasicInfoXMLsString(fabricateRequiredSkill: 60)}

    {GenerateItemSpriteXMLString(
        texture: "%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/Guns/Guns.png",
        sourceRect: [0, 768, 107, 52],
        depth: 0.52f,
        origin: [0.5f, 0.5f]
    )}

    {GenerateRectangleBodyXMLString(width: 103, height: 48, density: 25)}

    <Holdable {GenerateHoldableXMLAttributesString(
            slots: ["Any", "RightHand+LeftHand"],
            controlPos: true,
            aimPos: [72, 8],
            handle1: [-38, -11]
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

    <RangedWeapon {GenerateRangedWeaponXMLAttributesString()}>
        {GenerateDefaultCrosshairXMLsString()}

        {GenerateMuzzleFlashXMLsString("impactfirearm", amount: 6, scale: [3.5f, 7.0f], color: [0.95f, 1.00f, 0.65f, 0.34f])}

        <StatusEffect type=""OnUse"" target=""This"">
          <ParticleEmitter particle=""casingfirearm"" particleamount=""1"" anglemin=""90"" anglemax=""150"" velocitymin=""50"" velocitymax=""250"" copyentityangle=""true"" />
        </StatusEffect>

        {GenerateGunfireOnShootXMLsString(
            normalSoundFiles: [
                @"Content/Items/JobGear/Captain/WEAPONS_handCannon.ogg",
                @"Content/Items/JobGear/Captain/WEAPONS_handCannon_1.ogg",
                @"Content/Items/JobGear/Captain/WEAPONS_handCannon_2.ogg",
                @"Content/Items/JobGear/Captain/WEAPONS_handCannon_3.ogg",
                @"Content/Items/JobGear/Captain/WEAPONS_handCannon_4.ogg",
                @"Content/Items/JobGear/Captain/WEAPONS_handCannon_5.ogg",
            ]
        )}

        {GenerateGunSpreadChangesOnShootXMLsString()}

        {GenerateGunSimulatedRecoilXMLsString()}

        <RequiredItems items=""handcannonammo"" type=""Contained"" targetslot=""0"" msg=""ItemMsgAmmoRequired"" />
        {GenerateMajorRequiredWeaponsSkillXMLsString()}
    </RangedWeapon>

    {GeneratePropulsionXMLsString()}

    <ItemContainer capacity=""1"" maxstacksize=""6"" hideitems=""false"" containedstateindicatorslot=""0"" containedstateindicatorstyle=""bullet"">
        <Containable items=""handcannonammo"" hide=""true"" />

        {GenerateSlotIconBulletsXMLString(0)}
        {GenerateSlotIconFlashlightXMLString(1)}
        {GenerateSlotIconAimingDeviceXMLString(2)}

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateContainableGenericAccessories(itemPos: [13, 4])}
        </SubContainer>

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateAimingDeviceOnContainedXMLsString()}
            {GenerateContainableGenericAccessories(itemPos: [-3, 10])}
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
