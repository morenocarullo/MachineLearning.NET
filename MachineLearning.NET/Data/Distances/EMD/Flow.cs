namespace MachineLearning.NET.Data.Distances.EMD
{
    public class Flow
    {
        /// <summary> </summary>
        internal double amount;

        /// <summary> </summary>
        internal bool endNode = false;

        /// <summary> </summary>
        internal int from;

        /// <summary> </summary>
        internal int pos;

        /// <summary> </summary>
        internal int to;

        public Flow()
        {
            // TODO Auto-generated constructor stub
        }

        virtual public int Pos
        {
            get
            {
                return pos;
            }
			
            set
            {
                this.pos = value;
            }
			
        }
        virtual public bool EndNode
        {
            get
            {
                return endNode;
            }
			
            set
            {
                this.endNode = value;
            }
			
        }
        virtual public int From
        {
            get
            {
                return from;
            }
			
            set
            {
                this.from = value;
            }
			
        }
        virtual public int To
        {
            get
            {
                return to;
            }
			
            set
            {
                this.to = value;
            }
			
        }
        virtual public double Amount
        {
            get
            {
                return amount;
            }
			
            set
            {
                this.amount = value;
            }
			
        }
    }
}