using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Model.Entities.Interfaces
{
    public class AngleRange
    {
        public double First;
        public double Last;
        public PointF Apex;
        public const float Radius = 1.08F;

        public int FirstDegree 
        {
            get
            {
                return (int)RadianToDegree(First);
            }
        }
        public int LastDegree
        {
            get
            {
                return (int)RadianToDegree(Last);
            }
        }
        public int SweepingAngleDegree
        {
            get 
            {
                return LastDegree - FirstDegree;
            }
        }
        private double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }
        public PointF tmp1 
        {
            get
            {
                return new PointF(Apex.X + (float)Math.Cos(First) * Radius, Apex.Y + (float)Math.Sin(First) * Radius);
            }
        }
        public PointF tmp2
        {
            get
            {
                return new PointF(Apex.X + (float)Math.Cos(Last) * Radius, Apex.Y + (float)Math.Sin(Last) * Radius);
            }
        }
        public bool IsInRange(PointF arg)
        {
            //PointF tmp=new PointF(arg.X-Apex.X,arg.Y-Apex.Y);
            //double angle = Math.Atan2(tmp.Y, tmp.X);

            return PointInTriangle(arg, Apex, tmp1, tmp2);
        }
        float sign(PointF p1, PointF p2, PointF p3)
        {
            return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
        }

        bool PointInTriangle(PointF pt, PointF v1, PointF v2, PointF v3)
        {
            bool b1, b2, b3;

            b1 = sign(pt, v1, v2) < 0.0f;
            b2 = sign(pt, v2, v3) < 0.0f;
            b3 = sign(pt, v3, v1) < 0.0f;

            return ((b1 == b2) && (b2 == b3));
        }
    }
}
