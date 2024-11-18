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
                new Slot { SlotName = "101", AreaId ="101-104", StorageId = 1 },
                new Slot { SlotName = "102", AreaId ="101-104", StorageId = 1 },
                new Slot { SlotName = "103", AreaId ="101-104", StorageId = 1 },
                new Slot { SlotName = "104", AreaId ="101-104", StorageId = 1 },
                new Slot { SlotName = "105", AreaId ="105", StorageId = 1 },

                new Slot { SlotName = "201", AreaId ="201-202", StorageId = 2 },
                new Slot { SlotName = "202", AreaId ="201-202", StorageId = 2 },
                new Slot { SlotName = "203", AreaId ="203-205", StorageId = 2 },
                new Slot { SlotName = "204", AreaId ="203-205", StorageId = 2 },
                new Slot { SlotName = "205", AreaId ="203-205", StorageId = 2 },



            };
           return SlotsFromDB;
        }
    }
}
