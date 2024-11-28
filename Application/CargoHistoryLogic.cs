using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Application
{
    public class CargoHistoryLogic
    {
        
        public static List<CargoHistory> GetFirstFreezeCargoHistory(IEnumerable<Area> areas)
        {
            List<CargoHistory> cargoHistorys = new();
            foreach (var area in areas)
            {
                CargoHistory cargo = new()
                {
                    DateTime = DateTime.Now,
                    AreaName = area.AreaName,
                    CargoOnArea = area.CargoOnArea
                };
                cargoHistorys.Add(cargo);
            }
            return cargoHistorys;
        }


        public static List<Area>? FindAllCargoByDate(IEnumerable<string> arealist, IEnumerable<CargoHistory> cargoHistory, DateTime? dateTime)
        {
            List<Area>? areas = new();
            if (cargoHistory is null) return areas;

            var HistoryBeforeDate = cargoHistory.Where(p => p.DateTime < dateTime).ToList();

            foreach (var area in arealist)
            {
                for (int i = HistoryBeforeDate.Count - 1; i >= 0; i--)
                {
                    if (area == HistoryBeforeDate[i].AreaName)
                    {
                        Area newArea = new()
                        {
                            AreaName = HistoryBeforeDate[i].AreaName,
                            CargoOnArea = HistoryBeforeDate[i].CargoOnArea
                        };
                        areas.Add(newArea);
                        break;
                    }
                }
            }
            areas = areas.OrderBy(p => p.AreaName).ToList();
            return areas;
        }
    }
}
