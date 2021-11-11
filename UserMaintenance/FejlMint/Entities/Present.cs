using FejlMint.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FejlMint.Entities
{
    public class Present : Toy
    {
        public SolidBrush BoxColor { get; private set; }
        public SolidBrush RibbonColor { get; private set; }
        public Present(Color bc, Color rc)
        {
            BoxColor = new SolidBrush(bc);
            RibbonColor = new SolidBrush(rc);
        }
        protected override void DrawImage(Graphics g)
        {
            g.FillRectangle(BoxColor, 0, 0, Width, Height);
            g.FillRectangle(RibbonColor, 0, 20, Width, Height / 5);
            g.FillRectangle(RibbonColor, 20, 0, Width/5, Height);
        }
    }
}
