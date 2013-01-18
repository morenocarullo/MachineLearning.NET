namespace MachineLearning.NET.RuleSystem
{
    /// <summary>
    /// This class defines a rule on a given property.
    /// </summary>
    /// <creator>Moreno Carullo</creator>
    /// <date>2009/06/29</date>
    public abstract class Rule
    {
        protected object CompareWithValue
        {
            get; set;
        }

        public object Property
        {
            get; protected set;
        }

        public abstract bool VerifiedBy(object o);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            var objAs = obj as Rule;
            if (objAs == null) return false;
            if (objAs.GetType() != GetType()) return false;

            return Equals(objAs.CompareWithValue, CompareWithValue) && Equals(objAs.Property, Property);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (CompareWithValue.GetHashCode() * 397) ^ Property.GetHashCode();
            }
        }
    }
}