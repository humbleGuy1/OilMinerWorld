using System.Collections;
using Assets.Scripts;
using Cinemachine;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Region _region;
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private InputRoot _inputRoot;

    private UpgradeMenu _upgradeMenu;

    private void OnEnable()
    {
        _region.StarsCollected += Move;
    }

    private void Start()
    {
        _upgradeMenu = FindObjectOfType<UpgradeMenu>();
    }

    private void Move()
    {
        StartCoroutine(Moving());
        _region.StarsCollected -= Move;
    }

    private IEnumerator Moving()
    {
        yield return new WaitUntil(() => _upgradeMenu.HasOpened == false);
        yield return new WaitForSeconds(0.4f);

        _inputRoot.Disable();

        Vector3 targetPosition = new Vector3(_targetPoint.transform.position.x, _camera.transform.position.y, _targetPoint.transform.position.z - 5f);

        while (Vector3.Distance(_camera.transform.position, targetPosition) > 0.001f)
        {
            _camera.transform.position = Vector3.MoveTowards(_camera.transform.position, targetPosition, _moveSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        _inputRoot.Enable();
    }
}
