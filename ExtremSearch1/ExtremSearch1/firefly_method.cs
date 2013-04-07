using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace FireflyMethod
{
    class FireflyMethod
    {
        struct Firefly
        {
            public Point point;
            public double intensity;
            public Firefly(int n)
            {
                point = new Point(n);
                intensity = 0;
            }
        }
        int B;
        int argCnt;
        Point rangeMin;
        Point rangeMax;
        double gamma;
        int optOrient;
        int iterMax;
        double alpha;
        double beta0;
        Parser.PostfixNotationExpression func;
        static Random rand;   
        public FireflyMethod(int myB, int myArgCnt, double[] rMin, double[] rMax, double myGamma, int itMax, int myOptOrient, double myAlpha, double myBeta0, Parser.PostfixNotationExpression myFunc)
        {
            B = myB;
            argCnt = myArgCnt;
            rangeMax = new Point(myArgCnt, rMax);
            rangeMin = new Point(myArgCnt, rMin);
            gamma = myGamma;
            iterMax = itMax;
            optOrient = myOptOrient;
            alpha = myAlpha;
            beta0 = myBeta0;
            func = myFunc;
            rand = new Random();        
        }
        private double getIntensity(Point x)
        {
            return optOrient * func.result(argCnt, x.x);
        }
        private Firefly getRandomFirefly()
        {
            Firefly tmp = new Firefly(argCnt);
            for (int i = 0; i < argCnt; i++)
            {
                tmp.point[i] = rand.NextDouble() * (rangeMax[i] - rangeMin[i]) + rangeMin[i];
            }
            tmp.intensity = getIntensity(tmp.point);
            return tmp;
        }

        double getDestination2(Firefly a, Firefly b)
        {
            double d = 0;
            for (int i = 0; i < argCnt; i++)
            {
                d += (a.point[i] - b.point[i]) * (a.point[i] - b.point[i]);
            }
            return d;
        }
        private void moveFirefly(ref Firefly a, ref Firefly b)
        {
            double dist2 = getDestination2(a, b);
            double currRand = rand.NextDouble();
            for (int i = 0; i < argCnt; i++)
            {
                a.point[i] += beta0 * Math.Exp(-gamma * dist2) * (b.point[i] - a.point[i]) + alpha * (currRand - 0.5);
            }
        }

        public double[] search()
        {
            Firefly[] firefly = new Firefly [B];
            for (int i = 0; i < B; i++)
            {
                firefly[i].point = new Point(argCnt);
            }
            Firefly best = new Firefly();
            for(int i = 0; i < B; i++)
            {
                firefly[i] = getRandomFirefly();
            }
            for(int iter = 0; iter < iterMax; iter++)
            {
                for(int i = 0; i < B; i++)
                {
                    for (int j = 0; j < B; j++)
                    {
                        if (firefly[j].intensity > firefly[i].intensity)
                        {
                            moveFirefly(ref firefly[i], ref firefly[j]);
                            firefly[i].intensity = getIntensity(firefly[i].point);
                        }
                    }
                }
                best = firefly[0];
                for(int i = 0; i < B; i++)
                {
                    if (firefly[i].intensity > best.intensity)
                    {
                        best = firefly[i];
                    }
                }
            }
            return best.point.x;
        }
    }
}