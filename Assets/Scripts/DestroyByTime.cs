using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    // Start is called before the first frame update
    public float destroytime;
    void Start()
    {
        Destroy(gameObject,destroytime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
