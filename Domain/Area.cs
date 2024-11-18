namespace Domain
{
    public class Area
    {
        public int AreaId { get; set; }
        public string AreaName { get; set; } = string.Empty;
        public double CargoOnArea { get; set; }


        //public List<CargoHistory> CargoHistory { get; set; } = new();
        //public List<Slot> SlotList { get; set; } = new();
    }
}
