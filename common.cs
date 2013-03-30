using System;

namespace Common
{
    public class Point
    {
        public double[] x;
        int n;
        public Point(int num)
        {
            n = num;
            x = new double[n];
        }
        public Point(int num, double[] p)
        {
            n = num;
            x = new double[n];
            Array.Copy(p, x, n);
        }
        public void Copy(Point a)
        {
            n = a.n;
            Array.Copy(a.x, x, n);
            return;
        }
        public double this[int pos]
        {
            get
            {
                return x[pos];
            }
            set
            {
                x[pos] = value;
            }
        }
    }
}

