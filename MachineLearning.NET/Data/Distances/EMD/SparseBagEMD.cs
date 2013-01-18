using MachineLearning.NET.Data.Patterns;

namespace MachineLearning.NET.Data.Distances.EMD
{
    [Distance(Name = "emd")]
    public class SparseBagEMD : IDistance<SparseBag>
    {
        public double Distance(SparseBag p1, SparseBag p2)
        {
            // Signature1
            var signatureP1weights = new double[p1.Istances.Length];
            for (var i = 0; i < signatureP1weights.Length; i++)
            {
                signatureP1weights[i] = 1.0f / signatureP1weights.Length;
            }
            var signatureP1 = new Signature(signatureP1weights);

            // Signature2
            var signatureP2weights = new double[p2.Istances.Length];
            for (var i = 0; i < signatureP2weights.Length; i++)
            {
                signatureP2weights[i] = 1.0f / signatureP2weights.Length;
            }
            var signatureP2 = new Signature(signatureP2weights);

            // Compute the cost matrix
            var costMatrix = new double[signatureP1.N][];
            for(var costMatrixRowId=0; costMatrixRowId<costMatrix.Length; costMatrixRowId++)
            {
                var costMatrixRow = new double[signatureP2.N];
                costMatrix[costMatrixRowId] = costMatrixRow;
                for(var costMatrixColId=0; costMatrixColId<signatureP2.N; costMatrixColId++)
                {
                    var bag1Instance = p1.Istances[costMatrixRowId];
                    var bag2Instance = p2.Istances[costMatrixColId];
                    costMatrixRow[costMatrixColId] = Euclidean.Distance(bag1Instance,bag2Instance);
                }
            }

            // Compute EMD
            var emd = new EMD();
            return emd.Emd(signatureP1, signatureP2, costMatrix, null, 0, null);
        }
    }
}
