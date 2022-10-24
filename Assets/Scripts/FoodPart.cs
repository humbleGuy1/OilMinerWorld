using UnityEngine;

public class FoodPart : Resource
{
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private Vector3 _startScale;

    public bool IsWaitingRegrow { get; private set; }

    private void OnEnable()
    {
        Price = 2f;
    }

    private void Awake()
    {
        _startPosition = transform.position;
        _startRotation = transform.rotation;
        _startScale = transform.localScale;
    }

    public void SwitchToWaitingRegrow(Transform foodTransform)
    {
        transform.SetParent(foodTransform);

        transform.position = _startPosition;
        transform.rotation = _startRotation;
        transform.localScale = _startScale;

        gameObject.SetActive(false);

        IsWaitingRegrow = true;
    }

    public void SetWaitingRegrowState(bool state)
    {
        IsWaitingRegrow = state;
    }
}
