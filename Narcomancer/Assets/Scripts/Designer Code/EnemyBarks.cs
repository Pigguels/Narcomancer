using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBarks : MonoBehaviour
{
    private float timer;
    public GameObject barkEmitter;
    public float min;
    public float max;
    // Start is called before the first frame update
    void Start()
    {
        timer = Random.Range(min, max);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0f)
        {
            barkEmitter.SetActive(false);
            barkEmitter.SetActive(true);
            timer = Random.Range(min, max);
        }
    }
}
