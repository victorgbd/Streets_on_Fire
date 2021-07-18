using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider healthslider;
    public Image playerimage;
    public Text playername;
    public Text livestext;

    public GameObject enemyui;
    public Slider enemyslider;
    public Text enemyname;
    public Image enemyimage;

    public float enemyTimeUI = 4f;
    private float enemyTimer;
    private Player _player;
    void Start()
    {
        _player = FindObjectOfType<Player>();
        healthslider.maxValue = _player.maxhealth;
        healthslider.value = healthslider.maxValue;
        playername.text = _player.playername;
        playerimage.sprite = _player.playerimage;
        Updatelives();
    }

    // Update is called once per frame
    void Update()
    {
        enemyTimer += Time.deltaTime;
        if (enemyTimer >= enemyTimeUI)
        {
            enemyui.SetActive(false);
            enemyTimer = 0;
        }
    }

    public void UpdateHealth(int amount)
    {
        healthslider.value = amount;
    }

    public void UpdateEnemyUI(int maxhealth, int currentHealth, string name, Sprite image)
    {
        enemyslider.maxValue = maxhealth;
        enemyslider.value = currentHealth;
        enemyname.text = name;
        enemyimage.sprite = image;
        enemyTimer = 0;
        enemyui.SetActive(true);
    }

    public void Updatelives()
    {
        livestext.text = "x" + FindObjectOfType<GameManger>().numvidas.ToString();
    }
}
