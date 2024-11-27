using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static System.Reflection.Metadata.BlobBuilder;
using System.Data.SqlTypes;

namespace Application
{
    public class SlotsRedactor
    {
        static SlotsRedactor()
        {
            string settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");
            //Подгрузка общих настроек
            using (StreamReader r = new StreamReader(settingsPath))
            {
                string json = r.ReadToEnd();
                Dictionary<string, string> settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                AREA_SEPARATOR = settings[nameof(AREA_SEPARATOR)];
                AREA_DELETED_MESSAGE = settings[nameof(AREA_DELETED_MESSAGE)];
            }
        }

        readonly static string AREA_SEPARATOR;
        public readonly static string AREA_DELETED_MESSAGE;

        public List<SlotHistory> SlotChangeHistory { get; }

        //Внутренние переменные
        private List<Slot>? _slots;
        private Slot _slotForExclude;
        private List<Slot> _slotsOnArea;
        private int _slotPositionInArea;

        // при разбитии площадки мы получаем 1 или 2 новых площадки
        private List<string> _newAreaNames;
        // для нового имени площадки когда мы соединяем несколько пикетов
        public string NewAreaName { get; set; }

        public SlotsRedactor(List<Slot> Slots)
        {
            SlotChangeHistory = new();
            _slotForExclude = new();
            _slotsOnArea = new();
            _newAreaNames = new();

            if (Slots is null)
                _slots = new();
            else
                this._slots = Slots;
        }


        //Получаем слeдующую и предыдующую площадки, чтобы показывать только их для перемещения
        public List<string> GetAreasForMoving(Slot selectedSlot)
        {
            List<string> areasForMoving = new();
            var selectedIndex = _slots.IndexOf(selectedSlot);
            int storageIndex = selectedSlot.StorageId;
            // Если индекс не найден. Может встречаться первой асинхронной загрузке, когда база подгружается медленно
            if (selectedIndex < 0)
            {
                List<string> EmptyList = new();
                return EmptyList;
            }

            //Предыдущая площадка на данном складе
            if (selectedIndex != 0)
            {
                if (_slots[selectedIndex - 1].AreaName != _slots[selectedIndex].AreaName &&
                    _slots[selectedIndex - 1].StorageId == _slots[selectedIndex].StorageId)
                {
                    areasForMoving.Add(_slots[selectedIndex - 1].AreaName);
                }
            }
            //Последущая площадка на данном складе
            if (selectedIndex != _slots.Count)
            {
                if (_slots[selectedIndex + 1].AreaName != _slots[selectedIndex].AreaName &&
                    _slots[selectedIndex + 1].StorageId == _slots[selectedIndex].StorageId)
                {
                    areasForMoving.Add(_slots[selectedIndex + 1].AreaName);
                }
            }
            return areasForMoving;
        }

        ///Перемещение слота на другую площадку
        public void MoveSlotToArea(Slot slotForMoving, string resultArea)
        {
            //Исключаем слот, если он уже был на другой площадке
            if (slotForMoving.SlotName != slotForMoving.AreaName)
                ExcludeSlotFromArea(slotForMoving);

            //Получаем слоты на площадке на которую будем перемещать наш слот
            _slotsOnArea = GetsSlotOnArea(resultArea);
            _slotsOnArea.Add(slotForMoving);

            // добавляем в историю что удалили площадку на которую будем перемещать и площадку перемещаемого слота
            AddToHistory(AREA_DELETED_MESSAGE, slotForMoving.AreaName);
            AddToHistory(AREA_DELETED_MESSAGE, resultArea);

            // Получаем имя новой площадки
            NewAreaName = GetNewAreaName(_slotsOnArea);

            //Проставляем всем слотам новое имя
            foreach (var slot in _slotsOnArea)
            {
                slot.AreaName = NewAreaName;
            }

            //Заменяем имена площадок на выбранных слотах и добавляем в историю
            ChangeAreasNamesInResultSlots();
        }

