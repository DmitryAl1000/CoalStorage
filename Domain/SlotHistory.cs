using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class SlotHistory
    {
        public DateTime DateTime { get; set; }

        public string SlotName { get; set; } = string.Empty;

        public string NewAreaName { get; set; } = string.Empty;

        //public Area? Area { get; set; } = new();
        //public Slot Slot { get; set; } = new();
    }
}
