using Domain;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Application
{
    public class AreaLogic
    {
        List<Area> _areas;



        readonly static string AREA_DELETED_MESSEGE;
        static AreaLogic()
        {
            string settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");

            using (StreamReader r = new StreamReader(settingsPath))
            {
                string json = r.ReadToEnd();
                Dictionary<string, string> settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                AREA_DELETED_MESSEGE = "DELETED";
            }
        }



        public AreaLogic(List<Area> areas)
        {
            this._areas = areas;
        }



        //Имитация первичного запроса к БД
        public static List<Area> SetAreaStartValues()
        {
            List<Area> AreaFromDB = new()
            {
                new Area { AreaName = "101-104", CargoOnArea = 53000 },
                new Area { AreaName = "105", CargoOnArea = 5000},
                new Area { AreaName = "201-202", CargoOnArea = 8000},
                new Area { AreaName = "203-205", CargoOnArea = 15000},
            };
            return AreaFromDB;
        }


        public List<Area> RemoveAreasUsingHistory(IEnumerable<SlotHistory> slotHistorys)
        {
            List<Area> areasForDelete = new();

            foreach (var slotHistory in slotHistorys)
            {
                if (slotHistory.SlotName == AREA_DELETED_MESSEGE)
                {
                    foreach (var area in _areas)
                    {
                        if (area.AreaName == slotHistory.NewAreaName)
                            areasForDelete.Add(area);
                    }
                }
            }
            return areasForDelete;
        }
        private IEnumerable<string> GetUniqueValues(IEnumerable<SlotHistory> slotHistorys)
        {
            var distinctValues = slotHistorys.Select(p => p.NewAreaName)
                                        .Distinct()
                                        .ToList();
            return distinctValues;
        }
        private IEnumerable<SlotHistory> RemoveDeletedAreas(IEnumerable<SlotHistory> slotHistorys)
        {
            var distinctValues = slotHistorys.Select(p => p)
                            .Where(c => c.SlotName != AREA_DELETED_MESSEGE);
            return distinctValues;

        }
        public List<Area> AddNewAreasUsingHistory(IEnumerable<SlotHistory> slotHistorys)
        {
            // удаляем сообщения об удалении площадки
            slotHistorys = RemoveDeletedAreas(slotHistorys);
            // удаляем неуникальные площадки
            var UniqueValues = GetUniqueValues(slotHistorys);

            List<Area> areasForAdd = new();
            //Добавляем все новые площадки кроме помеченных на удаление
            foreach (var slotHistory in UniqueValues)
            {
                Area area = new()
                {
                    AreaName = slotHistory,
                    CargoOnArea = 0,
                };
                areasForAdd.Add(area);
            }
            return areasForAdd;
        }
    }
}
