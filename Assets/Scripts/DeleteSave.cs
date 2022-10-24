using UnityEngine;

public class DeleteSave : MonoBehaviour
{
    private const string DeleteSaveKey = "DeleteSaveKey";

    private void Awake()
    {
        if (PlayerPrefs.HasKey(DeleteSaveKey))
            return;

        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt(DeleteSaveKey, 1);
    }
}
