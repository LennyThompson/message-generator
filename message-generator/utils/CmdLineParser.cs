using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text.Json;

namespace Cougar.Utils
{
    /// <summary>
    /// Parses command lines in form of <Name>=<Value> that can then be referenced by name
    /// </summary>
    public class CmdLineParser
    {
        /// <summary>
        /// Helper class for individual arguments, a name-value pair
        /// </summary>
        public class CmdLineArg
        {
            private readonly string _argName;
            private readonly string _argValue;

            public CmdLineArg(string argName, string argValue)
            {
                _argName = argName;
                _argValue = argValue;
            }

            /// <summary>
            /// Get the argument name
            /// </summary>
            /// <returns>name</returns>
            public string Name() => _argName;

            /// <summary>
            /// Get the raw argument value
            /// </summary>
            /// <returns>value as string</returns>
            public string Value() => _argValue;

            /// <summary>
            /// Get the value as an integer
            /// </summary>
            /// <returns>the value parsed to an int or int.MinValue if the value is not an int</returns>
            public int AsIntValue()
            {
                if (int.TryParse(_argValue, out int result))
                {
                    return result;
                }
                return int.MinValue;
            }

            /// <summary>
            /// Removes quotes from a quoted string argument
            /// </summary>
            /// <returns>Returns the string value with quotes stripped</returns>
            public string AsQuotedString()
            {
                return QuotedString.UnquoteString(_argValue);
            }
        }

        private static readonly Regex ArgParser = new Regex(@"(.*)=(.*)");
        private readonly Dictionary<string, CmdLineArg> _args;

        /// <summary>
        /// Construct the parser from a list of command line arguments
        /// </summary>
        /// <param name="args">list of args from main</param>
        public CmdLineParser(string[] args)
        {
            _args = args
                .Where(arg => ArgParser.IsMatch(arg))
                .Select(arg =>
                {
                    var match = ArgParser.Match(arg);
                    if (match.Success)
                    {
                        return new CmdLineArg(match.Groups[1].Value, match.Groups[2].Value);
                    }
                    return new CmdLineArg("ERROR", arg);
                })
                .ToDictionary(arg => arg.Name(), arg => arg);
        }

        /// <summary>
        /// Return the CmdLineArg object for the argument name
        /// </summary>
        /// <param name="name">name of the command line argument</param>
        /// <returns>the command line argument or null if none found</returns>
        public CmdLineArg GetArg(string name)
        {
            if (_args.TryGetValue(name, out CmdLineArg arg))
            {
                return arg;
            }
            return null;
        }
    }
}

