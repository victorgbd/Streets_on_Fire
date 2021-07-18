using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip levelSong, bossSong, levelClearSong;
    private AudioSource audios;
    void Start()
    {
        audios = GetComponent<AudioSource>();
        PlaySong(levelSong);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySong(AudioClip audioClip)
    {
        audios.clip = audioClip;
        audios.Play();
    }
}
