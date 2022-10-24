using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _soundOnSprite;
    [SerializeField] private Sprite _soundOffSprite;

    private bool _isSoundEnabled = true;

    private const string Sound = "Sound";

    private void Start()
    {
        if (PlayerPrefs.GetInt(Sound) == 0)
        {
            EnableSound();
            return;
        }

        if (PlayerPrefs.GetInt(Sound) == 1)
        {
            DisableSound();
        }
        else
        {
            EnableSound();
        }
    }

    public void OnButtonClick()
    {
        if (_isSoundEnabled)
        {
            DisableSound();
            PlayerPrefs.SetInt(Sound, GetSoundState());

            return;
        }

        EnableSound();
        PlayerPrefs.SetInt(Sound, GetSoundState());
    }

    private void DisableSound()
    {
        AudioListener.volume = 0;
        _isSoundEnabled = false;
        _image.sprite = _soundOffSprite;
    }

    private void EnableSound()
    {
        AudioListener.volume = 1;
        _isSoundEnabled = true;
        _image.sprite = _soundOnSprite;
    }

    private int GetSoundState()
    {
        return _isSoundEnabled ? 2 : 1;
    }
}
