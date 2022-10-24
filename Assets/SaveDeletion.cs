using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDeletion
{
    private const string saveWord = "DeleteAllSave";
    private int _index = 1;

    public void Delete()
    {
        if (PlayerPrefs.GetInt(saveWord) == _index)
            return;

        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt(saveWord, _index);
    }

}
