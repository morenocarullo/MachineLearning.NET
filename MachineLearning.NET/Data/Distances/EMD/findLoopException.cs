using System;

namespace MachineLearning.NET.Data.Distances.EMD
{
    [Serializable]
    public class FindLoopException:System.Exception
    {
		
        public FindLoopException()
        {
            // TODO Auto-generated constructor stub
        }
		
        public FindLoopException(System.String message):base(message)
        {
            // TODO Auto-generated constructor stub
        }
		
        //UPGRADE_NOTE: Exception 'java.lang.Throwable' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
        public FindLoopException(System.Exception cause):base(cause.Message)
        {
            // TODO Auto-generated constructor stub
        }
		
        //UPGRADE_NOTE: Exception 'java.lang.Throwable' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
        public FindLoopException(System.String message, System.Exception cause):base(message, cause)
        {
            // TODO Auto-generated constructor stub
        }
    }
}