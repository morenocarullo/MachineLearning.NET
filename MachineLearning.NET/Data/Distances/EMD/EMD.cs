using System.Collections.Generic;
using System;

namespace MachineLearning.NET.Data.Distances.EMD
{
    public class EMD
    {
        private const double EPSILON = 1e-6;
		
        private const double INFINITY = 1e20;


        private const int MAX_ITERATIONS = 500;
        private const int MAX_SIG_SIZE = 500;
        private const int MAX_SIG_SIZE1 = 501;
        private static Node2Content[] colsX;

        private static float[][] costMatrix;

        private static Node2Content[] rowsX;
        private static Node2Content[] X;
        internal double[] d;
        private Node2Content endx, enterX;
        private int[][] isX;


        private float maxC;
        private double maxW;
        private int n1, n2;
        internal double[] s;

        public EMD()
        {
			
            X = new Node2Content[MAX_SIG_SIZE];
            for (var i = 0; i < X.Length; i++)
            {
                X[i] = new Node2Content();
                X[i].Pos = i;
            }
			
            rowsX = new Node2Content[MAX_SIG_SIZE];
            for (var i = 0; i < rowsX.Length; i++)
            {
                rowsX[i] = new Node2Content();
                rowsX[i].Pos = i;
                rowsX[i].LastNode = true;
            }
			
            colsX = new Node2Content[MAX_SIG_SIZE];
            for (var i = 0; i < colsX.Length; i++)
            {
                colsX[i] = new Node2Content();
                colsX[i].Pos = i;
                colsX[i].LastNode = true;
            }
        }


