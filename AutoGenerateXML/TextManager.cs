using System.Collections.Immutable;
using System.Net;

namespace AutoGenerateXML
{
    public static class TextManager
    {
        public enum LanguageId
        {
            English,
            SimplifiedChinese
        }

        private static ImmutableDictionary<string, ValueTuple<LanguageId, string>[]> languageTexts;

        static TextManager()
        {
            languageTexts = new Dictionary<string, ValueTuple<LanguageId, string>[]>()
            {
                { $@"entityname.{Identifiers.VGM_Rifle}", [ (LanguageId.English, "Rifle (Modifiable)"), (LanguageId.SimplifiedChinese, "步枪(可改装的)") ] },
                { $@"entityname.{Identifiers.VGM_SMG}", [ (LanguageId.English, "SMG (Modifiable)"), (LanguageId.SimplifiedChinese, "冲锋枪(可改装的)") ] },
                { $@"entityname.{Identifiers.VGM_SMGUnique}", [ (LanguageId.English, "Deadeye Carbine (Modifiable)"), (LanguageId.SimplifiedChinese, "亡眼步枪(可改装的)") ] },
                { $@"entityname.{Identifiers.VGM_MachinePistol}", [ (LanguageId.English, "Machine Pistol (Modifiable)"), (LanguageId.SimplifiedChinese, "机关手枪(可改装的)") ] },
                { $@"entityname.{Identifiers.VGM_AssaultRifle}", [ (LanguageId.English, "Assault Rifle (Modifiable)"), (LanguageId.SimplifiedChinese, "突击步枪(可改装的)") ] },
                { $@"entityname.{Identifiers.VGM_HMG}", [ (LanguageId.English, "HMG (Modifiable)"), (LanguageId.SimplifiedChinese, "重机枪(可改装的)") ] },
                { $@"entityname.{Identifiers.VGM_Shotgun}", [ (LanguageId.English, "Riot Shotgun (Modifiable)"), (LanguageId.SimplifiedChinese, "防暴霰弹枪(可改装的)") ] },

                { $@"entityname.{Identifiers.VGM_RGBLaserPointer}", [ (LanguageId.English, "RGB Laser Pointer"), (LanguageId.SimplifiedChinese, "RGB激光指示器") ] },

                { $@"entityname.{Identifiers.VGM_RifleStock}", [ (LanguageId.English, "Rifle OEM Stock"), (LanguageId.SimplifiedChinese, "步枪原装枪托") ] },
                { $@"entityname.{Identifiers.VGM_SMGUniqueStock}", [ (LanguageId.English, "Deadeye Carbine OEM Stock"), (LanguageId.SimplifiedChinese, "亡眼步枪原装枪托") ] },
                { $@"entityname.{Identifiers.VGM_AssaultRifleStock}", [ (LanguageId.English, "Assault Rifle OEM Stock"), (LanguageId.SimplifiedChinese, "突击步枪原装枪托") ] },
                { $@"entityname.{Identifiers.VGM_HMGStock}", [ (LanguageId.English, "HMG OEM Stock"), (LanguageId.SimplifiedChinese, "重机枪原装枪托") ] },
                { $@"entityname.{Identifiers.VGM_ShotgunStock}", [ (LanguageId.English, "Riot Shotgun OEM Stock"), (LanguageId.SimplifiedChinese, "防暴霰弹枪原装枪托") ] },
                { $@"entityname.{Identifiers.VGM_LightStock}", [ (LanguageId.English, "Light Tubular Stock"), (LanguageId.SimplifiedChinese, "轻型管材枪托") ] },
                { $@"entityname.{Identifiers.VGM_LightSniperStock}", [ (LanguageId.English, "Light Marksman Stock"), (LanguageId.SimplifiedChinese, "轻型射手枪托") ] },
                { $@"entityname.{Identifiers.VGM_LightWrenchStock}", [ (LanguageId.English, "Light Wrench Stock"), (LanguageId.SimplifiedChinese, "轻型扳手枪托") ] },
                { $@"entityname.{Identifiers.VGM_HeavyStock}", [ (LanguageId.English, "Heavy Counterweight Stock"), (LanguageId.SimplifiedChinese, "重型配重式枪托") ] },
                { $@"entityname.{Identifiers.VGM_HeavySniperStock}", [ (LanguageId.English, "Heavy Ergonomic Stock"), (LanguageId.SimplifiedChinese, "重型人体工程学枪托") ] },

                { $@"entityname.{Identifiers.VGM_LongSuppressorMuzzle}", [ (LanguageId.English, "Long Suppressor"), (LanguageId.SimplifiedChinese, "加长型‌消音器") ] },
                { $@"entityname.{Identifiers.VGM_ShortSuppressorMuzzle}", [ (LanguageId.English, "Short Suppressor"), (LanguageId.SimplifiedChinese, "短‌消音器") ] },
                { $@"entityname.{Identifiers.VGM_SimpleSuppressorMuzzle}", [ (LanguageId.English, "Gas Can Silencer"), (LanguageId.SimplifiedChinese, "气罐消音器") ] },
                { $@"entityname.{Identifiers.VGM_LongBarrelMuzzle}", [ (LanguageId.English, "Extended Barrel"), (LanguageId.SimplifiedChinese, "延长枪管") ] },
                { $@"entityname.{Identifiers.VGM_FlashHiderMuzzle}", [ (LanguageId.English, "Flash Hider"), (LanguageId.SimplifiedChinese, "消焰器") ] },

                { $@"entityname.{Identifiers.VGM_AngledForeGrip}", [ (LanguageId.English, "Angled Foregrip"), (LanguageId.SimplifiedChinese, "直角握把") ] },
                { $@"entityname.{Identifiers.VGM_VerticalGrip}", [ (LanguageId.English, "Vertical Grip"), (LanguageId.SimplifiedChinese, "垂直握把") ] },

                { $@"entityname.{Identifiers.VGM_RedDotSight}", [ (LanguageId.English, "Red Dot Sight"), (LanguageId.SimplifiedChinese, "红点瞄准镜") ] },
                { $@"entityname.{Identifiers.VGM_HolographicSight}", [ (LanguageId.English, "Holographic Sight"), (LanguageId.SimplifiedChinese, "全息瞄准镜") ] },
                { $@"entityname.{Identifiers.VGM_ACOGScope}", [ (LanguageId.English, "4× Scope"), (LanguageId.SimplifiedChinese, "四倍瞄准镜") ] },
                { $@"entityname.{Identifiers.VGM_RifleScope}", [ (LanguageId.English, "Rifle Stock"), (LanguageId.SimplifiedChinese, "步枪瞄准镜") ] },
                { $@"entityname.{Identifiers.VGM_SniperScope}", [ (LanguageId.English, "Sniper Scope"), (LanguageId.SimplifiedChinese, "狙击瞄准镜") ] },

                { $@"entityname.{Identifiers.VGM_HealthScanner}", [ (LanguageId.English, "Health Scanner"), (LanguageId.SimplifiedChinese, "医疗扫描仪") ] },
                { $@"entityname.{Identifiers.VGM_ThermalScanner}", [ (LanguageId.English, "Thermal Scanner"), (LanguageId.SimplifiedChinese, "热成像仪") ] },

                { $@"gunstatname.weapondamage", [ (LanguageId.English, "Weapon Damage"), (LanguageId.SimplifiedChinese, "武器伤害") ] },
                { $@"gunstatname.armorpenetration", [ (LanguageId.English, "Armor Penetration"), (LanguageId.SimplifiedChinese, "穿甲") ] },
                { $@"gunstatname.reload", [ (LanguageId.English, "Reload"), (LanguageId.SimplifiedChinese, "装填") ] },
                { $@"gunstatname.minimumspread", [ (LanguageId.English, "Minimum Spread"), (LanguageId.SimplifiedChinese, "散布下限") ] },
                { $@"gunstatname.spreadchangeswhenaimdownsight", [ (LanguageId.English, "Spread Changes When ADS"), (LanguageId.SimplifiedChinese, "举枪时散布变化") ] },
                { $@"gunstatname.spreadrecoveryrate", [ (LanguageId.English, "Spread Recovery Rate"), (LanguageId.SimplifiedChinese, "散布回正速度") ] },
                { $@"gunstatname.spreadchangeswhenfire", [ (LanguageId.English, "Spread Changes When Fire"), (LanguageId.SimplifiedChinese, "开火时散布变化") ] },
                { $@"gunstatname.recoil", [ (LanguageId.English, "Recoil"), (LanguageId.SimplifiedChinese, "后坐力") ] },
                { $@"gunstatname.stockrecoilreduction", [ (LanguageId.English, "Stock's Recoil Reduction"), (LanguageId.SimplifiedChinese, "枪托的后坐力减少") ] },
                { $@"gunstatname.movementspeed", [ (LanguageId.English, "Movement Speed"), (LanguageId.SimplifiedChinese, "移动速度") ] },

                { $@"gunmodsstatname.stockrecoilreduction", [ (LanguageId.English, "Recoil Reduction"), (LanguageId.SimplifiedChinese, "后坐力减少") ] },
                { $@"gunmodsstatname.movementspeedmodifier", [ (LanguageId.English, "Movement Speed"), (LanguageId.SimplifiedChinese, "移动速度") ] },
                { $@"gunmodsstatname.weapondamagemodifier", [ (LanguageId.English, "Weapon Damage"), (LanguageId.SimplifiedChinese, "武器伤害") ] },
                { $@"gunmodsstatname.armorpenetrationmodifier", [ (LanguageId.English, "Armor Penetration"), (LanguageId.SimplifiedChinese, "穿甲") ] },
                { $@"gunmodsstatname.spreadchangeswhenfiremodifier", [ (LanguageId.English, "Spread Changes When Fire"), (LanguageId.SimplifiedChinese, "开火时散布变化") ] },
                { $@"gunmodsstatname.spreadchangeswhenaimdownsightmodifier", [ (LanguageId.English, "Spread Changes When ADS"), (LanguageId.SimplifiedChinese, "举枪时散布变化") ] },
                { $@"gunmodsstatname.spreadrecoveryratemodifier", [ (LanguageId.English, "Spread Recovery Rate"), (LanguageId.SimplifiedChinese, "散布回正速度") ] },
                { $@"gunmodsstatname.minimumspreadonrecoveringmodifier", [ (LanguageId.English, "Minimum Spread"), (LanguageId.SimplifiedChinese, "散布下限") ] },
                { $@"gunmodsstatname.obstructvisionamount", [ (LanguageId.English, "Obstruct Vision"), (LanguageId.SimplifiedChinese, "视野阻挡") ] },
            }.ToImmutableDictionary();
        }

#pragma warning disable CS8619
        public static IEnumerable<KeyValuePair<string, LanguageId>> AvailableLanguages => Enum.GetValues<LanguageId>().Select(id => KeyValuePair.Create(Enum.GetName(id), id));
#pragma warning restore CS8619

        public static string TextFileRootXMLAttributesString => Language switch
        {
            LanguageId.English => $@"language=""English"" nowhitespace=""true"" translatedname=""English""",
            LanguageId.SimplifiedChinese => $@"language=""Simplified Chinese"" nowhitespace=""true"" translatedname=""中文(简体)""",
            _ => throw new NotImplementedException(),
        };

        public static LanguageId Language { get; set; } = LanguageId.English;

        public static void SetLanguage(LanguageId id) => Language = id;
        public static string Get(string tag)
        {
            return languageTexts.TryGetValue(tag, out var values)
            && values.FirstOrDefault(v => v.Item1 == Language) is var value
                ? value.Item2 : throw new Exception($@"Not found any localized string by key '{tag}' in language '{Enum.GetName(Language)}'.");
        }



    }
}
