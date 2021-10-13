using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeWindow : MonoBehaviour
{
    public GameObject brokenWindow;
    public GameObject glassParticle;
    public GameObject NarcomancerGas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BreakTheWindow()
    {
        brokenWindow.SetActive(true);
        glassParticle.SetActive(true);
        NarcomancerGas.SetActive(true);
        gameObject.SetActive(false);

    }
}
