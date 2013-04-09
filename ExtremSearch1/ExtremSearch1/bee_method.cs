using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Parser;

namespace BeeMethod
{

    class BeeMethod
    {
        struct Bee
        {
            public Point point;
            public double profit;
            public void Copy(Bee a)
            {
                point.Copy(a.point);
                profit = a.profit;
            }
            public Bee(int n)
            {
                point = new Point(n);
                profit = 0;
            }
        };
        int B; // current number of explorers
        double alpha;
        double T; //current temperature
        double TFinal; // final temperatire
        int argCnt; // number of variables
        int iterMax; // number of iterations 
        int iter; //current iteration
        Point rangeMin;
        Point rangeMax;
        double wMax;
        double eMax;
        double eta;
        double beta;
        double gamma;
        double range;
        int optOrient; // 1 for max, -1 for min
        Parser.PostfixNotationExpression func;
        static Random rand;
           
        public BeeMethod(int Bs, double a, double T0, double T1, int n, int maxIt, double[] rMin, double[] rMax, double w, double e, double myEta, double myBeta, double myGamma, double myRange, int orient, Parser.PostfixNotationExpression myFunc)
        {
            B = Bs; // must be > 0
            alpha = a;
            T = T0;
            TFinal = T1;
            argCnt = n; // must be > 0
            iterMax = maxIt; // must be > 0
            rangeMin = new Point(argCnt, rMin); // 
            rangeMax = new Point(argCnt, rMax); //
            wMax = w;
            eMax = e;
            eta = myEta;
            beta = myBeta;
            gamma = myGamma;
            range = myRange;
            optOrient = orient; // must be +-1
            func = myFunc;
            rand = new Random();
        }
        private int getRandSign()
        {
            return (rand.NextDouble() < 0.5) ? 1 : -1;
        }
        private void checkRange(ref Bee tmp)
        {
            for (int i = 0; i < argCnt; i++)
            {
                tmp.point[i] = tmp.point[i] < rangeMin[i] ? rangeMin[i] : tmp.point[i];
                tmp.point[i] = tmp.point[i] > rangeMax[i] ? rangeMax[i] : tmp.point[i];
            }
        }
        private double getProfit(Bee tmp)
        {
            return optOrient * func.result(argCnt, tmp.point.x);
        }
        private Bee getRandBee()
        {
            Bee tmp = new Bee(argCnt);
            for (int j = 0; j < argCnt; j++)
            {
                tmp.point[j] = rand.NextDouble() * (rangeMax[j] - rangeMin[j]) + rangeMin[j];
                tmp.profit = getProfit(tmp);
            }
            return tmp;
        }

        public double[] search()
        {
            List<Bee> bee = new List<Bee>();
            Bee best = new Bee(argCnt);
            List<Bee> workBee = new List<Bee>();
            List<Bee> newWorkBee = new List<Bee>();
            Bee tmp;

            //Step 2
            for (int i = 0; i < B; i++)
            {
                tmp = getRandBee();
                bee.Add(tmp);
            }
            iter = 1;
            //Step 3
            best.Copy(bee.First());
            while (T != TFinal && iter != iterMax)
            {
                newWorkBee.Clear();
                workBee.Clear();
                for (int i = 0; i < bee.Count; i++)
                {
                    if (best.profit < bee[i].profit)
                    {
                        best.Copy(bee[i]);
                    }
                }
                for (int i = 0; i < bee.Count; i++)
                {
                    double r = Math.Exp(-Math.Abs(bee[i].profit - best.profit) / T);
                    if (Math.Exp(-Math.Abs(bee[i].profit - best.profit) / T) > rand.NextDouble())
                    {
                        workBee.Add(bee[i]);
                    }
                }
                workBee.Add(best);
                //Step 4
                foreach (Bee currBee in workBee)
                {
                    tmp = new Bee(argCnt);
                    for (int i = 0; i < argCnt; i++)
                    {
                        tmp.point[i] = currBee.point[i] - getRandSign() * rand.NextDouble() * (currBee.point[i] - best.point[i]);
                    }
                    checkRange(ref tmp);
                    tmp.profit = getProfit(tmp);
                    newWorkBee.Add(tmp);
                }
                foreach (Bee currBee in workBee)
                {
                    tmp = new Bee(argCnt);
                    for (int i = 0; i < argCnt; i++)
                    {
                        tmp.point[i] = best.point[i] - getRandSign() * rand.NextDouble() * (currBee.point[i] - best.point[i]);
                    }
                    checkRange(ref tmp);
                    tmp.profit = getProfit(tmp);
                    newWorkBee.Add(tmp);
                }
                newWorkBee.AddRange(workBee);
                foreach (Bee currBee in newWorkBee)
                {
                    if (best.profit < currBee.profit)
                    {
                        best.Copy(currBee);
                    }
                }
                //Step 5
                double fullProfit = 0;
                foreach (Bee currBee in newWorkBee)
                {
                    fullProfit += currBee.profit;
                }
                double d;
                double L;
                bee.Clear();
                for (int i = 0; i < newWorkBee.Count; i++)
                {
                    d = newWorkBee[i].profit / fullProfit;
                    d += getRandSign() * rand.NextDouble() * wMax;
                    d = d > 1 ? 1 : d;
                    d = d < eMax ? 0 : d;
                    L = (d - eta * fullProfit / newWorkBee.Count) < 0 ? 0 : (d - eta * fullProfit / newWorkBee.Count);
                    if (L / beta > (gamma * fullProfit / newWorkBee.Count))
                    {
                        bee.Add(newWorkBee[i]); //danced bee are added

                        tmp = new Bee(argCnt);
                        for (int j = 0; j < argCnt; j++)
                        {
                            tmp.point[j] = newWorkBee[i].point[j] + range * rand.NextDouble() - range / 2;
                        }
                        checkRange(ref tmp);
                        tmp.profit = getProfit(tmp);
                        bee.Add(tmp);
                    }
                    else
                    {
                        tmp = getRandBee();
                        bee.Add(tmp);
                    }

                }
                iter++;
                T *= alpha;
                range *= (1 - (double)iter / iterMax);
            }
            return (best.point.x);
        }
    }
}
