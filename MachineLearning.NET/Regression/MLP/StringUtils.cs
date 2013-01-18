using System.Collections;

namespace MachineLearning.NET.Regression.MLP
{
    public class StringUtils
    {
        public static string[] EliminaStrVuote(string[] s)
        {
            ArrayList ar = new ArrayList(s);
            for (int i = 0; i < ar.Count; i++)
            {
                ar[i] = ((string)ar[i]).Trim();
                if (((string)ar[i]).Equals(""))
                {
                    ar.Remove(ar[i]);
                    i = -1;
                }
            }
            return (string[])ar.ToArray(typeof(string));
        }
    }
}