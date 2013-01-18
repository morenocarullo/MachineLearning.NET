namespace MachineLearning.NET.RuleSystem
{
    /// <summary>
    /// This class defines a the "equal to" rule on a given property.
    /// </summary>
    /// <creator>Moreno Carullo</creator>
    /// <date>2009/07/08</date>
    public class EqualTo : Rule
    {
        public const char OperatorSymbol = '=';

        public EqualTo(object propertyName, object o)
        {
            Property = propertyName;
            CompareWithValue = o;
        }

        public override bool VerifiedBy(object o)
        {
            return CompareWithValue.Equals(o);
        }
    }
}