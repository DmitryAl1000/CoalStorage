using Domain;

namespace Application
{
    public class AreaLogic
    {
        public static List<Area> SetAreaStartValues()
        {
            List<Area> AreaFromDB = new()
            {
                new Area { AreaName = "101-104", CargoOnArea = 53000 },
                new Area { AreaName = "105", CargoOnArea = 5000},
                new Area { AreaName = "201-202", CargoOnArea = 8000},
                new Area { AreaName = "203-205", CargoOnArea = 15000},
            };
            return AreaFromDB;
        }




    }
}
