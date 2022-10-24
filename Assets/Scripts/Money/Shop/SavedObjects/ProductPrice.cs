using UnityEngine;

public class ProductPrice : SavedObject<ProductPrice>
{
    private const float ProgressionDenominator = 1.4f;
    private const int StartValue = 500;

    [field: SerializeField] public int Value { get; private set; } = StartValue;

    public ProductPrice(string guid) : base(guid) { }

    public void Next(int stepsCounter)
    {
        int value = Mathf.FloorToInt(StartValue * Mathf.Pow(ProgressionDenominator, stepsCounter));
        value /= 100;
        value *= 100;
        Value = value;        
    }

    protected override void OnLoad(ProductPrice loadedObject)
    {
        Value = loadedObject.Value;
    }
}
