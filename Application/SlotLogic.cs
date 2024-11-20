using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Application
{
    public class SlotLogic
    {

        public static List<Slot> SetSlotStartValues()
        {
            List<Slot> SlotsFromDB = new()
            {
                new Slot { SlotName = "101", AreaName ="101-104", StorageId = 1 },
                new Slot { SlotName = "102", AreaName ="101-104", StorageId = 1 },
                new Slot { SlotName = "103", AreaName ="101-104", StorageId = 1 },
                new Slot { SlotName = "104", AreaName ="101-104", StorageId = 1 },
                new Slot { SlotName = "105", AreaName ="105", StorageId = 1 },

                new Slot { SlotName = "201", AreaName ="201-202", StorageId = 2 },
                new Slot { SlotName = "202", AreaName ="201-202", StorageId = 2 },
                new Slot { SlotName = "203", AreaName ="203-205", StorageId = 2 },
                new Slot { SlotName = "204", AreaName ="203-205", StorageId = 2 },
                new Slot { SlotName = "205", AreaName ="203-205", StorageId = 2 },
            };
            return SlotsFromDB;
        }


    }
}
