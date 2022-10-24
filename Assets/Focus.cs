using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Focus : MonoBehaviour
{
    [SerializeField] private InputRoot _inputRoot;

    private void OnApplicationFocus(bool focus)
    {
        _inputRoot.enabled = focus;
    }
}
