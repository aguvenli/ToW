using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Entities;
using Model.Entities.Interfaces;
using System.Drawing;

namespace Model.Arenas
{
    public class Arena:IDrawable
    {
        public Arena()
        {
            AllItems = new List<Item>();
        }


        public readonly float TILE_SIZE_Y = 0.2F;
        public readonly float TILE_SIZE_X = 0.2F;

        public ICollection<Item> AllItems { get; set; }
        public IEnumerable<Unit> AllUnits 
        {
            get
            {
                return AllItems.Where(a => a is Unit).Cast<Unit>();
            }
        }

        public Map Map { get; set; }

        public readonly int HORIZONTAL_OFFSET = 450;
        public readonly int VERTICAL_OFFSET = 150;

        public readonly int HORIZONTAL_RATIO = 150;
        public readonly int VERTICAL_RATIO = 150;



        public void Paint(System.Drawing.Graphics g)
        {
            SolidBrush sb=new SolidBrush(Color.Black);

            g.FillRectangle(sb, 0, 0, HORIZONTAL_OFFSET * 2, VERTICAL_OFFSET * 2);
            foreach (var item in AllItems)
            {
                item.Paint(g);
            }
        }

        public void Update()
        {
            foreach (var item in AllItems)
            {
                item.Update();
            }

            List<Item> CloneItems = AllItems.Where(a=>a.HitPoints>0).ToList();
            AllItems = CloneItems;
        }

        public void DoAILoop()
        {
            foreach (var unit in AllUnits)
            {
                unit.Act();
            }
        }

        public static float GetDistance(PointF from, PointF to)
        {
            return (float)Math.Sqrt((from.X - to.X) * (from.X - to.X) + (from.Y - to.Y) * (from.Y - to.Y));
        }
    }
}
