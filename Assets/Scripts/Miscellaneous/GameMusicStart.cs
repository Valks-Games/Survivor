using UnityEngine;

public class GameMusicStart : MonoBehaviour
{
    public void Start()
    {
        GetComponent<AudioSource>().volume = Options.VolumeMusic;
    }
}
