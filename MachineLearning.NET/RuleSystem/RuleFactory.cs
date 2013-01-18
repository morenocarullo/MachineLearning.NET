using System;
using System.Globalization;

namespace MachineLearning.NET.RuleSystem
{
    static class RuleFactory
    {
        /// <summary>
        /// Preserve culture-invariant formatting of numbers when serializing.
        /// </summary>
        private static readonly NumberFormatInfo s_numberFormat = CultureInfo.InvariantCulture.NumberFormat;

        /// <summary>
        /// Factory method that load a rule from a string.
        /// </summary>
        /// <param name="rule">the rule in a string form</param>
        /// <returns>the instance of a rule</returns>
        public static Rule LoadFromString(string rule)
        {
            var operatorChars = new[] {GreaterThan.OperatorSymbol, LessThan.OperatorSymbol, EqualTo.OperatorSymbol};
            var ruleParts = rule.Split(operatorChars);

            var thisOperator = rule.Substring(ruleParts[0].Length,1)[0];

            switch (thisOperator)
            {
                case GreaterThan.OperatorSymbol:
                    return new GreaterThan(ruleParts[0], Single.Parse(ruleParts[1], s_numberFormat));
                case LessThan.OperatorSymbol:
                    return new LessThan(ruleParts[0], Single.Parse(ruleParts[1], s_numberFormat));
                case EqualTo.OperatorSymbol:
                    return new EqualTo(ruleParts[0], Single.Parse(ruleParts[1], s_numberFormat));
                default:
                    throw new Exception(String.Format("Unknown operator in rules: {0}", thisOperator));
            }
        }
    }
}