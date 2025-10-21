using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static AutoGenerateXML.ItemXMLExtensions;

namespace AutoGenerateXML.Items.Weapons
{
    public class MachinePistol : GunXMLGenerator
    {
        public override string OutputPath => Path.Combine("Guns", $@"{GunName}.xml");

        public override string GunName => nameof(MachinePistol);

        public override string Identifier => Identifiers.VGM_MachinePistol;
        public override string SelfTags => "smallautoweapon,smallitem,weapon,gun,gunsmith,provocativetohumanai,mountableweapon,machinepistol";

        public override int LowerAccessorySlotIndex => 1;
        public override int MuzzleSlotIndex => 2;
        public override int UpperAccessorySlotIndex => 3;

        public override float[] BarrelPos => [53, 14];
        public override float WeaponDamageModifier => 0.8f;
        public override float Penetration => 0.0f;
        public override float RequiredWeaponsSkill => 50;

        public override float Reload => 0.175f;
        public override float CombatPriority => 70;
        public override float MinimumSpread => 12;
        public override float MinimumUnskilledSpread => 18;
        public override float SpreadChangesOnAimDownSight => 8;
        public override float SpreadRecovery => 0.4f;
        public override float SpreadChangesOnShoot => 5;
        public override float SpreadLimit => 11.0f;
        public override float Recoil => 170;
        public override float StocklessSpeedMultiplier => 1.2f;

        public override ContainableMuzzle[] CompatibleMuzzles => [
            new(Identifiers.VGM_SimpleSuppressorMuzzle),
            new(Identifiers.VGM_ShortSuppressorMuzzle),
            new(Identifiers.VGM_FlashHiderMuzzle),
        ];

        public override ContainableAimingDevice[] CompatibleAimingDevices => [
            new(Identifiers.VGM_RedDotSight, ItemPos: [1,13]),
            new(Identifiers.VGM_HolographicSight, ItemPos: [2,14]),
            new(Identifiers.VGM_ACOGScope, ItemPos: [-1,16]),
        ];

        public override string Generate()
        {
            string gunXmlString =
$@"
<Item {GenerateGunXMLAttributesString()} cargocontaineridentifier=""metalcrate"" impactsoundtag=""impact_metal_light"">
    {GenerateBasicInfoXMLsString(fabricateRequiredSkill: 45)}

    {GenerateItemSpriteXMLString(
        texture: "%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/Guns/Guns.png",
        sourceRect: [0, 416, 107, 49],
        depth: 0.52f,
        origin: [0.5f, 0.5f]
    )}

    {GenerateRectangleBodyXMLString(width: 103, height: 45, density: 25)}

    <Holdable {GenerateHoldableXMLAttributesString(
            slots: ["Any", "RightHand", "LeftHand"],
            controlPos: true,
            aimPos: [72, 2],
            handle1: [-31, -9]
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

    <RangedWeapon {GenerateRangedWeaponXMLAttributesString()} dualwieldaccuracypenalty=""10"">
        {GenerateDefaultCrosshairXMLsString()}
        
        {GenerateMuzzleFlashXMLsString()}

        <StatusEffect type=""OnUse"" target=""This"">
          <ParticleEmitter particle=""casingfirearm"" particleamount=""1"" anglemin=""90"" anglemax=""150"" velocitymin=""50"" velocitymax=""250"" copyentityangle=""true"" />
        </StatusEffect>

        {GenerateGunfireOnShootXMLsString(
            normalSoundFiles: [
                @"Content/Items/Weapons/WEAPON_machinePistolShot1.ogg",
                @"Content/Items/Weapons/WEAPON_machinePistolShot2.ogg",
                @"Content/Items/Weapons/WEAPON_machinePistolShot3.ogg",
                @"Content/Items/Weapons/WEAPON_machinePistolShot4.ogg",
                @"Content/Items/Weapons/WEAPON_machinePistolShot5.ogg",
                @"Content/Items/Weapons/WEAPON_machinePistolShot6.ogg"
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
        <Containable identifier=""VGM_SMGLongMagazine"" hide=""false"" itempos=""3,-8"" rotation=""-5"" />
        <Containable identifier=""VGM_SMGDrumMagazine"" hide=""false"" itempos=""3,-8"" rotation=""-5"" />
        <Containable tag=""smgammo"" hide=""false"" itempos=""3,-8"" rotation=""-5"" />
        {GenerateSlotIconBulletsXMLString(0)}
        {GenerateSlotIconFlashlightXMLString(1)}
        {GenerateSlotIconMuzzleXMLString(2)}
        {GenerateSlotIconAimingDeviceXMLString(3)}

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateContainableGenericAccessories(itemPos: [13, 4])}
        </SubContainer>

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateMuzzleOnContainedXMLsString()}
        </SubContainer>

        <SubContainer capacity=""1"" maxstacksize=""1"">
            {GenerateAimingDeviceOnContainedXMLsString()}
            {GenerateContainableGenericAccessories(itemPos: [2, 10])}
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