        /// <param name="s1">
        /// </param>
        /// <param name="s2">
        /// </param>
        /// <returns>
        /// </returns>
        /// <throws>  SignatureSizeException </throws>
        /// <throws>  FindBasicVariablesException </throws>
        /// <throws>  IsOptimalException  </throws>
        /// <throws>  FindLoopException  </throws>
        public virtual float Emd(Signature signature1, Signature signature2, 
                                 double[][] costMatrix, Flow flow, int flowSize, List<Flow> flowList)
        {
            #region Pre-Execution checks

            if (signature1.N != costMatrix.Length || signature2.N != costMatrix[0].Length)
            {
                throw new ArgumentException("The costMatrix is not compatible with the two signature!");
            }
            #endregion

            int itr;
            double totalCost;
            double w;
            Node2Content xp;
            Flow[] flowP;
            Flow flowp;
            Node1Content[] U, V;
            
            //  var flowList= new List<flow_t>();

			
            U = new Node1Content[MAX_SIG_SIZE1];
            for (var i = 0; i < U.Length; i++)
            {
                U[i] = new Node1Content();
                U[i].Pos = i;
            }
            V = new Node1Content[MAX_SIG_SIZE1];
            for (var i = 0; i < V.Length; i++)
            {
                V[i] = new Node1Content();
                V[i].Pos = i;
            }
			
            flowP = new Flow[MAX_SIG_SIZE1];
            for (var i = 0; i < flowP.Length; i++)
            {
                flowP[i] = new Flow();
                flowP[i].Pos = i;
            }
			
			
            w = init(signature1, signature2, costMatrix);
			
            if (n1 > 1 && n2 > 1)
                /* IF _n1 = 1 OR _n2 = 1 THEN WE ARE DONE */
            {
                for (itr = 1; itr < MAX_ITERATIONS; itr++)
                {
                    //				System.out.println("EMD");
                    /* FIND BASIC VARIABLES */
                    findBasicVariables(U, V);
					
                    //			  /* CHECK FOR OPTIMALITY */
                    if (isOptimal(U, V))
                        break;
                    //			  
                    //			  /* IMPROVE SOLUTION */
                    newSol();
                }
            }
            totalCost = 0;
            if (flow != null)
            {
                flowP[0] = flow;
            }
            flowp = flowP[0];
            for (xp = X[0]; xp.Pos < endx.Pos; xp = X[xp.Pos + 1])
            {
                if (xp.Equals(enterX))
                {
                    continue;
                }
                if (xp.I == signature1.N || xp.J == signature2.N)
                {
                    continue;
                }
                if (xp.Val == 0)
                {
                    continue;
                }
				
                totalCost = totalCost + xp.Val * costMatrix[xp.I][xp.J];
                if (flow != null)
                {

                    // my code
                    var flowTemp = new Flow();
                    flowTemp.From = xp.I;
                    flowTemp.To = xp.J;
                    flowTemp.Amount = xp.Val;
                    flowTemp = flowP[flowp.Pos + 1];
                    flowList.Add(flowTemp);
                    // end of my code


                    flowp.From = xp.I;
                    flowp.To = xp.J;
                    flowp.Amount = xp.Val;
                    flowp = flowP[flowp.Pos + 1];
                }
            }
			
            //		if(flow != null){
            //			flowSize=flowp.getPos() - flow.getPos();
            //		}
			
			
			
            //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
            return (float) (totalCost / w);
        }
		
		
		
		
        /// <throws>  FindLoopException  </throws>
        /// <summary> 
        /// </summary>
        private void  newSol()
        {
			
            int i, j, k;
            double xMin;
            int steps;
            Node2Content[] Loop;
            Node2Content curX, leaveX = null;
			
			
            i = enterX.I;
            j = enterX.J;
            isX[i][j] = 1;
            enterX.NC = rowsX[i];
            enterX.NR = colsX[j];
            enterX.Val = 0;
            rowsX[i] = enterX;
            colsX[j] = enterX;
			
			
            Loop = new Node2Content[MAX_SIG_SIZE1];
            for (i = 0; i < n1 + n2; i++)
            {
				
                Loop[i] = new Node2Content();
                Loop[i].Pos = i;
            }
			
            steps = findLoop(Loop);
			
            xMin = INFINITY;
            for (k = 1; k < steps; k = k + 2)
            {
                if (Loop[k].Val < xMin)
                {
                    leaveX = Loop[k];
                    xMin = Loop[k].Val;
                }
            }
			
            for (k = 0; k < steps; k = k + 2)
            {
                Loop[k].Val = Loop[k].Val + xMin;
                Loop[k + 1].Val = Loop[k + 1].Val - xMin;
            }
			
			
            i = leaveX.I;
            j = leaveX.J;
            isX[i][j] = 0;
            if (rowsX[i].Equals(leaveX))
            {
                rowsX[i] = leaveX.NC;
            }
            else
            {
                for (curX = rowsX[i]; curX.IsLast != true; curX = curX.NC)
                {
                    if (curX.NC.Equals(leaveX))
                    {
                        curX.NC = curX.NC.NC;
                        break;
                    }
                }
            }
            if (colsX[j].Equals(leaveX))
            {
                colsX[j] = leaveX.NR;
            }
            else
            {
                for (curX = colsX[j]; curX.IsLast != true; curX = curX.NR)
                {
                    if (curX.NR.Equals(leaveX))
                    {
                        curX.NR = curX.NR.NR;
                        break;
                    }
                }
            }
            enterX = leaveX;
        }
		
		
        private void  printArray(Node2Content[] loop, int steps)
        {
			
            System.Console.Out.Write("Loop= {");
            for (var i = 0; i < steps; i++)
            {
                System.Console.Out.Write("( ");
                System.Console.Out.Write(loop[i].I);
                System.Console.Out.Write(" ");
                System.Console.Out.Write(loop[i].J);
                System.Console.Out.Write(" ");
                System.Console.Out.Write(loop[i].Val);
                System.Console.Out.Write(" )");
            }
            System.Console.Out.WriteLine("}");
        }
		
		
        /// <param name="loop">
        /// </param>
        /// <returns>
        /// </returns>
        /// <throws>  FindLoopException  </throws>
        private int findLoop(Node2Content[] loop)
        {
			
            int i, steps;
            Node2Content newX;
            Node2Content curX;
			
            int[] isUsed;
            var pos = 0;
			
            isUsed = new int[2 * MAX_SIG_SIZE1];
			
            for (i = 0; i < n1 + n2; i++)
            {
                isUsed[i] = 0;
            }
			
			
			
            curX = loop[0] = enterX;
            //		curX=enterX;
            newX = curX;
			
            isUsed[enterX.Pos] = 1;
            steps = 1;
			
            do 
            {
                //			printArray(loop,steps);
                if (steps % 2 == 1)
                {
                    newX = rowsX[newX.I];
                    //				newX.setPos(pos);
                    while ((newX.IsLast != true) && (isUsed[newX.Pos] != 0))
                    {
                        newX = newX.NC;
                    }
                }
                else
                {
                    newX = colsX[newX.J];
                    //				newX.setPos(pos);
                    while ((newX.IsLast != true) && (isUsed[newX.Pos] != 0) && (!(newX.Equals(enterX))))
                    {
                        newX = newX.NR;
                    }
                    if (newX.Equals(enterX))
                    {
                        break;
                    }
                }
				
                if (!newX.IsLast)
                {
                    loop[pos + 1] = newX;
                    curX = loop[pos + 1];
                    pos++;
                    isUsed[newX.Pos] = 1;
                    steps = steps + 1;
                }
                else
                {
					
                    do 
                    {
                        newX = curX;
                        do 
                        {
                            if (steps % 2 == 1)
                            {
                                newX = newX.NR;
                            }
                            else
                            {
                                newX = newX.NC;
                            }
                        }
                        while ((newX.IsLast != true) && (isUsed[newX.Pos] != 0));
						
                        if (newX.IsLast == true)
                        {
                            isUsed[curX.Pos] = 0;
                            curX = loop[pos - 1];
                            pos--;
							
                            steps = steps - 1;
                        }
                    }
                    while ((newX.IsLast == true) && (pos >= 0));
					
                    isUsed[curX.Pos] = 0;
                    curX = loop[pos] = newX;
                    isUsed[newX.Pos] = 1;
                }
            }
            while (pos >= 0);
			
            if (pos == 0)
            {
                throw new FindLoopException();
            }
			
            return steps;
        }
		
