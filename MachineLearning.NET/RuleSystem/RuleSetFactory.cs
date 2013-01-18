using System;
using System.Linq;

namespace MachineLearning.NET.RuleSystem
{
    public static class RuleSetFactory
    {
        public static RuleSet BuildRule(string ruleExpression)
        {
            return new RuleSet(
                from rule
                    in string.Format("{0}", ruleExpression).Split(new[] { '^', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                select RuleFactory.LoadFromString(rule));
        }
    }
}