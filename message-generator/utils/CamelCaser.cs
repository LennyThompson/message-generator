using System;
using System.Linq;

namespace Cougar.Utils
{
    public static class CamelCaser
    {
        public static string ToCamelCase(string strFrom)
        {
            return string.Join("", strFrom.Split('_')
                .Where(word => !string.IsNullOrEmpty(word))
                .Where(word => word != "_")
                .Select(word =>
                {
                    string strReturn = word.ToLower();
                    return char.ToUpper(strReturn[0]) + strReturn.Substring(1);
                }));
        }
    }
}