using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Assets.Scripts
{
    public class CellData
    {
        public enum CellDifficult
        {
            Ligth,
            Medium,
            Hard
        }

        public enum CellType
        {
            Default = 0,
            Food = 1,
            Queen = 2,
            DiggersHouse = 3,
            LoaderHouse = 4,
            Enemy = 5
        }

        public enum CellState
        {
            Locked = 0,
            Unlocked = 1,
            Digging = 2,
            Opened = 3,
            Blocked = 4,
            DeadEnemy = 7,
            EatenEnemy = 8
        }

        private Dictionary<CellDifficult, int> _multiplierByDifficulty = new Dictionary<CellDifficult, int>
        {
            { CellDifficult.Ligth, 0 },
            { CellDifficult.Medium, 2 },
            { CellDifficult.Hard, 3 }
        };

        private Dictionary<CellDifficult, int> _pieceMultiplierByDifficulty = new Dictionary<CellDifficult, int>
        {
            { CellDifficult.Ligth, 0 },
            { CellDifficult.Medium, 1 },
            { CellDifficult.Hard, 2 }
        };

        public static IReadOnlyDictionary<CellDifficult, int> MultiplierByDifficulty => new CellData()._multiplierByDifficulty;
        public static IReadOnlyDictionary<CellDifficult, int> PieceMultiplierByDifficulty => new CellData()._pieceMultiplierByDifficulty;
    }
}
