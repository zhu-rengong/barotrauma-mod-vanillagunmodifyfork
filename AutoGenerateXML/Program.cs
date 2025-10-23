using AutoGenerateXML;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Xml.Linq;

Dictionary<string, Dictionary<string, StringBuilder>> xmlsStringStore = new();

Assembly.GetExecutingAssembly().GetTypes()
    .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(ObjectXMLGenerator)))
    .ToList()
    .OrderBy(t => t switch
    {
        { Name: nameof(GunXMLGenerator) } => 100,
        _ => 10,
    })
    .ForEach(t =>
    {
        if (Activator.CreateInstance(t) is ObjectXMLGenerator gen)
        {
            ObjectXMLGenerator.List.Add(gen);

            switch (gen)
            {
                case GunXMLGenerator gun:
                    GunXMLGenerator.All.Add(gun.Identifier, gun);
                    break;
                case GripXMLGenerator grip:
                    GripXMLGenerator.All.Add(grip.Identifier, grip);
                    break;
                case StockXMLGenerator stock:
                    StockXMLGenerator.All.Add(stock.Identifier, stock);
                    break;
                case MuzzleXMLGenerator muzzle:
                    MuzzleXMLGenerator.All.Add(muzzle.Identifier, muzzle);
                    break;
                case AimingDeviceXMLGenerator aimingDevice:
                    AimingDeviceXMLGenerator.All.Add(aimingDevice.Identifier, aimingDevice);
                    break;
                case ScannerXMLGenerator scanner:
                    ScannerXMLGenerator.All.Add(scanner.Identifier, scanner);
                    break;
                default:
                    break;
            }
        }
    });

ObjectXMLGenerator.List.Sort((t1, t2) =>
{
    return t1.GetType().Name.CompareTo(t2.GetType().Name);
});

GunXMLGenerator.All.Values.ForEach(gun =>
{
    if (gun.ContainableGrips is not null)
    {
        GripXMLGenerator.All.Values
            .Where(grip => gun.ContainableGrips.Any(containable => containable.Grip == grip))
            .ForEach(grip => grip.SelfTags.Add($@"{Tags.VGM_Grip}Attr{gun.Name}Compatible"));
    }

    if (gun.ContainableStocks is not null)
    {
        StockXMLGenerator.All.Values
            .Where(stock => gun.ContainableStocks.Any(containable => containable.Stock == stock))
            .ForEach(stock => stock.SelfTags.Add($@"{Tags.VGM_Stock}Attr{gun.Name}Compatible"));
    }

    if (gun.ContainableMuzzles is not null)
    {
        MuzzleXMLGenerator.All.Values
            .Where(muzzle => gun.ContainableMuzzles.Any(containable => containable.Muzzle == muzzle))
            .ForEach(muzzle => muzzle.SelfTags.Add($@"{Tags.VGM_Muzzle}Attr{gun.Name}Compatible"));
    }

    if (gun.ContainableAimingDevices is not null)
    {
        AimingDeviceXMLGenerator.All.Values
            .Where(aimingDevice => gun.ContainableAimingDevices.Any(containable => containable.AimingDevice == aimingDevice))
            .ForEach(aimingDevice => aimingDevice.SelfTags.Add($@"{Tags.VGM_AimingDevice}Attr{gun.Name}Compatible"));
    }

    if (gun.ContainableScanners is not null)
    {
        ScannerXMLGenerator.All.Values
            .Where(scanner => gun.ContainableScanners.Any(containable => containable.Scanner == scanner))
            .ForEach(scanner => scanner.SelfTags.Add($@"{Tags.VGM_Scanner}Attr{gun.Name}Compatible"));
    }
});

MuzzleXMLGenerator.All.Values.ForEach(muzzle =>
{
    if (muzzle.IsSuppressor)
    {
        muzzle.SelfTags.Add(Tags.VGM_MuzzleAttrSuppressor);
    }

    if (muzzle.IsFlashHider)
    {
        muzzle.SelfTags.Add(Tags.VGM_MuzzleAttrFlashHider);
    }

    if (muzzle.SpreadChangesOnShootMultiplier.HasValue)
    {
        muzzle.SelfTags.Add(Tags.VGM_MuzzleAttrOverrideSpreadChangesOnShoot);
    }
});

