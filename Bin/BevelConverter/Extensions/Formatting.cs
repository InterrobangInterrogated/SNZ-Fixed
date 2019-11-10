﻿using System.Globalization;
using System.Linq;

// This global class defines extension methods to numeric types
// where I don't want system globalization to come into play.

public static class Format
{
    private const string decimalFmt = "0.00";
    private static CultureInfo invariant => CultureInfo.InvariantCulture;

    private static string filterNan(string value, string replace = decimalFmt)
    {
        if (value.ToLower() == "nan")
            value = replace;

        return value;
    }

    public static string ToInvariantString(this float value)
    {
        string result = value.ToString(decimalFmt, invariant);
        return filterNan(result);
    }

    public static string ToInvariantString(this double value)
    {
        string result = value.ToString(decimalFmt, invariant);
        return filterNan(result);
    }

    public static string ToInvariantString(this int value)
    {
        return value.ToString(invariant);
    }

    public static string ToInvariantString(this object value)
    {
        if (value is float)
        {
            float f = (float)value;
            return f.ToInvariantString();
        }
        else if (value is double)
        {
            double d = (double)value;
            return d.ToInvariantString();
        }
        else if (value is int)
        {
            int i = (int)value;
            return i.ToInvariantString();
        }
        else
        {
            // Unhandled
            return value.ToString();
        }
    }

    public static float ParseFloat(string s)
    {
        return float.Parse(s, invariant);
    }

    public static double ParseDouble(string s)
    {
        return double.Parse(s, invariant);
    }

    public static int ParseInt(string s)
    {
        return int.Parse(s, invariant);
    }

    public static string FormatFloats(params float[] values)
    {
        string[] results = values
            .Select(value => value.ToInvariantString())
            .ToArray();

        for (int i = 0; i < results.Length; i++)
        {
            string result = results[i];
            
            while (result.Contains(".") && (result.EndsWith("0") || result.EndsWith(".")))
                result = result.Substring(0, result.Length - 1);

            results[i] = result;
        }

        return '[' + string.Join("][", results) + ']';
    }
}