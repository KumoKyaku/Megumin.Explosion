using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Megumin;

namespace Megumin
{
    public static class MeguminColorUtility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlString"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        /// <remarks>与<see cref="ColorUtility"/>结果一致，并可以在初始化时使用</remarks>
        public static bool TryParseHtmlString(string htmlString, out Color color)
        {
            int startIndex = 0;
            if (htmlString.StartsWith("#"))
            {
                startIndex = 1;
            }
            bool ret = htmlString.TryPerseHexColor(out var r, out var g, out var b, out var a, startIndex);

            //color = new Color32(r, g, b, a);
            color = new Color(r / 255f, g / 255f, b / 255f, a / 255f);
            return ret;
        }

        public static bool TryParseHtmlString2Color32(string htmlString, out Color32 color)
        {
            int startIndex = 0;
            if (htmlString.StartsWith("#"))
            {
                startIndex = 1;
            }
            bool ret = htmlString.TryPerseHexColor(out var r, out var g, out var b, out var a, startIndex);

            color = new Color32(r, g, b, a);
            //color = new Color(r / 255f, g / 255f, b / 255f, a / 255f);
            return ret;
        }

        public static string ToHtmlStringRGB(Color color)
        {
            // Round to int to prevent precision issues that, for example cause values very close to 1 to become FE instead of FF (case 770904).
            Color32 col32 = new Color32(
                (byte)Mathf.Clamp(Mathf.RoundToInt(color.r * 255), 0, 255),
                (byte)Mathf.Clamp(Mathf.RoundToInt(color.g * 255), 0, 255),
                (byte)Mathf.Clamp(Mathf.RoundToInt(color.b * 255), 0, 255),
                1);

            return string.Format("{0:X2}{1:X2}{2:X2}", col32.r, col32.g, col32.b);
        }

        public static string ToHtmlStringRGBA(Color color)
        {
            // Round to int to prevent precision issues that, for example cause values very close to 1 to become FE instead of FF (case 770904).
            Color32 col32 = new Color32(
                (byte)Mathf.Clamp(Mathf.RoundToInt(color.r * 255), 0, 255),
                (byte)Mathf.Clamp(Mathf.RoundToInt(color.g * 255), 0, 255),
                (byte)Mathf.Clamp(Mathf.RoundToInt(color.b * 255), 0, 255),
                (byte)Mathf.Clamp(Mathf.RoundToInt(color.a * 255), 0, 255));

            return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", col32.r, col32.g, col32.b, col32.a);
        }
    }
}

namespace UnityEngine
{
    /// <summary>
    /// 16进制颜色 (长度为8的RGBA16进制字符串)
    /// <see cref="ColorUtility.TryParseHtmlString(string, out Color)"/>
    /// <para></para>todo 支持HDR
    /// </summary>
    public partial struct HexColor
    {
        public string hexCode;
        public HexColor(string hex)
        {
            if (hex.StartsWith("#"))
            {
                if (hex.Length != 9 && hex.Length != 7)
                {
                    throw new ArgumentException("格式不对");
                }
            }
            else
            {
                if (hex.Length == 6 || hex.Length == 8)
                {
                    hex = "#" + hex;
                }
                else
                {
                    throw new ArgumentException("格式不对");
                }
            }

            hexCode = hex;
        }

