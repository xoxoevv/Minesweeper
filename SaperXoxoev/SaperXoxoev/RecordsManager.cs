using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SaperXoxoev
{
    public static class RecordsManager
    {
        private static string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"records.json");

        public static RecordsData LoadRecords()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    return JsonConvert.DeserializeObject<RecordsData>(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка загрузки рекордов: " + ex.Message);
            }

            return new RecordsData();
        }

        public static void SaveRecords(RecordsData records)
        {
            try
            {
                string json = JsonConvert.SerializeObject(records, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка сохранения рекордов: " + ex.Message);
            }
        }

        public static void UpdateRecord(int bombsCount, int seconds, RecordsData records)
        {
            switch (bombsCount)
            {
                case 10:
                    if (records.EasyRecord == 0 || seconds < records.EasyRecord)
                        records.EasyRecord = seconds;
                    break;

                case 13:
                    if (records.MediumRecord == 0 || seconds < records.MediumRecord)
                        records.MediumRecord = seconds;
                    break;

                case 16:
                    if (records.HardRecord == 0 || seconds < records.HardRecord)
                        records.HardRecord = seconds;
                    break;

                case 52:
                    if (records.ExpertRecord == 0 || seconds < records.ExpertRecord)
                        records.ExpertRecord = seconds;
                    break;
            }

            SaveRecords(records);
        }
    }
}