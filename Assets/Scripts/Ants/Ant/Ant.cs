using System;
using UnityEngine;
using DG.Tweening;
using PathCreation;
using Assets.Scripts;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public abstract class Ant : MonoBehaviour, IAnt
{
    private const float Delay = 2f;

    [SerializeField] private AntView _view;
    [SerializeField] private AntTrigger _antTrigger;
    [SerializeField] private Stats _stats = new Stats();

    private Shop _shop;
    private AntHouse _antHouse;
    private Coroutine _move;
    private Resource _piece;
    private float _pathDistance;
    private float _traveledDistance;
    private float _speed => _stats.Speed.Value;
    private float _strenght => _stats.Strengh.Value;
    private float _income => _stats.Income.Value;

    [SerializeField] protected Transform TransferPoint;

    public int Index { get; private set; }
    public AntState CurrentState { get; private set; }
    public Cell HouseCell { get; private set; }
    public AntHouse House => _antHouse;
    public Cell Target { get; private set; }
    public bool HasPart => _piece != null;
    public float DistanceToTarget => _pathDistance - _traveledDistance;
    public abstract AntType Type { get; }
    protected AntView View => _view;
    public Resource TargetPart { get; protected set; }
    protected WalletPresenter Wallet { get; private set; }

    private void OnEnable()
    {
        _antTrigger.Initialize(this);
    }

    private void OnDisable()
    {
        foreach (Product product in _shop.Products)
        {
            product.Buyed -= _stats.Upgrade;
            product.Boosted += _stats.Upgrade;
        }

        _view.Stop();
    }

    public void Initialize(Cell house, WalletPresenter wallet, AntHouse antHouse, Shop shop, int index)
    {
        HouseCell = house;
        _antHouse = antHouse;
        _shop = shop;
        Wallet = wallet;
        Index = index;

        foreach (Product product in _shop.Products)
        {
            _stats.Upgrade(product);
            product.Buyed += _stats.Upgrade;
            product.Boosted += _stats.Upgrade;
        }
    }

    public void Die()
    {
        if(_move != null)
            StopCoroutine(_move);

        _view.OnDie();

        //if (this is Loader)
        //{
        //    if (HasPart)
        //    {
        //        (_piece as FoodPart).SwitchToWaitingRegrow(Target.Food.transform);
        //        _piece = null;
        //    }
        //}

        //if (this is Digger)
        //{
        //    if (TargetPart != null && Target !=null)
        //        Target.SlicedHex.ReturnPart(TargetPart, HasPart);
        //}

        if (Target != null)
            RemoveAnt();

        CurrentState = AntState.WaitingWork;
        _antHouse.RemoveAnt(this);
        Target = null;
        Destroy(gameObject, Delay);
    }

    public void SetTarget(Cell target)
    {
        Target = target;
        CurrentState = AntState.WaitingMove;
    }

    public void SetTargetPiece(Resource part)
    {
        TargetPart = part;
    }

    public void StartMove(VertexPath path)
    {
        _view.gameObject.SetActive(true);
        _pathDistance = path.Distance;
        _move = StartCoroutine(Move(path));
    }

    public void AddToHome()
    {
        transform.position = HouseCell.transform.position + Vector3.down * 0.2f;
        CurrentState = AntState.WaitingWork;
    }

    protected void TakePiece(Resource takablePart) => _piece = takablePart;

    protected abstract IEnumerator Work(Cell cell, float speed, float strenght);
    protected virtual bool CanWork(Cell target) => TargetPart != null;
    protected virtual void OnEndMove(Resource spawnedPiece, float income) { }

    protected bool IsTargetReached()
    {
        float offset = 0.25f;
        //if (Target.SlicedHex.IsInfinite)
        //    offset = 0.25f;

        //bool isClose = Vector3.Distance(transform.position,
        //    new Vector3(TargetPart.Root.position.x, transform.position.y, TargetPart.Root.position.z)) <= offset;
        //Debug.Log($"Target: {Target != null} Target part: {TargetPart != null}, Distance {isClose}");

        return Target != null && TargetPart != null && Vector3.Distance(transform.position,
            new Vector3(TargetPart.Root.position.x, transform.position.y, TargetPart.Root.position.z)) <= offset;
    }

    private void RemoveAnt()
    {
        if (Type == AntType.Digger)
            Target.CellAntsHolder.RemoveDigger((Digger)this);
        else
            Target.CellAntsHolder.RemoveLoader((Loader)this);
    }

    private IEnumerator Move(VertexPath path)
    {
        _traveledDistance = 0;
        CurrentState = AntState.Moving;

        while (_traveledDistance <= path.Distance)
        {
            transform.position = path.GetPointAtDistance(_traveledDistance, EndOfPathInstruction.Stop);
            Vector3 direction = path.GetDirectionAtDistance(_traveledDistance, EndOfPathInstruction.Stop);
            transform.LookAt(transform.position + direction);
            _traveledDistance += (_antTrigger.LastTriggeredAnt == null ? _speed : _speed  * 0.3f) * Time.deltaTime;

            if (IsTargetReached() && _piece == null)
            {
                if (CanWork(Target))
                {
                    CurrentState = AntState.Working;
                    yield return Work(Target, _speed , _strenght);
                    CurrentState = AntState.Moving;
                }

                RemoveAnt();
            }

            yield return null;
        }


        if(transform!=null)
            transform.DOJump(HouseCell.transform.position + Vector3.down * 0.2f, 0.75f, 1, 0.5f);

        yield return new WaitForSeconds(0.5f);

        CurrentState = AntState.WaitingWork;
        OnEndMove(_piece, _income);
        Target = null;
        _piece = null;
    }
}
