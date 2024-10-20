using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Cougar.Utils
{
    public static class SnakeCaser
    {
        private static readonly string SNAKE_CASE_REGEX = @"(?<!(^|[A-Z]))(?=[A-Z_])|(?<!^)(?=[A-Z][a-z])";

        public static string GetSnakeCase(string strFrom)
        {
            return string.Join("-", Regex.Split(strFrom, SNAKE_CASE_REGEX)
                .Where(word => !string.IsNullOrEmpty(word))
                .Where(word => word != "_")
                .Select(word => word.ToLower()));
        }

        public static string GetSnakeBellyCase(string strFrom)
        {
            return string.Join("_", Regex.Split(strFrom, SNAKE_CASE_REGEX)
                .Where(word => !string.IsNullOrEmpty(word))
                .Where(word => word != "_")
                .Select(word => word.ToLower()));
        }
    }
}