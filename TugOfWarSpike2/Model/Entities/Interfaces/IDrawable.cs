using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Model.Entities.Interfaces
{
    public interface IDrawable
    {
        void Paint(Graphics g);
    }
}
