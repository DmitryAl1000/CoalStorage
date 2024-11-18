using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Slot
    {
        public int SlotId { get; set; }
        public string SlotName { get; set; } = string.Empty;
        public string AreaId { get; set; } = string.Empty;
        public int StorageId { get; set; }

        //public Area? Area { get; set; }
        //public List<SlotHistory> SlotHistory { get; set; } = new();
    }
}
