using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Area
    {
        public Guid AreaId { get; set; }
        public string AreaName { get; set; } = string.Empty;
        public double CargoOnArea { get; set; }
    }
}
