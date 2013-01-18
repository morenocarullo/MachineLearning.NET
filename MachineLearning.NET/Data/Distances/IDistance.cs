namespace MachineLearning.NET.Data.Distances
{
    public interface IDistance<P>
    {
        double Distance(P p1, P p2);
    }
}