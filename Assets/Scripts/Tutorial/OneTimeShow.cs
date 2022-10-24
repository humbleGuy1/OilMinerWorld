using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeShow : MonoBehaviour
{
    [SerializeField] private int _id;

    private const string SaveWord = "OneTimeShow";

    public bool WasShown()
    {
        return PlayerPrefs.GetInt(SaveWord) == _id;
    }

    public void Show()
    {
        if (WasShown())
        {
            gameObject.SetActive(false);

            return;
        }

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        PlayerPrefs.SetInt(SaveWord, _id);
    }
}
