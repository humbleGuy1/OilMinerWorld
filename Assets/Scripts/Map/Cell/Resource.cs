using Assets.Scripts;
using UnityEngine;

public abstract class Resource : MonoBehaviour
{
    public Transform Root => transform;
    private float _initialPrice = 6f;

    public float Price { get; protected set; }

    public void SetMultiply(float pieceMultiply, CellData.CellDifficult  difficulty)
    {
        //CellData.PieceMultiplierByDifficulty.TryGetValue(difficulty, out int dificultyMultiplier);
        Price = _initialPrice * (int)difficulty;
        Price += _initialPrice + _initialPrice * pieceMultiply;
    }

    public void SetInfinitePrice()
    {
        Price = _initialPrice / 3f;
    }

    public void Disable()
    {
        Destroy(gameObject);
    }
}
