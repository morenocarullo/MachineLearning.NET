using MachineLearning.NET.Mapack;

namespace MachineLearning.NET.Optimization
{
    public interface ILMSOptimizer   
    {
        /// <summary>
        /// Find w such that w*x = y
        /// </summary>
        /// <param name="x">the x vector or matrix</param>
        /// <param name="y">the y vector or matrix</param>
        /// <returns></returns>
        Matrix Optimize(Matrix x, Matrix y);
    }
}