using Assets.Scripts;
using PathCreation;

public interface IAnt
{
    AntType Type { get; }
    AntState CurrentState { get; }
    Cell Target { get; }
    float DistanceToTarget { get; }
    bool HasPart { get; }

    void Initialize(Cell house, WalletPresenter wallet, AntHouse antHouse, Shop shop, int index);
    void SetTarget(Cell target);
    void StartMove(VertexPath path);
}
