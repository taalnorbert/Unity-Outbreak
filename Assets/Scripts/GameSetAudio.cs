using UnityEngine;

public class GameSetAudio : MonoBehaviour
{
    public AudioSource pistolReload;
    public AudioSource pistolShoot;
    public AudioSource music;

    void Start()
    {
        pistolReload.volume = Settings.gameVolume / 100f;
        pistolShoot.volume = Settings.gameVolume / 100f;
        music.volume = Settings.gameMusic / 100f;
    }
}
