using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Cinemachine;
using TMPro;
using UnityEngine;

public class TutorialCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private float _moveSpeed;

    private InputRoot _inputRoot;


    public void Init(InputRoot inputRoot)
    {
        _inputRoot = inputRoot;
    }

    public void Move(Action OnMoveEnded)
    {
        StartCoroutine(Moving(OnMoveEnded));
    }

    public void Move()
    {
        StartCoroutine(Moving());
    }

    private IEnumerator Moving(Action OnMoveEnded = null)
    {
        _inputRoot.Disable();
        float elapsedTime = 0;
        float duration = 0.6f;
        Vector3 initialPosition = _camera.transform.position;
        Vector3 targetPosition = new Vector3(_targetPoint.transform.position.x, _targetPoint.transform.position.y, _targetPoint.transform.position.z);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            _camera.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            yield return null;
        }

        yield return new WaitForSeconds(6f);

        elapsedTime = 0;
        duration = 0.6f;

        while(elapsedTime< duration)
        {
            elapsedTime += Time.deltaTime;

            _camera.transform.position = Vector3.Lerp(targetPosition, initialPosition, elapsedTime/duration);

            yield return null;
        }

        OnMoveEnded?.Invoke();
        _inputRoot.Enable();
    }
}