        ///Исключить слот из Площадки
        public void ExcludeSlotFromArea(Slot slotForExclude)
        {
            if (slotForExclude == null) return;
            // добавляем в историю что удалили площадку
            AddToHistory(AREA_DELETED_MESSAGE, slotForExclude.AreaName);

            //Получаем слоты на площадке, на которой наш исключаемый слот
            _slotsOnArea = GetSlotsOnAreaWhithExclededSlot(slotForExclude);

            //предполагаем что слоты идут по порядку, получаем номер позиции пикета на площадке
            _slotPositionInArea = GetPosition(slotForExclude);

            //Новые имена площадок. Первое до исключаемого пикета, второе после.
            //Если из 101-105 исключаем 103 слот, то получим: 101-102 и 104-105
            _newAreaNames = GetNewAreasNamesForExcludedSlot();

            //Заменяем имена площадок в наших выделенных пикетах относительно позиции исключаемого пикета
            //Сам исключаемый получает свой номер в качестве номера площадки
            ChangeAreaNamesInCurrenArea();

            //заменяем имена площадок в результате
            ChangeAreasNamesInResultSlots();
        }


        //===================================================
        //=============== Вспомогательные ===================
        //===================================================

        private string GetNewAreaName(List<Slot> slotsOnArea)
        {
            slotsOnArea = slotsOnArea.OrderBy(p => p.SlotName).ToList();

            string newAreaName = slotsOnArea[0].SlotName;
            newAreaName += AREA_SEPARATOR;
            newAreaName += slotsOnArea[slotsOnArea.Count-1].SlotName; //-1 так как отсчет Count идет с 1, а нумерация с нуля

            return newAreaName;
        }
        private void AddToHistory(string slotName, string changedAreaName)
        {
            SlotHistory slotHistory = new SlotHistory()
            {
                DateTime = DateTime.Now,
                SlotName = slotName,
                NewAreaName = changedAreaName
            };
            SlotChangeHistory.Add(slotHistory);
        }
        private List<Slot> GetsSlotOnArea(string areaName)
        {
            List <Slot> emptyList = new();
            if (_slots is null)
                return emptyList;

            var slotsOnArea = _slots.Where(p => p.AreaName == areaName).ToList();
            return slotsOnArea;
        }
        private void ChangeAreasNamesInResultSlots()
        {
            List<Slot> emptyList = new();
            if (_slots is null)
                return;

            foreach (var fixedSlot in _slotsOnArea)
            {
                foreach (var mainSlot in _slots)
                {
                    if (fixedSlot == mainSlot)
                    {
                        mainSlot.AreaName = fixedSlot.AreaName;
                        AddToHistory(mainSlot.SlotName, mainSlot.AreaName);
                    }    
                }
            }
        }
        private void ChangeAreaNamesInCurrenArea()
        {
            int areaNum = 0;
            for (int i = 0; i < _slotsOnArea.Count; i++)
            {
                if (i == _slotPositionInArea)
                {
                    _slotsOnArea[i].AreaName = _slotsOnArea[i].SlotName;
                    // если первый и последний то не делаем переход
                    if (i != 0 && i != _slotsOnArea.Count - 1)
                        areaNum++;
                }
                else
                    _slotsOnArea[i].AreaName = _newAreaNames[areaNum];
            }

        }
        private List<string> GetNewAreasNamesForExcludedSlot()
        {
            List<string> areaNames = new();
            string newAreaName = string.Empty;

            for (int i = 0; i < _slotsOnArea.Count; i++)
            {
                //начальная позиция для имени площадки
                if (i == 0 || i == _slotPositionInArea + 1)
                {
                    newAreaName = _slotsOnArea[i].SlotName;
                }
                //Конечная позиция для имени площадки
                if (i == _slotsOnArea.Count - 1 || i == _slotPositionInArea - 1)
                {
                    //Если она не является и начальной, тогда оставляем только начальную
                    if (i != 0 && i != _slotPositionInArea + 1)
                        newAreaName += AREA_SEPARATOR + _slotsOnArea[i].SlotName;
                    areaNames.Add(newAreaName);
                }
            }
            return areaNames;
        }
        private int GetPosition(Slot slotForExclude)
        {
            for (int i = 0; i < _slotsOnArea.Count; i++)
            {
                if (slotForExclude.SlotName == _slotsOnArea[i].SlotName)
                {
                    return i;
                }
            }
            return -1;
        }
        private List<Slot> GetSlotsOnAreaWhithExclededSlot(Slot slotForExclude)
        {
            List<Slot> emptyList = new();
            if (_slots is null)
                return emptyList;

            var SlotsOnAreaWhithExclededSlot = _slots.Where(a => a.AreaName == slotForExclude.AreaName).ToList();
            return SlotsOnAreaWhithExclededSlot;
        }
    }
}
