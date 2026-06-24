using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaperXoxoev
{
    public class RecordsData
    {
        public int EasyRecord { get; set; } = 0;     
        public int MediumRecord { get; set; } = 0;   
        public int HardRecord { get; set; } = 0;     
        public int ExpertRecord { get; set; } = 0;  

        // Количество сыгранных партий (для статистики)
        public int TotalGames { get; set; } = 0;
        public int Wins { get; set; } = 0;
        public int Losses { get; set; } = 0;
    }
}