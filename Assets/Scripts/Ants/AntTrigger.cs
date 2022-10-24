using UnityEngine;

public class AntTrigger : MonoBehaviour
{
    [SerializeField] private bool _enabled = true;

    private IAnt _ant;

    public IAnt LastTriggeredAnt { get; private set; }

    public void Initialize(IAnt ant)
    {
        _ant = ant;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_enabled == false)
            return;

        if (_ant.CurrentState == AntState.WaitingWork || _ant.CurrentState == AntState.Working)
            return;

        if (LastTriggeredAnt != null)
        {
            if (LastTriggeredAnt.CurrentState != AntState.Moving)
                LastTriggeredAnt = null;
            else
                return;
        }

        if (other.TryGetComponent(out IAnt ant))
        {
            bool collidedWithAnt = ant.Type == _ant.Type && ant.Target == _ant.Target && ant.CurrentState == AntState.Moving
                && ant.HasPart == _ant.HasPart && ant.DistanceToTarget < _ant.DistanceToTarget;

            if (collidedWithAnt)
                LastTriggeredAnt = ant;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_enabled == false)
            return;

        if (LastTriggeredAnt == null)
            return;

        if (other.TryGetComponent(out IAnt ant))
            if (LastTriggeredAnt == ant)
                LastTriggeredAnt = null;
    }
}
