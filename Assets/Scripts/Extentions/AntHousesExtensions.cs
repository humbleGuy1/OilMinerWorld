using System.Linq;
using System.Collections.Generic;
using static Assets.Scripts.CellData;

namespace Assets.Scripts
{
    public static class AntHousesExtensions
    {
        public static T FindFreeHouse<T>(this IEnumerable<T> houses) where T : AntHouse
        {
            return houses.Where(house => house.Cell.CellState == CellState.Opened).FirstOrDefault();
        }
    }
}
