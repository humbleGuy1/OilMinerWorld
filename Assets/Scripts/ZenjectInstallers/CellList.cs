using System.Linq;
using Assets.Scripts;

public class CellList
{
    public CellList(Cell[] cells)
    {
        AllCells = cells;
        DefaultCells = cells.Where(cell => cell.CellType == CellData.CellType.Default).ToArray();
        EnemyCells = cells.Where(cell => cell.CellType == CellData.CellType.Enemy).ToArray();
        FoodCells = cells.Where(cell => cell.CellType == CellData.CellType.Food).ToArray();
    }

    public Cell[] AllCells { get; private set; }
    public Cell[] DefaultCells { get; private set; }
    public Cell[] EnemyCells { get; private set; }
    public Cell[] FoodCells { get; private set; }
}
