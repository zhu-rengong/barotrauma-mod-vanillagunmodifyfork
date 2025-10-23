namespace AutoGenerateXML
{
    public static class ItemXMLExtensions
    {
        public static string ConcatValues<T>(IEnumerable<T> objects) => string.Join(",", objects);
        public static string GetTickDurationString(int tick) => (tick * 1.0f / 60.0f).ToString("0.0000").Substring(0, "0.000".Length);

        public static string GenerateItemSpriteXMLString(
            string texture,
            float[] sourceRect,
            float depth,
            float[] origin)
        {
            return
$@"<Sprite
texture=""{texture}""
sourcerect=""{ConcatValues(sourceRect)}""
depth=""{depth}""
origin=""{ConcatValues(origin)}""/>";
        }

        public static string GenerateRectangleBodyXMLString(float width, float height, float density)
        {
            return
$@"<Body
width=""{width}""
height=""{height}""
density=""{density}""/>";
        }

        public static string FollowedByModPrefix(this string name)
        {
            return $@"{UserDefinedGlobal.ModNameAbbreviation} {name}";
        }

        public static string GenerateSlotIconStockXMLString(int index)
        {
            return $@"<SlotIcon slotindex=""{index}"" texture=""%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/UI/icons.png"" sourcerect=""64,0,64,64"" origin=""0.5,0.5"" />";
        }

        public static string GenerateSlotIconMuzzleXMLString(int index)
        {
            return $@"<SlotIcon slotindex=""{index}"" texture=""%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/UI/icons.png"" sourcerect=""256,0,64,64"" origin=""0.5,0.5"" />";
        }

        public static string GenerateSlotIconScannerXMLString(int index)
        {
            return $@"<SlotIcon slotindex=""{index}"" texture=""%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/UI/icons_betadev.png"" sourcerect=""0,0,64,64"" origin=""0.5,0.5"" />";
        }

        public static string GenerateSlotIconAimingDeviceXMLString(int index)
        {
            return $@"<SlotIcon slotindex=""{index}"" texture=""%ModDir%/EuropaArmedGroupCommunity/VanillaGunModify/UI/icons.png"" sourcerect=""128,0,64,64"" origin=""0.5,0.5"" />";
        }

        public static string GenerateSlotIconBulletsXMLString(int index)
        {
            return $@"<SlotIcon slotindex=""{index}"" texture=""Content/UI/StatusMonitorUI.png"" sourcerect=""256,448,64,64"" origin=""0.5,0.5"" />";
        }

        public static string GenerateSlotIconFlashlightXMLString(int index)
        {
            return $@"<SlotIcon slotindex=""{index}"" texture=""Content/UI/StatusMonitorUI.png"" sourcerect=""320,448,64,64"" origin=""0.5,0.5"" />";
        }

        public static string GenerateAccessoryThrowableXMLsString()
        {
            return $@"<Throwable characterusable=""false"" slots=""Any,LeftHand,RightHand"" throwforce=""4.5"" aimpos=""35,-10"" holdangle=""70"" msg=""ItemMsgPickUpSelect"" />";
        }

        public static string GenerateAccessoryPreferredContainerXMLsString()
        {
            return $@"<PreferredContainer primary=""armcab"" secondary=""secarmcab"" />";
        }
    }
}
