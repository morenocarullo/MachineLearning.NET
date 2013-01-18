namespace MachineLearning.NET.Data.Distances.EMD
{
    public class Node1Content
    {
        private bool endNode = false;
        private int i;
        private Node1Content Next;
        private int pos;
        private double val;

        public Node1Content()
        {
            // TODO Auto-generated constructor stub
        }

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
        virtual public bool LastNode
        {
            set
            {
                this.endNode = value;
            }
			
        }
        virtual public bool IsLast
        {
            get
            {
                return endNode;
            }
			
        }

        public virtual void  setNode(int i, double v)
        {
            this.i = i;
            this.val = v;
        }


        public virtual void  setNext(Node1Content next)
        {
            this.Next = next;
        }
		
        public virtual Node1Content getNext()
        {
            return Next;
        }
    }
}