        /// <param name="u">
        /// </param>
        /// <param name="v">
        /// </param>
        /// <returns>
        /// </returns>
        /// <throws>  IsOptimalException </throws>
        private bool isOptimal(Node1Content[] u, Node1Content[] v)
        {
			
            double delta, deltaMin;
            int i, j, minI = 0, minJ = 0;
			
            deltaMin = INFINITY;
            for (i = 0; i < n1; i++)
            {
                for (j = 0; j < n2; j++)
                {
                    if (isX[i][j] == 0)
                    {
                        delta = costMatrix[i][j] - u[i].Val - v[j].Val;
                        if (deltaMin > delta)
                        {
                            deltaMin = delta;
                            minI = i;
                            minJ = j;
                        }
                    }
                }
            }
			
            if (deltaMin == INFINITY)
            {
                throw new IsOptimalException();
            }
			
            enterX.I = minI;
            enterX.J = minJ;
			
            return (deltaMin >= (- EPSILON) * maxC);
        }
		
        /// <param name="U">
        /// </param>
        /// <param name="V">
        /// </param>
        /// <throws>  FindBasicVariablesException </throws>
        private void  findBasicVariables(Node1Content[] U, Node1Content[] V)
        {
			
            int i, j, found;
            int uFoundNum, vFoundNum;
            Node1Content nodenull, prevU, prevV, curU, curV, u1head, v1head = new Node1Content(), u0head = new Node1Content(), v0head = new Node1Content();
			
			
            //
            //		System.out.println("###############");
            //		for (int st1=0;st1<n1;st1++){
            //			for (int st2=0;st2<n2;st2++){
            //		      System.out.print(isX[st1][st2]);
            //		      System.out.print("     ");}
            //		System.out.println();}
			
			
			
            u0head.setNext(U[0]);
            curU = U[0];
            for (i = 0; i < n1; i++)
            {
                curU.I = i;
                curU.setNext(U[curU.Pos + 1]);
                curU = U[curU.Pos + 1];
            }
            curU.LastNode = true;
			
            nodenull = new Node1Content();
            nodenull.LastNode = true;
            u1head = new Node1Content();
            u1head.setNext(nodenull);
			
			
            curV = V[1];
			
            v0head.setNext(n2 > 1?V[1]:nodenull);
            for (j = 1; j < n2; j++)
            {
                curV.I = j;
                curV.setNext(V[curV.Pos + 1]);
                curV = V[curV.Pos + 1];
            }
            curV.LastNode = true;
			
            v1head.setNext(nodenull);
			
            V[0].I = 0;
            V[0].Val = 0;
            v1head.setNext(V[0]);
            v1head.getNext().setNext(nodenull);
			
			
			
            uFoundNum = 0;
            vFoundNum = 0;
            while (uFoundNum < n1 || vFoundNum < n2)
            {
				
				
				
                found = 0;
                if (vFoundNum < n2)
                {
                    prevV = v1head;
                    for (curV = v1head.getNext(); curV.IsLast != true; curV = curV.getNext())
                    {
						
                        j = curV.I;
                        prevU = u0head;
						
                        for (curU = u0head.getNext(); curU.IsLast != true; curU = curU.getNext())
                        {
                            i = curU.I;
                            if (isX[i][j] == 1)
                            {
								
                                curU.Val = costMatrix[i][j] - curV.Val;
                                prevU.setNext(curU.getNext());
                                curU.setNext(u1head.getNext().IsLast != true?u1head.getNext():nodenull);
                                u1head.setNext(curU);
                                curU = prevU;
                            }
                            else
                            {
								
                                prevU = curU;
                            }
                        }
                        prevV.setNext(curV.getNext());
                        vFoundNum = vFoundNum + 1;
                        found = 1;
                    }
                }
                if (uFoundNum < n1)
                {
					
                    prevU = u1head;
                    for (curU = u1head.getNext(); curU.IsLast != true; curU = curU.getNext())
                    {
						
                        i = curU.I;
                        prevV = v0head;
                        for (curV = v0head.getNext(); curV.IsLast != true; curV = curV.getNext())
                        {
                            j = curV.I;
                            if (isX[i][j] == 1)
                            {
                                curV.Val = costMatrix[i][j] - curU.Val;
                                prevV.setNext(curV.getNext());
                                curV.setNext(v1head.getNext().IsLast != true?v1head.getNext():nodenull);
                                v1head.setNext(curV);
                                curV = prevV;
                            }
                            else
                            {
                                prevV = curV;
                            }
                        }
                        prevU.setNext(curU.getNext());
                        uFoundNum = uFoundNum + 1;
                        found = 1;
                    }
                }
                if (found == 0)
                {
                    throw new FindBasicVariablesException();
                }
            }
        }
		
