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

        public int SlotId { get; set; }

        public string NewAreaId { get; set; } = string.Empty;

        //public Area? Area { get; set; } = new();
        //public Slot Slot { get; set; } = new();

    }
}
