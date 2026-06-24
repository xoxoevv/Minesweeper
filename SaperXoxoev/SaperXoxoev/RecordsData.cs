using Newtonsoft.Json;

namespace SaperXoxoev
{
    public class RecordsData
    {
        [JsonProperty("Лучшее_время_Новичок")]
        public int EasyRecord { get; set; } = 0;

        [JsonProperty("Лучшее_время_Любитель")]
        public int MediumRecord { get; set; } = 0;

        [JsonProperty("Лучшее_время_Профессионал")]
        public int HardRecord { get; set; } = 0;

        [JsonProperty("Лучшее_время_Особый")]
        public int ExpertRecord { get; set; } = 0;

        [JsonProperty("Всего_игр")]
        public int TotalGames { get; set; } = 0;

        [JsonProperty("Побед")]
        public int Wins { get; set; } = 0;

        [JsonProperty("Поражений")]
        public int Losses { get; set; } = 0;
    }
}