        public static HexColor Parse(string hexcode)
        {
            return new HexColor(hexcode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Color32(HexColor c)
        {
            if (MeguminColorUtility.TryParseHtmlString2Color32(c.hexCode, out Color32 color))
            {
                return color;
            }
            throw new FormatException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Color(HexColor c)
        {
            if (MeguminColorUtility.TryParseHtmlString(c.hexCode, out Color color))
            {
                return color;
            }
            throw new FormatException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HexColor(Color c)
        {
            return $"#{ColorUtility.ToHtmlStringRGBA(c)}";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HexColor(Color32 c)
        {
            return $"#{ColorUtility.ToHtmlStringRGBA(c)}";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HexColor(string hexCode)
        {
            return new HexColor(hexCode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator string(HexColor c)
        {
            return c.hexCode;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HSVColor(in HexColor hex)
        {
            return (Color)hex;
        }

        /// <summary>
        /// 使用Html标签包裹对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="color"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public string Html<T>(T target)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGBA(this)}>{target}</color>";
        }

        public override string ToString()
        {
            return hexCode;
        }

        public string ToHtmlString()
        {
            return this.Html(hexCode);
        }
    }

    #region CommonColors

    public partial struct HexColor
    {
        public static IEnumerable<(string Name, HexColor)> GetAllStaticColor()
        {
            var ret = from field in typeof(HexColor).GetFields()
                      where field.FieldType == typeof(HexColor)
                      select (field.Name, (HexColor)field.GetValue(null));
            return ret;
        }

        public static void DebugLogColor(string str = "控制台文字颜色")
        {
            foreach (var item in GetAllStaticColor())
            {
                Debug.Log(item.Item2.Html($"{str}    {item.Name}"));
            }
        }


#if UNITY_EDITOR

        [UnityEditor.MenuItem("Tools/Style/Log with HexColor")]
        static void MenuItem()
        {
            foreach (var item in GetAllStaticColor())
            {
                Debug.Log(item.Item2.Html($"HexColor.<b>{item.Name}</b>.Html(\"Hello World!\")"));
            }
        }

#endif

    }

    /// <summary>
    /// https://forum.unity.com/threads/extending-static-colors.379093/
    /// </summary>
    public partial struct HexColor
    {
        public static readonly HexColor AbsoluteZero = "#0048BA";
        public static readonly HexColor AcidGreen = "#B0BF1A";
        public static readonly HexColor Aero = "#7CB9E8";
        public static readonly HexColor AeroBlue = "#C9FFE5";
        public static readonly HexColor AfricanViolet = "#B284BE";
        public static readonly HexColor AirForceBlueRAF = "#5D8AA8";
        public static readonly HexColor AirForceBlueUSAF = "#00308F";
        public static readonly HexColor AirSuperiorityBlue = "#72A0C1";
        public static readonly HexColor AlabamaCrimson = "#AF002A";
        public static readonly HexColor AliceBlue = "#F0F8FF";
        public static readonly HexColor AlienArmpit = "#84DE02";
        public static readonly HexColor AlizarinCrimson = "#E32636";
        public static readonly HexColor AlloyOrange = "#C46210";
        public static readonly HexColor Almond = "#EFDECD";
        public static readonly HexColor Amaranth = "#E52B50";
        public static readonly HexColor AmaranthDeepPurple = "#AB274F";
        public static readonly HexColor AmaranthPink = "#F19CBB";
        public static readonly HexColor AmaranthPurple = "#AB274F";
        public static readonly HexColor AmaranthRed = "#D3212D";
        public static readonly HexColor Amazon = "#3B7A57";
        public static readonly HexColor Amber = "#FFBF00";
        public static readonly HexColor AmberSAEECE = "#FF7E00";
        public static readonly HexColor AmericanRose = "#FF033E";
        public static readonly HexColor Amethyst = "#9966CC";
        public static readonly HexColor AndroidGreen = "#A4C639";
        public static readonly HexColor AntiFlashWhite = "#F2F3F4";
        public static readonly HexColor AntiqueBrass = "#CD9575";
        public static readonly HexColor AntiqueBronze = "#665D1E";
        public static readonly HexColor AntiqueFuchsia = "#915C83";
        public static readonly HexColor AntiqueRuby = "#841B2D";
        public static readonly HexColor AntiqueWhite = "#FAEBD7";
        public static readonly HexColor AoEnglish = "#008000";
        public static readonly HexColor AppleGreen = "#8DB600";
        public static readonly HexColor Apricot = "#FBCEB1";
        public static readonly HexColor Aqua = "#00FFFF";
        public static readonly HexColor Aquamarine = "#7FFFD4";
        public static readonly HexColor ArcticLime = "#D0FF14";
        public static readonly HexColor ArmyGreen = "#4B5320";
        public static readonly HexColor Arsenic = "#3B444B";
        public static readonly HexColor Artichoke = "#8F9779";
        public static readonly HexColor ArylideYellow = "#E9D66B";
        public static readonly HexColor AshGrey = "#B2BEB5";
        public static readonly HexColor Asparagus = "#87A96B";
        public static readonly HexColor AtomicTangerine = "#FF9966";
        public static readonly HexColor Auburn = "#A52A2A";
        public static readonly HexColor Aureolin = "#FDEE00";
        public static readonly HexColor AuroMetalSaurus = "#6E7F80";
        public static readonly HexColor Avocado = "#568203";
        public static readonly HexColor AztecGold = "#C39953";
        public static readonly HexColor Azure = "#007FFF";
        public static readonly HexColor AzureWebHexColor = "#F0FFFF";
        public static readonly HexColor AzureMist = "#F0FFFF";
        public static readonly HexColor AzureishWhite = "#DBE9F4";
        public static readonly HexColor BabyBlue = "#89CFF0";
        public static readonly HexColor BabyBlueEyes = "#A1CAF1";
        public static readonly HexColor BabyPink = "#F4C2C2";
        public static readonly HexColor BabyPowder = "#FEFEFA";
        public static readonly HexColor BakerMillerPink = "#FF91AF";
        public static readonly HexColor BallBlue = "#21ABCD";
        public static readonly HexColor BananaMania = "#FAE7B5";
        public static readonly HexColor BananaYellow = "#FFE135";
        public static readonly HexColor BangladeshGreen = "#006A4E";
        public static readonly HexColor BarbiePink = "#E0218A";
        public static readonly HexColor BarnRed = "#7C0A02";
        public static readonly HexColor BattleshipGrey = "#848482";
        public static readonly HexColor Bazaar = "#98777B";
        public static readonly HexColor BeauBlue = "#BCD4E6";
        public static readonly HexColor Beaver = "#9F8170";
        public static readonly HexColor Beige = "#2E5894";
        public static readonly HexColor BdazzledBlue = "#2E5894";
        public static readonly HexColor BigDipOruby = "#9C2542";
        public static readonly HexColor BigFootFeet = "#E88E5A";
        public static readonly HexColor Bisque = "#FFE4C4";
        public static readonly HexColor Bistre = "#3D2B1F";
        public static readonly HexColor BistreBrown = "#967117";
        public static readonly HexColor BitterLemon = "#CAE00D";
        public static readonly HexColor BitterLime = "#BFFF00";
        public static readonly HexColor Bittersweet = "#FE6F5E";
        public static readonly HexColor BittersweetShimmer = "#BF4F51";
        public static readonly HexColor Black = "#000000";
        public static readonly HexColor BlackBean = "#3D0C02";
        public static readonly HexColor BlackCoral = "#54626F";
        public static readonly HexColor BlackLeatherJacket = "#253529";
        public static readonly HexColor BlackOlive = "#3B3C36";
        public static readonly HexColor BlackShadows = "#BFAFB2";
        public static readonly HexColor BlanchedAlmond = "#FFEBCD";
        public static readonly HexColor BlastOffBronze = "#A57164";
        public static readonly HexColor BleuDeFrance = "#318CE7";
        public static readonly HexColor BlizzardBlue = "#ACE5EE";
        public static readonly HexColor Blond = "#FAF0BE";
        public static readonly HexColor Blue = "#0000FF";
        public static readonly HexColor BlueCrayola = "#1F75FE";
        public static readonly HexColor BlueMunsell = "#0093AF";
        public static readonly HexColor BlueNCS = "#0087BD";
        public static readonly HexColor BluePantone = "#0018A8";
        public static readonly HexColor BluePigment = "#333399";
        public static readonly HexColor BlueRYB = "#0247FE";
        public static readonly HexColor BlueBell = "#A2A2D0";
        public static readonly HexColor BlueGray = "#6699CC";
        public static readonly HexColor BlueGreen = "#0D98BA";
        public static readonly HexColor BlueJeans = "#5DADEC";
        public static readonly HexColor BlueLagoon = "#ACE5EE";
        public static readonly HexColor BlueMagentaViolet = "#553592";
        public static readonly HexColor BlueSapphire = "#126180";
        public static readonly HexColor BlueViolet = "#8A2BE2";
        public static readonly HexColor BlueYonder = "#5072A7";
        public static readonly HexColor Blueberry = "#4F86F7";
        public static readonly HexColor Bluebonnet = "#1C1CF0";
        public static readonly HexColor Blush = "#DE5D83";
        public static readonly HexColor Bole = "#79443B";
        public static readonly HexColor BondiBlue = "#0095B6";
        public static readonly HexColor Bone = "#E3DAC9";
        public static readonly HexColor BoogerBuster = "#DDE26A";
        public static readonly HexColor BostonUniversityRed = "#CC0000";
        public static readonly HexColor BottleGreen = "#006A4E";
        public static readonly HexColor Boysenberry = "#873260";
        public static readonly HexColor BrandeisBlue = "#0070FF";
        public static readonly HexColor Brass = "#B5A642";
        public static readonly HexColor BrickRed = "#CB4154";
        public static readonly HexColor BrightCerulean = "#1DACD6";
        public static readonly HexColor BrightGreen = "#66FF00";
        public static readonly HexColor BrightLavender = "#BF94E4";
        public static readonly HexColor BrightLilac = "#D891EF";
        public static readonly HexColor BrightMaroon = "#C32148";
        public static readonly HexColor BrightNavyBlue = "#1974D2";
        public static readonly HexColor BrightPink = "#FF007F";
        public static readonly HexColor BrightTurquoise = "#08E8DE";
        public static readonly HexColor BrightUbe = "#D19FE8";
        public static readonly HexColor BrightYellowCrayola = "#FFAA1D";
        public static readonly HexColor BrilliantAzure = "#3399FF";
        public static readonly HexColor BrilliantLavender = "#F4BBFF";
        public static readonly HexColor BrilliantRose = "#FF55A3";
        public static readonly HexColor BrinkPink = "#FB607F";
        public static readonly HexColor BritishRacingGreen = "#004225";
        public static readonly HexColor Bronze = "#CD7F32";
        public static readonly HexColor BronzeYellow = "#737000";
        public static readonly HexColor BrownTraditional = "#964B00";
        public static readonly HexColor BrownWeb = "#A52A2A";
        public static readonly HexColor BrownNose = "#6B4423";
        public static readonly HexColor BrownSugar = "#AF6E4D";
        public static readonly HexColor BrownYellow = "#cc9966";
        public static readonly HexColor BrunswickGreen = "#1B4D3E";
        public static readonly HexColor BubbleGum = "#FFC1CC";
        public static readonly HexColor Bubbles = "#E7FEFF";
        public static readonly HexColor BudGreen = "#7BB661";
        public static readonly HexColor Buff = "#F0DC82";
        public static readonly HexColor BulgarianRose = "#480607";
        public static readonly HexColor Burgundy = "#800020";
        public static readonly HexColor Burlywood = "#DEB887";
        public static readonly HexColor BurnishedBrown = "#A17A74";
        public static readonly HexColor BurntOrange = "#CC5500";
        public static readonly HexColor BurntSienna = "#E97451";
        public static readonly HexColor BurntUmber = "#8A3324";
        public static readonly HexColor Byzantine = "#BD33A4";
        public static readonly HexColor Byzantium = "#702963";
        public static readonly HexColor Cadet = "#536872";
        public static readonly HexColor CadetBlue = "#5F9EA0";
        public static readonly HexColor CadetGrey = "#91A3B0";
        public static readonly HexColor CadmiumGreen = "#006B3C";
        public static readonly HexColor CadmiumOrange = "#ED872D";
        public static readonly HexColor CadmiumRed = "#E30022";
        public static readonly HexColor CadmiumYellow = "#FFF600";
        public static readonly HexColor CafeAuLait = "#A67B5B";
        public static readonly HexColor CafeNoir = "#4B3621";
        public static readonly HexColor CalPolyGreen = "#1E4D2B";
        public static readonly HexColor CambridgeBlue = "#A3C1AD";
        public static readonly HexColor Camel = "#C19A6B";
        public static readonly HexColor CameoPink = "#EFBBCC";
        public static readonly HexColor CamouflageGreen = "#78866B";
        public static readonly HexColor CanaryYellow = "#FFEF00";
        public static readonly HexColor CandyAppleRed = "#FF0800";
        public static readonly HexColor CandyPink = "#E4717A";
        public static readonly HexColor Capri = "#00BFFF";
        public static readonly HexColor CaputMortuum = "#592720";
        public static readonly HexColor Cardinal = "#C41E3A";
        public static readonly HexColor CaribbeanGreen = "#00CC99";
        public static readonly HexColor Carmine = "#960018";
        public static readonly HexColor CarmineMP = "#D70040";
        public static readonly HexColor CarminePink = "#EB4C42";
        public static readonly HexColor CarmineRed = "#FF0038";
        public static readonly HexColor CarnationPink = "#FFA6C9";
        public static readonly HexColor Carnelian = "#B31B1B";
        public static readonly HexColor CarolinaBlue = "#56A0D3";
        public static readonly HexColor CarrotOrange = "#ED9121";
        public static readonly HexColor CastletonGreen = "#00563F";
        public static readonly HexColor CatalinaBlue = "#062A78";
        public static readonly HexColor Catawba = "#703642";
        public static readonly HexColor CedarChest = "#C95A49";
        public static readonly HexColor Ceil = "#92A1CF";
        public static readonly HexColor Celadon = "#ACE1AF";
        public static readonly HexColor CeladonBlue = "#007BA7";
        public static readonly HexColor CeladonGreen = "#2F847C";
        public static readonly HexColor Celeste = "#B2FFFF";
        public static readonly HexColor CelestialBlue = "#4997D0";
        public static readonly HexColor Cerise = "#DE3163";
        public static readonly HexColor CerisePink = "#EC3B83";
        public static readonly HexColor Cerulean = "#007BA7";
        public static readonly HexColor CeruleanBlue = "#2A52BE";
        public static readonly HexColor CeruleanFrost = "#6D9BC3";
        public static readonly HexColor CGBlue = "#007AA5";
        public static readonly HexColor CGRed = "#E03C31";
        public static readonly HexColor Chamoisee = "#A0785A";
        public static readonly HexColor Champagne = "#F7E7CE";
        public static readonly HexColor Charcoal = "#36454F";
        public static readonly HexColor CharlestonGreen = "#232B2B";
        public static readonly HexColor CharmPink = "#E68FAC";
        public static readonly HexColor ChartreuseTraditional = "#DFFF00";
        public static readonly HexColor ChartreuseWeb = "#7FFF00";
        public static readonly HexColor Cherry = "#DE3163";
        public static readonly HexColor CherryBlossomPink = "#FFB7C5";
        public static readonly HexColor Chestnut = "#954535";
        public static readonly HexColor ChinaPink = "#DE6FA1";
        public static readonly HexColor ChinaRose = "#A8516E";
        public static readonly HexColor ChineseRed = "#AA381E";
        public static readonly HexColor ChineseViolet = "#856088";
        public static readonly HexColor ChlorophyllGreen = "#4AFF00";
        public static readonly HexColor ChocolateTraditional = "#7B3F00";
        public static readonly HexColor ChocolateWeb = "#D2691E";
        public static readonly HexColor ChromeYellow = "#FFA700";
        public static readonly HexColor Cinereous = "#98817B";
        public static readonly HexColor Cinnabar = "#E34234";
        public static readonly HexColor CinnamonCitationNeeded = "#D2691E";
        public static readonly HexColor CinnamonSatin = "#CD607E";
        public static readonly HexColor Citrine = "#E4D00A";
        public static readonly HexColor Citron = "#9FA91F";
        public static readonly HexColor Claret = "#7F1734";
        public static readonly HexColor ClassicRose = "#FBCCE7";
        public static readonly HexColor CobaltBlue = "#0047AB";
        public static readonly HexColor CocoaBrown = "#D2691E";
        public static readonly HexColor Coconut = "#965A3E";
        public static readonly HexColor Coffee = "#6F4E37";
        public static readonly HexColor ColumbiaBlue = "#C4D8E2";
        public static readonly HexColor CongoPink = "#F88379";
        public static readonly HexColor CoolBlack = "#002E63";
        public static readonly HexColor CoolGrey = "#8C92AC";
        public static readonly HexColor Copper = "#B87333";
        public static readonly HexColor CopperCrayola = "#DA8A67";
        public static readonly HexColor CopperPenny = "#AD6F69";
        public static readonly HexColor CopperRed = "#CB6D51";
        public static readonly HexColor CopperRose = "#996666";
        public static readonly HexColor Coquelicot = "#FF3800";
        public static readonly HexColor Coral = "#FF7F50";
        public static readonly HexColor CoralPink = "#F88379";
        public static readonly HexColor CoralRed = "#FF4040";
        public static readonly HexColor Cordovan = "#893F45";
        public static readonly HexColor Corn = "#FBEC5D";
        public static readonly HexColor CornellRed = "#B31B1B";
        public static readonly HexColor CornfFowerBlue = "#6495ED";
        public static readonly HexColor CornSilk = "#FFF8DC";
        public static readonly HexColor CosmicCobalt = "#2E2D88";
        public static readonly HexColor CosmicLatte = "#FFF8E7";
        public static readonly HexColor CoyoteBrown = "#81613e";
        public static readonly HexColor CottonCandy = "#FFBCD9";
        public static readonly HexColor Cream = "#FFFDD0";
        public static readonly HexColor Crimson = "#DC143C";
        public static readonly HexColor CrimsonGlory = "#BE0032";
        public static readonly HexColor CrimsonRed = "#990000";
        public static readonly HexColor Cultured = "#F5F5F5";
        public static readonly HexColor Cyan = "#00FFFF";
        public static readonly HexColor CyanAzure = "#4E82b4";
        public static readonly HexColor CyanBlueAzure = "#4682BF";
        public static readonly HexColor CyanCobaltBlue = "#28589C";
        public static readonly HexColor CyanCornflowerBlue = "#188BC2";
        public static readonly HexColor CyanProcess = "#00B7EB";
        public static readonly HexColor CyberGrape = "#58427C";
        public static readonly HexColor CyberYellow = "#FFD300";
        public static readonly HexColor Cyclamen = "#F56FA1";
        public static readonly HexColor Daffodil = "#FFFF31";
        public static readonly HexColor Dandelion = "#F0E130";
        public static readonly HexColor DarkBlue = "#00008B";
        public static readonly HexColor DarkBlueGray = "#666699";
        public static readonly HexColor DarkBrown = "#654321";
        public static readonly HexColor DarkBrownTangelo = "#88654E";
        public static readonly HexColor DarkByzantium = "#5D3954";
        public static readonly HexColor DarkCandyAppleRed = "#A40000";
        public static readonly HexColor DarkCerulean = "#08457E";
        public static readonly HexColor DarkChestnut = "#986960";
        public static readonly HexColor DarkCoral = "#CD5B45";
        public static readonly HexColor DarkCyan = "#008B8B";
        public static readonly HexColor DarkElectricBlue = "#536878";
        public static readonly HexColor DarkGoldenrod = "#B8860B";
        public static readonly HexColor DarkGrayX = "#A9A9A9";
        public static readonly HexColor DarkGreen = "#013220";
        public static readonly HexColor DarkGreenX = "#006400";
        public static readonly HexColor DarkGunmetal = "#1F262A";
        public static readonly HexColor DarkImperialBlueLight = "#00416A";
        public static readonly HexColor DarkImperialBlue = "#6E6EF9";
        public static readonly HexColor DarkJungleGreen = "#1A2421";
        public static readonly HexColor DarkKhaki = "#BDB76B";
        public static readonly HexColor DarkLava = "#483C32";
        public static readonly HexColor DarkLavender = "#734F96";
        public static readonly HexColor DarkLiver = "#534B4F";
        public static readonly HexColor DarkLiverHorses = "#543D37";
        public static readonly HexColor DarkMagenta = "#8B008B";
        public static readonly HexColor DarkMediumGray = "#A9A9A9";
        public static readonly HexColor DarkMidnightBlue = "#003366";
        public static readonly HexColor DarkMossGreen = "#4A5D23";
        public static readonly HexColor DarkOliveGreen = "#556B2F";
        public static readonly HexColor DarkOrange = "#FF8C00";
        public static readonly HexColor DarkOrchid = "#9932CC";
        public static readonly HexColor DarkPastelBlue = "#779ECB";
        public static readonly HexColor DarkPastelGreen = "#03C03C";
        public static readonly HexColor DarkPastelPurple = "#966FD6";
        public static readonly HexColor DarkPastelRed = "#C23B22";
        public static readonly HexColor DarkPink = "#E75480";
        public static readonly HexColor DarkPowderBlue = "#003399";
        public static readonly HexColor DarkPuce = "#4F3A3C";
        public static readonly HexColor DarkPurple = "#301934";
        public static readonly HexColor DarkRaspberry = "#872657";
        public static readonly HexColor DarkRed = "#8B0000";
        public static readonly HexColor DarkSalmon = "#E9967A";
        public static readonly HexColor DarkScarlet = "#560319";
        public static readonly HexColor DarkSeaGreen = "#8FBC8F";
        public static readonly HexColor DarkSienna = "#3C1414";
        public static readonly HexColor DarkSkyBlue = "#8CBED6";
        public static readonly HexColor DarkSlateBlue = "#483D8B";
        public static readonly HexColor DarkSlateGray = "#2F4F4F";
        public static readonly HexColor DarkSpringGreen = "#177245";
        public static readonly HexColor DarkTan = "#918151";
        public static readonly HexColor DarkTangerine = "#FFA812";
        public static readonly HexColor DarkTaupe = "#483C32";
        public static readonly HexColor DarkTerraCotta = "#CC4E5C";
        public static readonly HexColor DarkTurquoise = "#00CED1";
        public static readonly HexColor DarkVanilla = "#D1BEA8";
        public static readonly HexColor DarkViolet = "#9400D3";
        public static readonly HexColor DarkYellow = "#9B870C";
        public static readonly HexColor DartmouthGreen = "#00703C";
        public static readonly HexColor DavysGrey = "#555555";
        public static readonly HexColor DebianRed = "#D70A53";
        public static readonly HexColor DeepAquamarine = "#40826D";
        public static readonly HexColor DeepCarmine = "#A9203E";
        public static readonly HexColor DeepCarminePink = "#EF3038";
        public static readonly HexColor DeepCarrotOrange = "#E9692C";
        public static readonly HexColor DeepCerise = "#DA3287";
        public static readonly HexColor DeepChampagne = "#FAD6A5";
        public static readonly HexColor DeepChestnut = "#B94E48";
        public static readonly HexColor DeepCoffee = "#704241";
        public static readonly HexColor DeepFuchsia = "#C154C1";
        public static readonly HexColor DeepGreen = "#056608";
        public static readonly HexColor DeepGreenCyanTurquoise = "#0E7C61";
        public static readonly HexColor DeepJungleGreen = "#004B49";
        public static readonly HexColor DeepKoamaru = "#333366";
        public static readonly HexColor DeepLemon = "#F5C71A";
        public static readonly HexColor DeepLilac = "#9955BB";
        public static readonly HexColor DeepMagenta = "#CC00CC";
        public static readonly HexColor DeepMaroon = "#820000";
        public static readonly HexColor DeepMauve = "#D473D4";
        public static readonly HexColor DeepMossGreen = "#355E3B";
        public static readonly HexColor DeepPeach = "#FFCBA4";
        public static readonly HexColor DeepPink = "#FF1493";
        public static readonly HexColor DeepPuce = "#A95C68";
        public static readonly HexColor DeepRed = "#850101";
        public static readonly HexColor DeepRuby = "#843F5B";
        public static readonly HexColor DeepSaffron = "#FF9933";
        public static readonly HexColor DeepSkyBlue = "#00BFFF";
        public static readonly HexColor DeepSpaceSparkle = "#4A646C";
        public static readonly HexColor DeepSpringBud = "#556B2F";
        public static readonly HexColor DeepTaupe = "#7E5E60";
        public static readonly HexColor DeepTuscanRed = "#66424D";
        public static readonly HexColor DeepViolet = "#330066";
        public static readonly HexColor Deer = "#BA8759";
        public static readonly HexColor Denim = "#1560BD";
        public static readonly HexColor DenimBlue = "#2243B6";
        public static readonly HexColor DesaturatedCyan = "#669999";
        public static readonly HexColor Desert = "#C19A6B";
        public static readonly HexColor DesertSand = "#EDC9AF";
        public static readonly HexColor Desire = "#EA3C53";
        public static readonly HexColor Diamond = "#B9F2FF";
        public static readonly HexColor DimGray = "#696969";
        public static readonly HexColor DingyDungeon = "#C53151";
        public static readonly HexColor Dirt = "#9B7653";
        public static readonly HexColor DodgerBlue = "#1E90FF";
        public static readonly HexColor DogwoodRose = "#D71868";
        public static readonly HexColor DollarBill = "#85BB65";
        public static readonly HexColor DonkeyBrown = "#664C28";
        public static readonly HexColor Drab = "#967117";
        public static readonly HexColor DukeBlue = "#00009C";
        public static readonly HexColor DustStorm = "#E5CCC9";
        public static readonly HexColor DutchWhite = "#EFDFBB";
        public static readonly HexColor EarthYellow = "#E1A95F";
        public static readonly HexColor Ebony = "#555D50";
        public static readonly HexColor Ecru = "#C2B280";
        public static readonly HexColor EerieBlack = "#1B1B1B";
        public static readonly HexColor Eggplant = "#614051";
        public static readonly HexColor Eggshell = "#F0EAD6";
        public static readonly HexColor EgyptianBlue = "#1034A6";
        public static readonly HexColor ElectricBlue = "#7DF9FF";
        public static readonly HexColor ElectricCrimson = "#FF003F";
        public static readonly HexColor ElectricCyan = "#00FFFF";
        public static readonly HexColor ElectricGreen = "#00FF00";
        public static readonly HexColor ElectricIndigo = "#6F00FF";
        public static readonly HexColor ElectricLavender = "#F4BBFF";
        public static readonly HexColor ElectricLime = "#CCFF00";
        public static readonly HexColor ElectricPurple = "#BF00FF";
        public static readonly HexColor ElectricUltramarine = "#3F00FF";
        public static readonly HexColor ElectricViolet = "#8F00FF";
        public static readonly HexColor ElectricYellow = "#FFFF33";
        public static readonly HexColor Emerald = "#50C878";
        public static readonly HexColor Eminence = "#6C3082";
        public static readonly HexColor EnglishGreen = "#1B4D3E";
        public static readonly HexColor EnglishLavender = "#B48395";
        public static readonly HexColor EnglishRed = "#AB4B52";
        public static readonly HexColor EnglishVermillion = "#CC474B";
        public static readonly HexColor EnglishViolet = "#563C5C";
        public static readonly HexColor ETHBlue = "#1F407A";
        public static readonly HexColor EtonBlue = "#96C8A2";
        public static readonly HexColor Eucalyptus = "#44D7A8";
        public static readonly HexColor Fallow = "#C19A6B";
        public static readonly HexColor FaluRed = "#801818";
        public static readonly HexColor Fandango = "#B53389";
        public static readonly HexColor FandangoPink = "#DE5285";
        public static readonly HexColor FashionFuchsia = "#F400A1";
        public static readonly HexColor Fawn = "#E5AA70";
        public static readonly HexColor Feldgrau = "#4D5D53";
        public static readonly HexColor Feldspar = "#FDD5B1";
        public static readonly HexColor FernGreen = "#4F7942";
        public static readonly HexColor FerrariRed = "#FF2800";
        public static readonly HexColor FieldDrab = "#6C541E";
        public static readonly HexColor FieryRose = "#FF5470";
        public static readonly HexColor Firebrick = "#B22222";
        public static readonly HexColor FireEngineRed = "#CE2029";
        public static readonly HexColor Flame = "#E25822";
        public static readonly HexColor FlamingoPink = "#FC8EAC";
        public static readonly HexColor Flattery = "#6B4423";
        public static readonly HexColor Flavescent = "#F7E98E";
        public static readonly HexColor Flax = "#EEDC82";
        public static readonly HexColor Flirt = "#A2006D";
        public static readonly HexColor FloralWhite = "#FFFAF0";
        public static readonly HexColor FluorescentOrange = "#FFBF00";
        public static readonly HexColor FluorescentPink = "#FF1493";
        public static readonly HexColor FluorescentYellow = "#CCFF00";
        public static readonly HexColor Folly = "#FF004F";
        public static readonly HexColor ForestGreenTraditional = "#014421";
        public static readonly HexColor ForestGreenWeb = "#228B22";
        public static readonly HexColor FrenchBeige = "#A67B5B";
        public static readonly HexColor FrenchBistre = "#856D4D";
        public static readonly HexColor FrenchBlue = "#0072BB";
        public static readonly HexColor FrenchFuchsia = "#FD3F92";
        public static readonly HexColor FrenchLilac = "#86608E";
        public static readonly HexColor FrenchLime = "#9EFD38";
        public static readonly HexColor FrenchMauve = "#D473D4";
        public static readonly HexColor FrenchPink = "#FD6C9E";
        public static readonly HexColor FrenchPlum = "#811453";
        public static readonly HexColor FrenchPuce = "#4E1609";
        public static readonly HexColor FrenchRaspberry = "#C72C48";
        public static readonly HexColor FrenchRose = "#F64A8A";
        public static readonly HexColor FrenchSkyBlue = "#77B5FE";
        public static readonly HexColor FrenchViolet = "#8806CE";
        public static readonly HexColor FrenchWine = "#AC1E44";
        public static readonly HexColor FreshAir = "#A6E7FF";
        public static readonly HexColor Frostbite = "#E936A7";
        public static readonly HexColor Fuchsia = "#FF00FF";
        public static readonly HexColor FuchsiaCrayola = "#C154C1";
        public static readonly HexColor FuchsiaPink = "#FF77FF";
        public static readonly HexColor FuchsiaPurple = "#CC397B";
        public static readonly HexColor FuchsiaRose = "#C74375";
        public static readonly HexColor Fulvous = "#E48400";
        public static readonly HexColor FuzzyWuzzy = "#CC6666";
    }

    #endregion

    public static class LogColor
    {
        public static readonly Color 深红 = HexColor.DeepRed;
        public static readonly Color 血红 = HexColor.AlizarinCrimson;
        public static readonly Color 玫瑰 = HexColor.DogwoodRose;
        public static readonly Color 浅红 = HexColor.CGRed;

        public static readonly Color 深绿 = HexColor.DeepGreen;
        public static readonly Color 浅绿 = HexColor.ForestGreenWeb;

        public static readonly Color 深蓝 = HexColor.EgyptianBlue;
        public static readonly Color 浅蓝 = HexColor.FrenchBlue;

        public static readonly Color 橘黄 = HexColor.AlloyOrange;
        public static readonly Color 浅褐 = HexColor.FuzzyWuzzy;

        public static readonly Color 开始 = HexColor.Denim;
        public static readonly Color 成功 = HexColor.DarkSpringGreen;

        public static readonly Color 进入 = HexColor.Denim;
        public static readonly Color 退出 = HexColor.DarkSpringGreen;
    }
}

