using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringUtilities {
    static private long thousand = 1000;
    static private long million = 1000000;
    static private long billion = 1000000000;
    static private long trillion = 1000000000;

    public static string longToShortString(long i, int precision = 0)
    {
        if (i < thousand) return i.ToString();
        if (thousand <= i && i < million) return (i / thousand).ToString() + ("." + (i % thousand).ToString()).Substring(0, precision+1) + "K";
        if (million <= i && i < billion) return  (i / million).ToString() + ("." + (i % million).ToString()).Substring(0, precision+1) + "M";
        if (billion <= i && i < trillion) return (i / billion).ToString() + ("." + (i % billion).ToString()).Substring(0, precision+1) + "G";
        return (i / trillion).ToString() + ("." + (i % trillion).ToString()).Substring(0, precision+1) + "T";
    }

    public static string floatToPercentage(float f)
    {
        return (f * 100).ToString("####0.00") + "%";
    }

    public static string toCurrency(long i)
    {
        return longToShortString(i, 1) + " $";
    }

    // http://answers.unity3d.com/questions/812240/convert-hex-int-to-colorcolor32.html
    // Note that Color32 and Color implictly convert to each other. You may pass a Color object to this method without first casting it.
    public static string colorToHex(Color32 color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }

    public static Color hexToColor(string hex)
    {
        hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
        byte a = 255;//assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }
}
