using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Model.Entities
{
    public class MapItem:Item
    {
       

        public Point GridLocation 
        { 
            get
            {
                return new Point() { 
                    X = Convert.ToInt32(Location.X / MyArena.TILE_SIZE_X),
                    Y = Convert.ToInt32(Location.Y / MyArena.TILE_SIZE_Y)
                };
            }
            set
            {
                Location.X = value.X * MyArena.TILE_SIZE_X;
                Location.Y = value.Y * MyArena.TILE_SIZE_Y;
            }
        }
    }
}
