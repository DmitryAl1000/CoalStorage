using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class StorageRedactorLogic
    {

        const string SEPARATOR = "-";

        public static void ExcudeSlotFromArea(this List<Slot>? Slots,  Slot slotForExclude )
        {
            if (Slots == null) return;

            //Получаем слоты на площадке, на которой наш исключаемый слот
            List<Slot> SlotsOnAreaWhithExcludedSlot = GetSlotsOnAreaWhithExclededSlot(Slots, slotForExclude);

            //предполагаем что слоты идут по порядку, получаем номер позиции пикета на площадке
            int slotPosition = GetPosition(SlotsOnAreaWhithExcludedSlot, slotForExclude);

            //Новые имена площадок. Первое до исключаемого пикета, второе после.
            //Если из 101-105 исключаем 103 слот, то получим: 101-102 и 104-105
            List<string> newAreaNames = GetNewAreasNames(SlotsOnAreaWhithExcludedSlot, slotPosition);

            //Заменяем имена площадок в наших выделенных пикетах относительно позиции исключаемого пикета
            //Сам исключаемый получает свой номер в качестве номера площадки
            ChangeSlotAreasNames(SlotsOnAreaWhithExcludedSlot, newAreaNames, slotPosition);

            //заменяем имена площадок в результате
            ChaneFinalAreasNames(Slots, SlotsOnAreaWhithExcludedSlot);
        }

        private static void ChaneFinalAreasNames(List<Slot>? Slots, List<Slot> SlotsOnAreaWhithExcludedSlot)
        {
            foreach (var editSlots in SlotsOnAreaWhithExcludedSlot)
            {
                foreach (var finalSlot in Slots)
                {
                    if (editSlots == finalSlot)
                        finalSlot.AreaName = editSlots.AreaName;
                }
            }
        }
        private static void ChangeSlotAreasNames(List<Slot> SlotsOnAreaWhithExcludedSlot, List<string> newAreaNames, int slotPosition)
        {
            int areaNum = 0;
            for (int i = 0; i < SlotsOnAreaWhithExcludedSlot.Count; i++)
            {
                if (i == slotPosition)
                {
                    SlotsOnAreaWhithExcludedSlot[i].AreaName = SlotsOnAreaWhithExcludedSlot[i].SlotName;
                    // если первый и последний то не делаем переход
                    if (i != 0 && i != SlotsOnAreaWhithExcludedSlot.Count - 1)
                        areaNum++;
                }
                else
                    SlotsOnAreaWhithExcludedSlot[i].AreaName = newAreaNames[areaNum];
            }

        }
      

        private static List<string> GetNewAreasNames(List<Slot> SlotsOnAreaWhithExcludedSlot, int slotPosition)
        {
            List<string> areaNames = new();
            string newAreaName = string.Empty;

            for (int i = 0; i < SlotsOnAreaWhithExcludedSlot.Count; i++)
            {
                //начальная позиция для имени площадки
                if (i == 0 || i == slotPosition + 1)
                {
                    newAreaName = SlotsOnAreaWhithExcludedSlot[i].SlotName;
                } 
                //Конечная позиция для имени площадки
                if(i == SlotsOnAreaWhithExcludedSlot.Count - 1 || i == slotPosition - 1)
                {
                    //Если она не является и начальной, тогда оставляем только начальную
                    if (i != 0 && i != slotPosition + 1)
                        newAreaName += SEPARATOR + SlotsOnAreaWhithExcludedSlot[i].SlotName;
                    areaNames.Add(newAreaName);
                }
            }
            return areaNames;
        }




        private static int GetPosition(List<Slot>? Slots, Slot slotForExclude)
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                if (slotForExclude.SlotName == Slots[i].SlotName)
                {
                    return i;
                }
            }
            return -1;
        }
        private static List<Slot> GetSlotsOnAreaWhithExclededSlot(List<Slot>? Slots, Slot slotForExclude)
        {
            List<Slot>? SlotsOnAreaWhithExclededSlot = new();
            foreach (var slot in Slots)
            {
                if (slot.AreaName == slotForExclude.AreaName)
                    SlotsOnAreaWhithExclededSlot.Add(slot);
            }
            return SlotsOnAreaWhithExclededSlot;
        }








    }
}
