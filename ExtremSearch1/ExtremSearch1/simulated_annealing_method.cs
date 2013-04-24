using System;
using System.Collections;
using System.Collections.Generic;
using Common;

namespace SimAnnealing
{
    public class SimAnnealing
    {
        struct Solution
        {
            public Point point;
            public double profit;
            public Solution(int n)
            {
                point = new Point(n);
                profit = 0;
            }
        }
        double TInit;
        double T;
        int argCnt;
        Point rangeMin;
        Point rangeMax;
        int iterMax;
        int optOrient;
        double alpha;
        Parser.PostfixNotationExpression func;
        static Random rand;

        public SimAnnealing(double T0, int myArgCnt, double[] rMin, double[] rMax, int itMax, int myOptOrient, double myAlpha, Parser.PostfixNotationExpression myFunc)
        {
            TInit = T0;
            T = T0;
            argCnt = myArgCnt;
            rangeMax = new Point(myArgCnt, rMax);
            rangeMin = new Point(myArgCnt, rMin);
            iterMax = itMax;
            optOrient = -myOptOrient;
            alpha = myAlpha;
            func = myFunc;
            rand = new Random();
        }
        private Solution getRandSolution()
        {
            Solution tmp = new Solution(argCnt);
            for (int i = 0; i < argCnt; i++)
            {
                tmp.point[i] = rand.NextDouble() * (rangeMax[i] - rangeMin[i]) + rangeMin[i];
            }
            tmp.profit = getProfit(tmp.point);
            return tmp;
        }
        private double getProfit(Point x)
        {
            return optOrient * func.result(argCnt, x.x);
        }
        private int getRandSign()
        {
            return (rand.NextDouble() >= 0.5 ? -1 : 1);
        }
        private void checkRange(ref Solution curr)
        {
            for (int i = 0; i < argCnt; i++)
            {
                curr.point[i] = (curr.point[i] > rangeMax[i] ? rangeMax[i] : curr.point[i]);
                curr.point[i] = (curr.point[i] < rangeMin[i] ? rangeMin[i] : curr.point[i]);
            }
        }           
        private Solution chooseNext(Solution curr)
        {
            Solution tmp = new Solution(argCnt);           
            for (int i = 0; i < argCnt; i++)
            {
            //    tmp.point[i] = curr.point[i] + getRandSign() * (rangeMax[i] - rangeMin[i]) * T / TInit * rand.NextDouble();
                tmp.point[i] = curr.point[i] + getRandSign() * (rangeMax[i] - rangeMin[i]) * rand.NextDouble();      
            }
            checkRange(ref tmp);
            tmp.profit = getProfit(tmp.point);
            return tmp;
        }
        private bool admit(double delta)
        {
            return (Math.Exp(-delta / T) > rand.NextDouble());
        }
        public double[] search()
        {
            Solution curr = getRandSolution();
            Solution best = curr;
            Solution next;
            for (int it = 0; it < iterMax; it++)
            {
                next = chooseNext(curr);
                if (admit(curr.profit - next.profit))
                {
                    curr = next;
                }
                if (best.profit > curr.profit)
                {
                    best = curr;
                }
                T *= alpha;
            }
            return best.point.x;
        }
    }
}