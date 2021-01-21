using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

public static class DBCast
{
    // Returns true is the value is null in the DB
    public static bool IsDBNull(object value) => value is DBNull;

    // Boolean check and conversion
    public static bool? ToBit(object value) => IsDBNull(value) ? null : (bool?)value;

    // String check and conversion
    public static char? ToChar(object value) => IsDBNull(value) ? null : (char?)value;
    public static string ToString(object value) => IsDBNull(value) ? null : (string)value;

    // Number checks and conversions
    public static byte? ToTinyInt(object value) => IsDBNull(value) ? null : (byte?)value;
    public static short? ToSmallInt(object value) => IsDBNull(value) ? null : (short?)value;
    public static int? ToInt(object value) => IsDBNull(value) ? null : (int?)value;
    public static long? ToBigInt(object value) => IsDBNull(value) ? null : (long?)value;
    public static decimal? ToDecimal(object value) => IsDBNull(value) ? null : (decimal?)value;

    // DateTime conversion
    public static DateTime? ToDateTime(object value) => IsDBNull(value) ? null : (DateTime?)value;

    public static void SmartDBCast(this PropertyInfo prop, object obj, object val)
    {
        if (val.GetType() == typeof(byte))
            prop.SetValue(obj, ToTinyInt(val));
        else if (val.GetType() == typeof(short))
            prop.SetValue(obj, ToSmallInt(val));
        else if (val.GetType() == typeof(long))
            prop.SetValue(obj, ToBigInt(val));
        else if (val.GetType() == typeof(int))
            prop.SetValue(obj, ToInt(val));
        else if (val.GetType() == typeof(decimal))
            prop.SetValue(obj, ToDecimal(val));
        else if (val.GetType() == typeof(char))
            prop.SetValue(obj, ToChar(val));
        else if (val.GetType() == typeof(string))
            prop.SetValue(obj, ToString(val));
        else if (val.GetType() == typeof(DateTime))
            prop.SetValue(obj, ToDateTime(val));
        else if (val.GetType() == typeof(bool))
            prop.SetValue(obj, ToBit(val));
    }
}