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
            = new Regex(@"Operation: new = old (?<op>\*|\+|-|/) (?<val>old|\d*)"
                        , RegexOptions.Compiled);


        private static Regex testRegex = new Regex(@"Test: divisible by (?<val>\d*)", RegexOptions.Compiled);

        public static Func<long, long> Operation(string line)
        {
            var opMatch = operationRegex.Match(line);
            var oldParameter = Expression.Parameter(typeof(long), "old");
            var useOld = opMatch.Groups["val"].Value == "old";
            var constExp = useOld ? Expression.Constant((long)1) : Expression.Constant(long.Parse(opMatch.Groups["val"].Value));

            Expression exp;
            if (useOld)
            {
                exp = opMatch.Groups["op"].Value switch
                {
                    "*" => Expression.Multiply(oldParameter, oldParameter),
                    "-" => Expression.Subtract(oldParameter, oldParameter),
                    "+" => Expression.Add(oldParameter, oldParameter),
                    "/" => Expression.Divide(oldParameter, oldParameter)
                };
            }
            else
            {
                exp = opMatch.Groups["op"].Value switch
                {
                    "*" => Expression.Multiply(oldParameter, constExp),
                    "-" => Expression.Subtract(oldParameter, constExp),
                    "+" => Expression.Add(oldParameter, constExp),
                    "/" => Expression.Divide(oldParameter, constExp)
                };
            }

            return Expression.Lambda<Func<long, long>>(exp, oldParameter).Compile();
        }

        public static Func<long, bool> Test(string line)
        {
            var match = testRegex.Match(line);
            var oldParameter = Expression.Parameter(typeof(long), "w");
            var constExp = Expression.Constant(long.Parse(match.Groups["val"].Value));
            var testExp = Expression.Constant((long)0);
            Expression exp = Expression.Equal(Expression.Modulo(oldParameter, constExp), testExp);
            return Expression.Lambda<Func<long, bool>>(exp, oldParameter).Compile();
        }
    }
}