        /// <param name="s1">
        /// </param>
        /// <param name="s2">
        /// </param>
        /// <returns>
        /// </returns>
        /// <throws>  SignatureSizeException  </throws>
        private double init(Signature s1, Signature s2, double[][] Dist)
        {
			
            int i, j;
            double sSum, dSum, diff;
			
			
            //n1 e n2 prendono la dimensione delle signature ( numero di cluster)
            n1 = s1.N;
            n2 = s2.N;
            isX = new int[MAX_SIG_SIZE1][];
            for (var i2 = 0; i2 < MAX_SIG_SIZE1; i2++)
            {
                isX[i2] = new int[MAX_SIG_SIZE1];
            }
            //creo una matrice n1xn2 per contenere i costi
            costMatrix = new float[MAX_SIG_SIZE1][];
            for (var i3 = 0; i3 < MAX_SIG_SIZE1; i3++)
            {
                costMatrix[i3] = new float[MAX_SIG_SIZE1];
            }
            //creo due array 
            s = new double[MAX_SIG_SIZE1];
            d = new double[MAX_SIG_SIZE1];
            //se il numero dei cluster delle signature  maggiore di MAX_SIG_SIZE1 
            //lancio un'eccezione
            if ((n1 > MAX_SIG_SIZE1) || (n2 > MAX_SIG_SIZE1))
            {
                throw new SignatureSizeException();
            }
            maxC = 0.0f;
			
            //per ogni i (da 0 a n1-1) ovvero per il numero di cluster della prima signature
            for (i = 0; i < n1; i++)
            {
                //per ogni j (da 0 a n2-1) ovvero per il numero di cluster della seconda signature
                for (j = 0; j < n2; j++)
                {
                    //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                    costMatrix[i][j] = (float) Dist[i][j];
                    //assegno a maxC il valore piu' grande della costMatrix 
                    if (costMatrix[i][j] > maxC)
                    {
                        maxC = costMatrix[i][j];
                    }
                }
            }
			
			
            var nodenull = new Node2Content();
            nodenull.LastNode = true;
			
			
            //Inizializzo a 0.0f sSum 
            sSum = 0.0f;
            //per ogni cluster della signature 1 assegno ad s[i] il peso associato al cluster i-esimo
            // e sSum prende la somma di tutti i pesi;
            for (i = 0; i < n1; i++)
            {
                s[i] = s1.getWeight(i);
                sSum = sSum + s[i];
                //			rowsX[i] = null;
            }
			
            //Inizializzo a 0.0f dSum 
            dSum = 0.0f;
            //per ogni cluster della signature 2 assegno ad d[j] il peso associato al cluster j-esimo
            // e dSum prende la somma di tutti i pesi;
            for (j = 0; j < n2; j++)
            {
                d[j] = s2.getWeight(j);
                dSum = dSum + d[j];
                //			colsX[j]=null;
            }
			
            // la variabile diff prende la differenza delle somme dei pesi (tot) della signature1 e signature2
            diff = sSum - dSum;
			
            if ((System.Math.Abs(diff)) >= (EPSILON * sSum))
            {
                if (diff < 0.0f)
                {
                    //per ogni cluster della signature2 costMatrix n1-j prende 0
                    for (j = 0; j < n2; j++)
                    {
                        costMatrix[n1][j] = 0;
                    }
                    s[n1] = - diff;
                    rowsX[n1] = nodenull;
                    n1++;
                }
                else
                {
                    //per ogni cluster della signature1 costMatrix n2-j prende 0
                    for (i = 0; i < n1; i++)
                    {
                        costMatrix[i][n2] = 0;
                    }
                    d[n2] = diff;
                    colsX[n2] = nodenull;
                    n2 = n2 + 1;
                }
            }
			
            //Inizializzo a 0 tutto l'array bidimensionale n1xn2 isX
            for (i = 0; i < n1; i++)
            {
                for (j = 0; j < n2; j++)
                {
                    isX[i][j] = 0;
                }
            }
            endx = X[0];
			
			
            //assegno a maxW il maggiore tra il totale dei pesi tra la signature1 e signature2
            maxW = sSum > dSum?sSum:dSum;
			
            //chiamo la funzione russel
            russel(s, d);
			
			
            //  _EnterX = _EndX++;  /* AN EMPTY SLOT (ONLY _n1+_n2-1 BASIC VARIABLES) */
            enterX = X[endx.Pos];
            endx = X[endx.Pos + 1];
			
			
			
            //ritorno il minimo tra le somme dei pesi tra la signature1 e signature2
            return sSum > dSum?dSum:sSum;
        }
		
		
		
		
		
		
		
