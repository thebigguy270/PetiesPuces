using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

public static class ExtensionMethods
{
    public static string ToStringEmptyNull(this string str) => str == "" ? null : str;

    // https://stackoverflow.com/questions/489258/linqs-distinct-on-a-particular-property
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>
        (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        HashSet<TKey> seenKeys = new HashSet<TKey>();
        foreach (TSource element in source)
        {
            if (seenKeys.Add(keySelector(element)))
            {
                yield return element;
            }
        }
    }

    public static decimal ParseDecimal(this string str)
    {
        decimal ret = 0;

        if (str.Contains(","))
            ret = decimal.Parse(str, CultureInfo.CreateSpecificCulture("fr-fr"));
        else
            ret = decimal.Parse(str, CultureInfo.CreateSpecificCulture("en-us"));

        return ret;
    }
}