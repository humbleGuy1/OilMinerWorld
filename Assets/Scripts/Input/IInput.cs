using System;
using UnityEngine;

public interface IInput
{
    event Action<float> Zooming;
    event Action<Vector2> StartedClick;
    event Action<Vector2> Moving;
    event Action<Vector2> EndedClick;

    void Initialize(Camera camera);
}
