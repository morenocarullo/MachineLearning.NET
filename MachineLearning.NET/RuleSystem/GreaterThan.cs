using System;

namespace MachineLearning.NET.RuleSystem
{
    /// <summary>
    /// This class defines a the "greater than" rule on a given property.
    /// </summary>
    /// <creator>Moreno Carullo</creator>
    /// <date>2009/06/29</date>
    public class GreaterThan : Rule
    {
        public const char OperatorSymbol = '>';

        public GreaterThan(object propertyName, object o)
        {
            Property = propertyName;
            CompareWithValue = o;
        }

        public override bool VerifiedBy(object o)
        {
            var compareWhat = o as IComparable;
            var compareWith = CompareWithValue as IComparable;
            return compareWhat.CompareTo(compareWith) > 0;
        }
    }
}