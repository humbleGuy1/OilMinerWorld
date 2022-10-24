using UnityEngine;

public class ProductValue : SavedObject<ProductValue>
{
    private float _maxValue;

    [SerializeField] private float _value = 1;

    public ProductValue(string guid, float maxValue) : base(guid)
    {
        _maxValue = maxValue;
    }

    public bool CanNext => _value < _maxValue;
    public float Value => _value;

    public void Next(float add)
    {
        _value += add;
    }

    public void UpdateMaxValue(float maxValue)
    {
        _maxValue = maxValue;
        Save();
    }

    public void Boost(float boost) => _value *= boost;

    public void StopBoost(float boost) => _value /= boost;

    protected override void OnLoad(ProductValue loadedObject)
    {
        _value = loadedObject._value;
    }
}