        /// <param name="S">
        /// </param>
        /// <param name="D">
        /// </param>
        private void  russel(double[] S, double[] D)
        {
			
            int i, j, found, minI = 0, minJ = 0;
            int lastCurU, lastCurV;
            double deltaMin, oldVal, diff;
            var delta = new double[n1][];
            for (var i2 = 0; i2 < n1; i2++)
            {
                delta[i2] = new double[n2];
            }
            Node1Content[] ur, vr;
			
            Node1Content n1c, prevU, prevV, curU, curV, headU = new Node1Content(), headV = new Node1Content(), remember = null, prevUMinI = null, prevVMinJ = null;
			
			
			
            ur = new Node1Content[MAX_SIG_SIZE1];
			
			
            for (i = 0; i < ur.Length; i++)
            {
                ur[i] = new Node1Content();
                ur[i].Pos = i;
            }
			
            headU.setNext(ur[0]);
            curU = ur[0];
            for (i = 0; i < n1; i++)
            {
                curU.I = i;
                curU.Val = - INFINITY;
                curU.setNext(ur[curU.Pos + 1]);
                curU = ur[curU.Pos + 1];
            }
            curU.LastNode = true;
			
			
            vr = new Node1Content[MAX_SIG_SIZE1];
			
            for (i = 0; i < vr.Length; i++)
            {
                vr[i] = new Node1Content();
                vr[i].Pos = i;
            }
            headV.setNext(vr[0]);
            curV = vr[0];
            for (j = 0; j < n2; j++)
            {
                curV.I = j;
                curV.Val = - INFINITY;
                curV.setNext(vr[curV.Pos + 1]);
                curV = vr[curV.Pos + 1];
            }
            curV.LastNode = true;
			
			
            for (i = 0; i < n1; i++)
            {
                for (j = 0; j < n2; j++)
                {
                    float v;
                    v = costMatrix[i][j];
                    if (ur[i].Val <= v)
                        ur[i].Val = v;
                    if (vr[j].Val <= v)
                        vr[j].Val = v;
                }
            }
            //		  vr[n2].setLastNode(true);
            //		  ur[n1].setLastNode(true);
			
            for (i = 0; i < n1; i++)
            {
                for (j = 0; j < n2; j++)
                {
                    delta[i][j] = costMatrix[i][j] - ur[i].Val - vr[j].Val;
                }
            }
			
			
			
			
            do 
            {
                found = 0;
                deltaMin = INFINITY;
                prevU = headU;
                for (curU = headU.getNext(); curU.IsLast != true; curU = curU.getNext())
                {
                    int u;
                    u = curU.I;
                    prevV = headV;
                    for (curV = headV.getNext(); curV.IsLast != true; curV = curV.getNext())
                    {
                        var v = curV.I;
                        if (deltaMin > delta[u][v])
                        {
                            deltaMin = delta[u][v];
                            minI = u;
                            minJ = v;
                            prevUMinI = prevU;
                            prevVMinJ = prevV;
                            found = 1;
                        }
                        prevV = curV;
                    }
                    prevU = curU;
                }
				
                if (found == 0)
                {
                    break;
                }
				
                remember = prevUMinI.getNext();
                addBasicVariable(minI, minJ, s, d, prevUMinI, prevVMinJ, headU);
				
                if (remember.Equals(prevUMinI.getNext()))
                {
                    for (curV = headV.getNext(); curV.IsLast == false; curV = curV.getNext())
                    {
                        var v = curV.I;
                        if (curV.Val == costMatrix[minI][v])
                        {
                            oldVal = curV.Val;
                            curV.Val = - INFINITY;
                            for (curU = headU.getNext(); curU.IsLast == false; curU = curU.getNext())
                            {
                                var u = curU.I;
                                if (curV.Val <= costMatrix[u][v])
                                {
                                    curU.Val = costMatrix[u][v];
                                }
                            }
                            diff = oldVal - curV.Val;
                            if (System.Math.Abs(diff) < EPSILON * maxC)
                            {
                                for (curU = headU.getNext(); curU.IsLast; curU = curU.getNext())
                                {
                                    delta[curU.I][v] = delta[curU.I][v] + diff;
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (curU = headU.getNext(); curU.IsLast == false; curU = curU.getNext())
                    {
                        var u = curU.I;
                        if (curU.Val == costMatrix[u][minJ])
                        {
                            oldVal = curU.Val;
                            curU.Val = - INFINITY;
                            for (curV = headV.getNext(); curV.IsLast == false; curV = curV.getNext())
                            {
                                var v = curV.I;
                                if (curU.Val <= costMatrix[u][v])
                                {
                                    curU.Val = costMatrix[u][v];
                                }
                            }
                            diff = oldVal - curU.Val;
                            if (System.Math.Abs(diff) < EPSILON * maxC)
                            {
                                for (curV = headV.getNext(); curV.IsLast == false; curV = curV.getNext())
                                {
                                    delta[u][curV.I] = delta[u][curV.I] + diff;
                                }
                            }
                        }
                    }
                }
            }
            while (headU.getNext().IsLast == false || headV.getNext().IsLast == false);
			
            //				System.out.println("###############");
            //				for (int st1=0;st1<n1;st1++){
            //					for (int st2=0;st2<n2;st2++){
            //				      System.out.print(isX[st1][st2]);
            //				      System.out.print("     ");}
            //				System.out.println();}
        }
		
		
		
		
		
		
		
        /// <param name="minI">
        /// </param>
        /// <param name="minJ">
        /// </param>
        /// <param name="prevUMinI">
        /// </param>
        /// <param name="prevVMinJ">
        /// </param>
        /// <param name="headU">
        /// </param>
        private void  addBasicVariable(int minI, int minJ, double[] s, double[] d, Node1Content prevUMinI, Node1Content prevVMinJ, Node1Content headU)
        {
            // TODO Auto-generated method stub
            double t;
			
			
            //passaggi per associare i nuovi valori agli array s e d
            if (System.Math.Abs(s[minI] - d[minJ]) <= EPSILON * maxW)
            {
                t = s[minI];
                s[minI] = 0;
                d[minJ] = d[minJ] - t;
            }
            else
            {
                if (s[minI] < d[minJ])
                {
                    t = s[minI];
                    s[minI] = 0;
                    d[minJ] = d[minJ] - t;
                }
                else
                {
                    t = d[minJ];
                    d[minJ] = 0;
                    s[minI] = s[minI] - t;
                }
            }
			
            isX[minI][minJ] = 1;
			
            endx.Val = t;
            endx.I = minI;
            endx.J = minJ;
            endx.NC = rowsX[minI];
            endx.NR = colsX[minJ];
			
            rowsX[minI] = endx;
            colsX[minJ] = endx;
            endx = X[endx.Pos + 1];
			
            if ((s[minI] == 0) && (headU.getNext().getNext().IsLast != true))
            {
                prevUMinI.setNext(prevUMinI.getNext().getNext());
            }
            else
            {
                prevVMinJ.setNext(prevVMinJ.getNext().getNext());
            }
        }
    }
}