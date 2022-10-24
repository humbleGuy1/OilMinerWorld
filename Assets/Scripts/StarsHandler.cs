using UnityEngine;

public class StarsHandler : MonoBehaviour
{
    [SerializeField] protected Star[] _stars;

    private void Awake()
    {
        //foreach (var star in _stars)
        //{
        //    star.Disable();
        //}
    }

    public void OnUpgrade(int level)
    {
        level--;

        for (int i = 0; i < level; i++)
        {
            if (i >= _stars.Length)
                break;

            _stars[i].Enable();
        }
    }
}
