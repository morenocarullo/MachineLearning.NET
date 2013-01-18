using System.Collections.Generic;

namespace MachineLearning.NET.Utils
{
    /// <summary>
    /// This extender permits to get a dictionary of parameters out of a configuration string.
    /// </summary>
    /// <creator>Moreno Carullo</creator>
    internal static class ConfigurationStringExtensions
    {
        public static IDictionary<string, string> GetParamsFromConfigString(this string sConfigString)
        {
            IDictionary<string, string> iParams = new Dictionary<string, string>();

            if (sConfigString.IndexOf(':') == -1)
            {
                return iParams;
            }

            var sParamsString = sConfigString.Substring(sConfigString.IndexOf(':') + 1);
            var sParams = sParamsString.Split(',');

            foreach (var sParam in sParams)
            {
                if (sParam.IndexOf('=') != -1)
                {
                    var sParamAndValue = sParam.Split('=');
                    var sParamName = sParamAndValue[0].Trim();
                    var sParamValue = sParamAndValue[1].Trim();
                    iParams.Add(sParamName, sParamValue);
                }
            }

            return iParams;
        }
    }
}