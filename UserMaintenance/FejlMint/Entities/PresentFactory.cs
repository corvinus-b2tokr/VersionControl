using FejlMint.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FejlMint.Entities
{
    public class PresentFactory : IToyFactory
    {
        public Color BoxColor { get; set; }
        public Color RibonColor { get; set; }

        public Toy CreateNew()
        {
            return new Present(BoxColor, RibonColor);
        }
    }
}
