using System.Linq;
using Assets.Scripts;

public class AntHouseList
{
    public AntHouseList(AntHouse[] antHouses)
    {
        AllHouses = antHouses;
        DiggersHouses = antHouses.Where(house => house is DiggersHouse).Cast<DiggersHouse>().ToArray();
        LoaderHouses = antHouses.Where(house => house is LoaderHouse).Cast<LoaderHouse>().ToArray();
    }

    public AntHouse[] AllHouses { get; private set; }
    public DiggersHouse[] DiggersHouses { get; private set; }
    public LoaderHouse[] LoaderHouses { get; private set; }
}
