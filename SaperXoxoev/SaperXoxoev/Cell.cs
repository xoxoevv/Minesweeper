using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaperXoxoev
{
    public class Cell
    {
        public bool IsMine { get; set; }

        public bool IsOpen { get; set; }

        public bool IsFlag { get; set; }

        public int NearMines { get; set; }
    }
}
