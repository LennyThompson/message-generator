using System;
using System.Text.Json;

namespace Cougar.Utils
{
    public class QuotedString
    {
        public static bool IsQuotedString(string strTest)
        {
            return strTest.Length >= 2 &&
                   (strTest[0] == '`' || strTest[0] == '\'' || strTest[0] == '\"') &&
                   strTest[0] == strTest[^1];
        }

        public static string UnquoteString(string strInput)
        {
            return IsQuotedString(strInput) ? strInput[1..^1].Trim() : strInput;
        }
    }
}