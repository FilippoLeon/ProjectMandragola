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
}
