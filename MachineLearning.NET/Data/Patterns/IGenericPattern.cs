namespace MachineLearning.NET.Data.Patterns
{
    public interface IGenericPattern
    {
        /// <value>
        /// The class this pattern belongs to. Can be -1 if
        /// none is defined.
        /// </value>
        int Label { get; }
    }
}