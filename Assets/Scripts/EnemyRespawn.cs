using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random =  UnityEngine.Random;

public class EnemyRespawn : MonoBehaviour
{
    public float Minz, Maxz;

    public GameObject[] enemies;

    public int numberofenemy;

    public float spawnTime;

    private int currentenemies;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentenemies >= numberofenemy)
        {
            int enemieslength = FindObjectsOfType<Enemy>().Length;
            if (enemieslength <= 0)
            {
                FindObjectOfType<CameraFollow>().maxXAndY.x = 200;
                gameObject.SetActive(false);
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<BoxCollider>().enabled = false;
            FindObjectOfType<CameraFollow>().maxXAndY.x = transform.position.x;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        bool positionX = Random.Range(0, 2) == 0 ? true : false;
        Vector3 spawnPosition;
        spawnPosition.z = Random.Range(Minz,Maxz);
        if (positionX)
        {
            spawnPosition = new Vector3(transform.position.x + 10, 0, spawnPosition.z);
        }
        else
        {
            spawnPosition = new Vector3(transform.position.x - 10, 0, spawnPosition.z);
        }
        Instantiate(enemies[Random.Range(0, enemies.Length)], spawnPosition, Quaternion.identity);
        currentenemies++;
        if (currentenemies < numberofenemy)
        {
            Invoke("SpawnEnemy",spawnTime);
        }
    }
}
