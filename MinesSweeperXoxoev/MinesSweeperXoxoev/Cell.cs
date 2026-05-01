using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesSweeperXoxoev
{
    internal class Cell
    {
        public bool HasMine { get; set; }
        public int NeighborMines { get; set; }
        public bool IsOpen { get; set; }

    }
}