void Generate(ObjectXMLGenerator gen)
{
    string xmlsString = gen.Generate();

    string filePath = CleanUpPathCrossPlatform(Path.Combine(UserDefinedGlobal.WorkingDirectory, UserDefinedGlobal.ContentFolder, gen.OutputPath));
    string folder = CleanUpPathCrossPlatform(Path.GetDirectoryName(filePath));
    if (!Directory.Exists(folder)) { Directory.CreateDirectory(folder); }

    if (!xmlsStringStore.TryGetValue(gen.Class, out var fileXMLsString))
    {
        xmlsStringStore.Add(gen.Class, fileXMLsString = new());
    }

    if (!fileXMLsString.TryGetValue(filePath, out var stringBuilder))
    {
        fileXMLsString.Add(filePath, stringBuilder = new());
    }

    stringBuilder.AppendLine(xmlsString);

    Console.WriteLine($"Generated {gen}");
}

ObjectXMLGenerator.List.ForEach(Generate);

xmlsStringStore.ForEach(kv =>
{
    string @class = kv.Key;
    var fileXMLsString = kv.Value;
    fileXMLsString.ForEach(kv2 =>
    {
        string filePath = kv2.Key;
        var stringBuilder = kv2.Value;
        var xml = XElement.Parse(
$@"
<{@class}s>
    <!-- Auto Generated -->
    {stringBuilder}
</{@class}s>");
        xml.Save(
            fileName: filePath,
            options: SaveOptions.None
        );
    });
});


foreach (var gun in GunXMLGenerator.All.Values)
{
    try
    {
        gun.VerifyPotentialErrors();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error Processing {gun.Name}: {ex.Message}");
        return;
    }
}

foreach (var (languageName, languageId) in TextManager.AvailableLanguages)
{
    TextManager.SetLanguage(languageId);

    {
        string savePath = CleanUpPathCrossPlatform(Path.Combine(UserDefinedGlobal.WorkingDirectory, UserDefinedGlobal.ContentFolder, "Languages", languageName, $@"{languageName}GunsAutoGenerated.xml"));
        string saveFolder = CleanUpPathCrossPlatform(Path.GetDirectoryName(savePath));
        if (!Directory.Exists(saveFolder)) { Directory.CreateDirectory(saveFolder); }

        StringBuilder stringBuilder = new StringBuilder();
        var gunList = GunXMLGenerator.All.Values.ToList();
        gunList.Sort((g1, g2) => { return g1.Identifier.CompareTo(g2.Identifier); });
        gunList.ForEach(gun =>
        {
            stringBuilder.AppendLine($@"<entityname.{gun.Identifier}>{TextManager.Get($@"entityname.{gun.Identifier}")}</entityname.{gun.Identifier}>");

            stringBuilder.Append($@"<entitydescription.{gun.Identifier}>");

            stringBuilder.Append($@"‖color:gui.orange‖{TextManager.Get("gunstatname.weapondamage")}:‖end‖ {gun.WeaponDamageModifier:0.##%}\n");
            stringBuilder.Append($@"‖color:gui.orange‖{TextManager.Get("gunstatname.armorpenetration")}:‖end‖ {gun.Penetration:0.##%}\n");
            stringBuilder.Append($@"‖color:gui.orange‖{TextManager.Get("gunstatname.reload")}:‖end‖ {gun.Reload}s{(gun.ReloadSkillRequirement.HasValue ? $@"~{gun.ReloadNoSkill.Value}s" : string.Empty)}\n");
            stringBuilder.Append($@"‖color:gui.orange‖{TextManager.Get("gunstatname.minimumspread")}:‖end‖ {gun.MinimumSpread}°~{gun.MinimumUnskilledSpread}°\n");
            stringBuilder.Append($@"‖color:gui.orange‖{TextManager.Get("gunstatname.spreadchangeswhenaimdownsight")}:‖end‖ {gun.SpreadChangesOnAimDownSight:+0.##;-0.##;0}°\n");
            stringBuilder.Append($@"‖color:gui.orange‖{TextManager.Get("gunstatname.spreadrecoveryrate")}:‖end‖ {gun.SpreadRecovery * 60.0f}°/s\n");
            stringBuilder.Append($@"‖color:gui.orange‖{TextManager.Get("gunstatname.spreadchangeswhenfire")}:‖end‖ {gun.SpreadChangesOnShoot:+0.##;-0.##;0}°\n");
            stringBuilder.Append($@"‖color:gui.orange‖{TextManager.Get("gunstatname.recoil")}:‖end‖ {gun.Recoil}\n");
            if (gun.ContainableStocks is not null) { stringBuilder.Append($@"‖color:gui.orange‖{TextManager.Get("gunstatname.stockrecoilreduction")}‖end‖ ×{gun.StockRecoilReductionEfficiency}\n"); }
            stringBuilder.Append($@"‖color:gui.orange‖{TextManager.Get("gunstatname.movementspeed")}:‖end‖ {gun.StocklessSpeedMultiplier:0.##%}\n");

            stringBuilder.Replace(@"\n", "", stringBuilder.Length - 2, 2);
            stringBuilder.AppendLine($@"</entitydescription.{gun.Identifier}>");
        });

        var xmlElementString = XElement.Parse(
$@"<infotexts {TextManager.TextFileRootXMLAttributesString}>
    <!-- Auto Generated -->
    {stringBuilder.ToString()}
</infotexts>");

        using (StreamWriter write = new(savePath, false, Encoding.UTF8))
        {
            write.Write(xmlElementString);
        }
    }

    {
        string savePath = CleanUpPathCrossPlatform(Path.Combine(UserDefinedGlobal.WorkingDirectory, UserDefinedGlobal.ContentFolder, "Languages", languageName, $@"{languageName}GunModsAutoGenerated.xml"));
        string saveFolder = CleanUpPathCrossPlatform(Path.GetDirectoryName(savePath));
        if (!Directory.Exists(saveFolder)) { Directory.CreateDirectory(saveFolder); }

        StringBuilder stringBuilder = new StringBuilder();

        {
            var gripList = GripXMLGenerator.All.Values.ToList();
            gripList.Sort((grip1, grip2) => { return grip1.Identifier.CompareTo(grip2.Identifier); });
            gripList.ForEach(grip =>
            {
                stringBuilder.AppendLine($@"<entityname.{grip.Identifier}>{TextManager.Get($@"entityname.{grip.Identifier}")}</entityname.{grip.Identifier}>");

                stringBuilder.Append($@"<entitydescription.{grip.Identifier}>");

                if (grip.SpreadChangesOnAimDownSightMultiplier.HasValue)
                {
                    stringBuilder.Append($@"‖color:gui.orange‖{TextManager.Get("gunmodsstatname.spreadchangeswhenaimdownsightmodifier")}‖end‖ ×{grip.SpreadChangesOnAimDownSightMultiplier.Value}\n");
                }

                if (grip.SpreadRecoveryMultiplier.HasValue)
                {
                    stringBuilder.Append($@"‖color:gui.orange‖{TextManager.Get("gunmodsstatname.spreadrecoveryratemodifier")}‖end‖ ×{grip.SpreadRecoveryMultiplier.Value}\n");
                }

                if (grip.MinimumSpreadOnRecoveringMultiplier.HasValue)
                {
                    stringBuilder.Append($@"‖color:gui.orange‖{TextManager.Get("gunmodsstatname.minimumspreadonrecoveringmodifier")}‖end‖ ×{grip.MinimumSpreadOnRecoveringMultiplier.Value}\n");
                }

                stringBuilder.Replace(@"\n", "", stringBuilder.Length - 2, 2);
                stringBuilder.AppendLine($@"</entitydescription.{grip.Identifier}>");
            });
        }

        {
            var stockList = StockXMLGenerator.All.Values.ToList();
            stockList.Sort((stock1, stock2) => { return stock1.Identifier.CompareTo(stock2.Identifier); });
            stockList.ForEach(stock =>
            {
                stringBuilder.AppendLine($@"<entityname.{stock.Identifier}>{TextManager.Get($@"entityname.{stock.Identifier}")}</entityname.{stock.Identifier}>");

                stringBuilder.Append($@"<entitydescription.{stock.Identifier}>");

                if (stock.RecoilReduction.HasValue)
                {
                    stringBuilder.Append($@"‖color:gui.orange‖{TextManager.Get("gunmodsstatname.stockrecoilreduction")}‖end‖: {stock.RecoilReduction.Value:0.##}\n");
                }

                if (stock.StocklessBasedSpeedMultiplier.HasValue)
                {
                    stringBuilder.Append($@"‖color:gui.orange‖{TextManager.Get("gunmodsstatname.movementspeedmodifier")}‖end‖ ×{stock.StocklessBasedSpeedMultiplier.Value}\n");
                }

                stringBuilder.Replace(@"\n", "", stringBuilder.Length - 2, 2);
                stringBuilder.AppendLine($@"</entitydescription.{stock.Identifier}>");
            });
        }

        {
            var muzzleList = MuzzleXMLGenerator.All.Values.ToList();
            muzzleList.Sort((gen1, gen2) => { return gen1.Identifier.CompareTo(gen2.Identifier); });
            muzzleList.ForEach(muzzle =>
            {
                stringBuilder.AppendLine($@"<entityname.{muzzle.Identifier}>{TextManager.Get($@"entityname.{muzzle.Identifier}")}</entityname.{muzzle.Identifier}>");

                stringBuilder.Append($@"<entitydescription.{muzzle.Identifier}>");

                if (muzzle.WeaponDamageMultiplier.HasValue)
                {
                    stringBuilder.Append($@"‖color:gui.orange‖{TextManager.Get("gunmodsstatname.weapondamagemodifier")}‖end‖ ×{muzzle.WeaponDamageMultiplier.Value}\n");
                }

                if (muzzle.PenetrationModifier.HasValue)
                {
                    stringBuilder.Append($@"‖color:gui.orange‖{TextManager.Get("gunmodsstatname.armorpenetrationmodifier")}‖end‖ {muzzle.PenetrationModifier.Value:+0.##%;-0.##%;0%}\n");
                }

                if (muzzle.SpreadChangesOnShootMultiplier.HasValue)
                {
                    stringBuilder.Append($@"‖color:gui.orange‖{TextManager.Get("gunmodsstatname.spreadchangeswhenfiremodifier")}‖end‖ ×{muzzle.SpreadChangesOnShootMultiplier.Value}\n");
                }

                if (muzzle.SpreadChoke.HasValue)
                {
                    stringBuilder.Append($@"‖color:gui.orange‖{TextManager.Get("gunmodsstatname.maximumshotschoke")}‖end‖: {muzzle.SpreadChoke.Value}°\n");
                }

                stringBuilder.Replace(@"\n", "", stringBuilder.Length - 2, 2);
                stringBuilder.AppendLine($@"</entitydescription.{muzzle.Identifier}>");
            });
        }

        {
            var aimingDeviceList = AimingDeviceXMLGenerator.All.Values.ToList();
            aimingDeviceList.Sort((gen1, gen2) => { return gen1.Identifier.CompareTo(gen2.Identifier); });
            aimingDeviceList.ForEach(aimingDevice =>
            {
                stringBuilder.AppendLine($@"<entityname.{aimingDevice.Identifier}>{TextManager.Get($@"entityname.{aimingDevice.Identifier}")}</entityname.{aimingDevice.Identifier}>");

                stringBuilder.Append($@"<entitydescription.{aimingDevice.Identifier}>");

                if (aimingDevice.SpreadChangesOnAimDownSightMultiplier.HasValue)
                {
                    stringBuilder.Append($@"‖color:gui.orange‖{TextManager.Get("gunmodsstatname.spreadchangeswhenaimdownsightmodifier")}‖end‖ ×{aimingDevice.SpreadChangesOnAimDownSightMultiplier.Value}\n");
                }

                if (aimingDevice.SpreadRecoveryMultiplier.HasValue)
                {
                    stringBuilder.Append($@"‖color:gui.orange‖{TextManager.Get("gunmodsstatname.spreadrecoveryratemodifier")}‖end‖ ×{aimingDevice.SpreadRecoveryMultiplier.Value}\n");
                }

                if (aimingDevice.MinimumSpreadOnRecoveringMultiplier.HasValue)
                {
                    stringBuilder.Append($@"‖color:gui.orange‖{TextManager.Get("gunmodsstatname.minimumspreadonrecoveringmodifier")}‖end‖ ×{aimingDevice.MinimumSpreadOnRecoveringMultiplier.Value}\n");
                }

                if (aimingDevice.ObstructVisionAmount.HasValue)
                {
                    stringBuilder.Append($@"‖color:gui.orange‖{TextManager.Get("gunmodsstatname.obstructvisionamount")}‖end‖: {aimingDevice.ObstructVisionAmount.Value:0.##%}\n");
                }

                stringBuilder.Replace(@"\n", "", stringBuilder.Length - 2, 2);
                stringBuilder.AppendLine($@"</entitydescription.{aimingDevice.Identifier}>");
            });
        }

        var xmlElementString = XElement.Parse(
$@"<infotexts {TextManager.TextFileRootXMLAttributesString}>
    <!-- Auto Generated -->
    {stringBuilder.ToString()}
</infotexts>");

        using (StreamWriter write = new(savePath, false, Encoding.UTF8))
        {
            write.Write(xmlElementString);
        }
    }
}

Console.ReadKey();


static string CleanUpPathCrossPlatform([AllowNull] string path, string directory = "")
{
    if (string.IsNullOrEmpty(path)) { return ""; }

    path = path
        .Replace('\\', '/');
    if (path.StartsWith("file:", StringComparison.OrdinalIgnoreCase))
    {
        path = path.Substring("file:".Length);
    }
    while (path.IndexOf("//") >= 0)
    {
        path = path.Replace("//", "/");
    }

    return path;
}