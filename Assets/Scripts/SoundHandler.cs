using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    [SerializeField] private AudioSource _background;
    [SerializeField] private AudioSource _upgrade;

    public static SoundHandler Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

       Destroy(gameObject);
    }

    public void PlayUpgradeSound()
    {
        if(_upgrade.isPlaying == false)
            _upgrade.Play();
    }

    private void RandomizePitch(AudioSource sound)
    {
        sound.pitch = Random.Range(0.8f, 1.2f);
    }
}
