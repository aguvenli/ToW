using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Model.Entities
{
    public class Player
    {
        public Player(int Up)
        {
            this.Up = Up;
            switch (Up)
            {
                case 1:
                    DefaultVelocity= new PointF() { X = 0.01F, Y = 0.00F};
                    TargetLocation = new PointF() { X = 14, Y = 0.00F };
                    break;
                case 2:
                    DefaultVelocity = new PointF() { X = -0.01F, Y = 0.00F };
                    TargetLocation = new PointF() { X = -14, Y = 0.00F };
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public int Up { get; private set; }
        public PointF DefaultVelocity { get; private set; }
        public PointF TargetLocation { get; private set; }
    }
}
