using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Application
{
    public class SlotHistoryLogic
    {
        public static List<SlotHistory> GetFirstFreezeSlotHistory(IEnumerable<Slot> slots)
        {
            List<SlotHistory> slotHistoryes = new();
            foreach (var slot in slots)
            {
                SlotHistory slotHistory = new()
                {
                    DateTime = DateTime.Now,
                    SlotName = slot.SlotName,
                    NewAreaName = slot.AreaName
                };
                slotHistoryes.Add(slotHistory);
            }
            return slotHistoryes;
        }

        public static List<string> FindAllAreasNamesByDate(IEnumerable<Slot>? Slots, IEnumerable<SlotHistory>? SlotHistorys, DateTime? dateTime)
        {
            List<string> areaNames = new();
            if (Slots is null || SlotHistorys is null) return areaNames;

            var HistoryBeforeDate = SlotHistorys.Where(p => p.DateTime < dateTime).ToList();

            foreach (var slot in Slots)
            {
                for (int i = HistoryBeforeDate.Count - 1; i >= 0; i--)
                {
                    if (slot.SlotName == HistoryBeforeDate[i].SlotName)
                    {
                        areaNames.Add(HistoryBeforeDate[i].NewAreaName);
                        break;
                    }
                }
            }
            areaNames = areaNames.Distinct().Order().ToList();
            return areaNames;
        }
    }
}
