using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Slot
    {
        public Guid SlotId { get; set; }
        public string SlotName { get; set; } = string.Empty;
        public string AreaName { get; set; } = string.Empty;
        public int StorageId { get; set; }
    }
}
