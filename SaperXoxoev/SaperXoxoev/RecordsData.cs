using Newtonsoft.Json;

namespace SaperXoxoev
{
    public class RecordsData
    {
        [JsonProperty("Лучшее время Новичок")]
        public int EasyRecord { get; set; } = 0;

        [JsonProperty("Лучшее время Любитель")]
        public int MediumRecord { get; set; } = 0;

        [JsonProperty("Лучшее время Профессионал")]
        public int HardRecord { get; set; } = 0;

        [JsonProperty("Лучшее время Особый")]
        public int ExpertRecord { get; set; } = 0;

        [JsonProperty("Всего игр")]
        public int TotalGames { get; set; } = 0;

        [JsonProperty("Побед")]
        public int Wins { get; set; } = 0;

        [JsonProperty("Поражений")]
        public int Losses { get; set; } = 0;
    }
}