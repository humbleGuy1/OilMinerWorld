using UnityEngine;
using DG.Tweening;
using System;
using System.Runtime.InteropServices;
using Random = UnityEngine.Random;

public class AddingResourceAnimation : MonoBehaviour
{
    [SerializeField] private AddedResourceFX _plusTemplate;
    [SerializeField] private Sprite _sprite;

    float _startHeight = 0.75f;
    float _startOffset = -0.2f;

    public void RenderAddingResource(float value)
    {
        Vector3 position = new Vector3(transform.position.x, _startHeight, transform.position.z);

        AddedResourceFX addedResourceFX = Instantiate(_plusTemplate, position, _plusTemplate.transform.rotation);
        addedResourceFX.Init(value, _sprite);
    }
}
