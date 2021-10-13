using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lineRenderer;

    [SerializeField]
    private Texture[] textures;

    private int animationStep;

    [SerializeField]
    private float fps = 30f;

    private float fpscounter;


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

    }

    void Start()
    {
        
    }

    private void Update()
    {
        fpscounter += Time.deltaTime;
        if (fpscounter >= 1f / fps)
        {
            animationStep++;
            if (animationStep == textures.Length)
                animationStep = 0;



            lineRenderer.material.SetTexture("_MainTex", textures[animationStep]);

            fpscounter = 0f;
                
                
                }
    }
}
