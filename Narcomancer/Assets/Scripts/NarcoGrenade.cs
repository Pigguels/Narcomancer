using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarcoGrenade : MonoBehaviour
{
    public GameObject Grenade;
    public Transform Target;
    public Transform launchPosition;
    public float h = 10;
    public Animator anim;

    float localGravity = -10;
    public float timer = 0;
    public float firtimer = 0;
    public float launchtime = 10;
    public bool firing = false;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(new Vector3(Target.position.x - transform.position.x, 0f, Target.position.z - transform.position.z).normalized, Vector3.up);

        if (timer >= launchtime)
        {
            if (firtimer > 0)
            {
                anim.SetTrigger("Shoot");
                firtimer -= Time.deltaTime;
            }
            else
            {
                Launch();
                timer = 0;
                firtimer = 2.5f;
            }
        }
        else
        {

        timer += Time.deltaTime;
        }
    }


    void Launch()
    {
       

        GameObject clone = Instantiate(Grenade);
        clone.GetComponent<Rigidbody>().useGravity = true;
        clone.GetComponent<Rigidbody>().transform.position = launchPosition.position;
        clone.GetComponent<Rigidbody>().velocity = Vector3.zero;
        // Debug.Log(CalculateLaunchVelocity());
        clone.GetComponent<Rigidbody>().velocity = CalculateLaunchVelocity();

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





}
