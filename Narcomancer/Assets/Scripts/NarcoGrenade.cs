using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarcoGrenade : MonoBehaviour
{
    public Rigidbody Grenade;
    public Transform Target;

    public float h = 10;

    float localGravity = -10;
    public float timer = 0;
    public float launchtime = 3;
    public bool firing = false;
    // Start is called before the first frame update
    void Start()
    {
        Grenade.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= launchtime)
        {
            Launch();
            timer = 0;
        }
        timer += Time.deltaTime;
    }


    void Launch()
    {

        Grenade.useGravity = true;
        Grenade.transform.position = this.transform.position;
        Grenade.velocity = Vector3.zero;
        Debug.Log(CalculateLaunchVelocity());
        Grenade.velocity = CalculateLaunchVelocity();

    }
   
    //h = peak height
    Vector3 CalculateLaunchVelocity()
    {

        float displacementY = Target.position.y - transform.position.y;
        Vector3 displacementXZ = new Vector3(Target.position.x - transform.position.x, 0, Target.position.z - transform.position.z);


        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * localGravity * h);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * h / localGravity) + Mathf.Sqrt(2 * (displacementY - h) / localGravity));


        return velocityXZ + velocityY;
    }


    private void OnCollisionEnter(Collision collision)
    {
       //play vfx 
    }


}
