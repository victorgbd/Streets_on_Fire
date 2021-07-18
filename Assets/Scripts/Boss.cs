using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Boss : Enemy
{
    // Start is called before the first frame update
    public GameObject boomerang;
    public float mintimeboomerang,maxtimeboomerang;
    
    private MusicController music;
    void Awake()
    {
        Invoke("ThrowBoomerang",Random.Range(mintimeboomerang,maxtimeboomerang));
        music = FindObjectOfType<MusicController>();
        music.PlaySong(music.bossSong);
    }

    void ThrowBoomerang()
    {
        if (!isDead)
        {
            _anim.SetTrigger("Boomerang");
            GameObject temp = Instantiate(boomerang, transform.position, transform.rotation);
            if (facingRight)
            {
                temp.GetComponent<Boomerang>().direction = 1;
            }
            else
            {
                temp.GetComponent<Boomerang>().direction = -1;
            }
            Invoke("ThrowBoomerang",Random.Range(mintimeboomerang,maxtimeboomerang));
        }
    }

    void BossDefeated()
    {
        music.PlaySong(music.levelClearSong);
        Invoke("LoadScene",8f);
    }

    void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
}
