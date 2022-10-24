using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider _volumeSlider;

    private const string VolumeSaveWord = "volume";

    private void Start()
    {
        if(PlayerPrefs.HasKey(VolumeSaveWord) == false)
        {
            PlayerPrefs.SetFloat(VolumeSaveWord, 1f);
            Load();
        }
        else
        {
            Load();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = _volumeSlider.value;
        Save();
    }

    private void Load()
    {
        _volumeSlider.value = PlayerPrefs.GetFloat(VolumeSaveWord);
    }

    private void Save()
    {
        PlayerPrefs.SetFloat(VolumeSaveWord, _volumeSlider.value);
    }
}
