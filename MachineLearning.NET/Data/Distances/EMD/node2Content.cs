namespace MachineLearning.NET.Data.Distances.EMD
{
    public class Node2Content
    {
        private bool endNode = false;

        /// <summary> </summary>
        private int i;
		
        /// <summary> </summary>
        private int j;

        /// <summary> </summary>
        internal Node2Content nC;
		
        /// <summary> </summary>
        internal Node2Content nR;

        /// <summary> </summary>
        private int pos;

        /// <summary> </summary>
        internal double val;

        /// <summary> </summary>
        public Node2Content()
        {
            // TODO Auto-generated constructor stub
        }

        virtual public bool IsLast
        {
            get
            {
                return endNode;
            }
			
        }
        virtual public bool LastNode
        {
            set
            {
                this.endNode = value;
            }
			
        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <returns>
        /// </returns>
        /// <param name="pos">
        /// </param>
        virtual public int Pos
        {
            get
            {
                // TODO Auto-generated method stub
                return pos;
            }
			
            set
            {
                this.pos = value;
            }
			
        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <returns>
        /// </returns>
        /// <param name="i">
        /// </param>
        virtual public int I
        {
            get
            {
                return i;
            }
			
            set
            {
                this.i = value;
            }
			
        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <returns>
        /// </returns>
        /// <param name="j">
        /// </param>
        virtual public int J
        {
            get
            {
                return j;
            }
			
            set
            {
                this.j = value;
            }
			
        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <returns>
        /// </returns>
        /// <param name="val">
        /// </param>
        virtual public double Val
        {
            get
            {
                return val;
            }
			
            set
            {
                this.val = value;
            }
			
        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <returns>
        /// </returns>
        /// <param name="nc">
        /// </param>
        virtual public Node2Content NC
        {
            get
            {
                return nC;
            }
			
            set
            {
                nC = value;
            }
			
        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <returns>
        /// </returns>
        /// <param name="nr">
        /// </param>
        virtual public Node2Content NR
        {
            get
            {
                return nR;
            }
			
            set
            {
                nR = value;
            }
			
        }
    }
}