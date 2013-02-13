using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Entities.Interfaces;
using System.Drawing;
using Model.Arenas;
using System.Drawing.Drawing2D;

namespace Model.Entities
{
    public class Item:IDrawable,IUpdateable
    {
        public PointF Location;
        public Arena MyArena { get; set; }
        public int HitPoints { get; set; }

        public double Radius { get; set; }
        public PointF Velocity { get; set; } //0,0 dan x,y ye bir dogru

        public List<AngleRange> CollisionRangeList;
        public Player Owner { get; set; }
        public int Up { get { return Owner.Up; } }
        private Pen Pen = new Pen(Color.White);
        private Pen Pen2 = new Pen(Color.Red);
        public PointF Locationp { get { return Location; } }
        public int Priority;
        public Item()
        {
            Random r=new Random();
            Priority = r.Next();
            CollisionRangeList = new List<AngleRange>();
            HitPoints = 100;
        }
        private Rectangle Rectangle
        {
            get
            {
                return new Rectangle()
                {
                    X = ScreenCoordinate.X - Convert.ToInt32(Radius * MyArena.VERTICAL_RATIO),
                    Y = ScreenCoordinate.Y - Convert.ToInt32(Radius * MyArena.HORIZONTAL_RATIO),
                    Height = Convert.ToInt32(Radius * MyArena.VERTICAL_RATIO *2),
                    Width = Convert.ToInt32(Radius * MyArena.HORIZONTAL_RATIO * 2),
                };
            }
        }

        public virtual void Update()
        {
            throw new NotImplementedException();
        }

        public virtual void Paint(Graphics g)
        {
            g.DrawEllipse(Pen, Rectangle);
             
            SolidBrush sb = new SolidBrush(Color.Red);
            foreach (AngleRange CollisionRange in CollisionRangeList)
            {
                //g.DrawPolygon(Pen2, new Point[] { ToScreenCoordinate(CollisionRange.Apex), ToScreenCoordinate(CollisionRange.tmp1), ToScreenCoordinate(CollisionRange.tmp2) });
            }
            g.DrawLine(Pen, ScreenCoordinate, ToScreenCoordinate(Add(Location ,Velocity)));
        }


        public Point ScreenCoordinate
        {
            get
            {
                return ToScreenCoordinate(Location);
            }
        }

        public Point ToScreenCoordinate(PointF val)
        {
            var p = new Point() { X = 0, Y = 0 };

            p.X = Convert.ToInt32((val.X) * MyArena.HORIZONTAL_RATIO + MyArena.HORIZONTAL_OFFSET );
            p.Y = Convert.ToInt32((val.Y) * MyArena.VERTICAL_RATIO + MyArena.VERTICAL_OFFSET);

            return p;
        }
        public PointF Add(PointF Left, PointF Right)
        {
            return new PointF(Left.X + Right.X, Left.Y + Right.Y);
        }
    }
}
