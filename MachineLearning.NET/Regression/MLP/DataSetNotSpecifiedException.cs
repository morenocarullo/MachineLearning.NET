using System;

namespace MachineLearning.NET.Regression.MLP
{
    /// <author>  Ignazio
    /// 
    /// </author>
    [Serializable]
    public class DataSetNotSpecifiedException : Exception
    {
        private readonly String detail;

        public DataSetNotSpecifiedException(String msg)
            : base(msg)
        {
            detail = msg;
        }

        public virtual String Description()
        {
            return detail;
        }
    }
}