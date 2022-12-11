using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day11
{
    public static class Parsers
    {
        private static Regex operationRegex 
            = new Regex(@"Operation: new = old (?<op>\*|\+) (?<val>old|\d*)"
                        , RegexOptions.Compiled);


        public static Regex testRegex = new Regex(@"Test: divisible by (?<val>\d*)", RegexOptions.Compiled);

        public static Func<ulong, ulong> Operation(string line)
        {
            var opMatch = operationRegex.Match(line);
            var oldParameter = Expression.Parameter(typeof(ulong), "old");
            var useOld = opMatch.Groups["val"].Value == "old";
            var constExp = useOld ? Expression.Constant((ulong)1) : Expression.Constant(ulong.Parse(opMatch.Groups["val"].Value));

            Expression exp;
            if (useOld)
            {
                exp = opMatch.Groups["op"].Value switch
                {
                    "*" => Expression.MultiplyChecked(oldParameter, oldParameter),
                    "+" => Expression.AddChecked(oldParameter, oldParameter),
                };
            }
            else
            {
                exp = opMatch.Groups["op"].Value switch
                {
                    "*" => Expression.MultiplyChecked(oldParameter, constExp),
                    "+" => Expression.Add(oldParameter, constExp),
                };
            }

            return Expression.Lambda<Func<ulong, ulong>>(exp, oldParameter).Compile();
        }

        public static Func<ulong, bool> Test(string line)
        {
            var match = testRegex.Match(line);
            var oldParameter = Expression.Parameter(typeof(ulong), "w");
            var constExp = Expression.Constant(ulong.Parse(match.Groups["val"].Value));
            var testExp = Expression.Constant((ulong)0);
            Expression exp = Expression.Equal(Expression.Modulo(oldParameter, constExp), testExp);
            return Expression.Lambda<Func<ulong, bool>>(exp, oldParameter).Compile();
        }
    }
}
