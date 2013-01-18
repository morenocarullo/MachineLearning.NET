namespace MachineLearning.NET.Data.Distances.EMD
{
    public class Features
    {
        internal int[] f;
		
        public Features(int n)
        {
            f = new int[n];
        }
        public virtual void  setFeatureAtPosition(int position, int val)
        {
            f[position] = val;
        }
        public virtual int getValAtPosition(int position)
        {
            return this.f[position];
        }
    }
}