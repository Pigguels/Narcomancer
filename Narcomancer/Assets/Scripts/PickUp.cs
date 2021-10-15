using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public float bounceSpeed;
    public float bounceAmplitude;
    public float rotationSpeed;
    private float timeOffset;
    public GameObject pickUpPrefab;
    public PickupType pickupType;
    public float pickupDistance = 5f;

    float startingHeight;
    // Start is called before the first frame update
    void Start()
    {
        startingHeight = transform.localPosition.y;
        timeOffset = Random.value * Mathf.PI * 2;
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, this.transform.position) <= pickupDistance)
        {
            
            Debug.Log("Inrange of Pickup");
        }*/


        float finalheight = startingHeight + Mathf.Sin(Time.time * bounceSpeed + timeOffset) * bounceAmplitude;
        var pos = transform.localPosition;
        pos.y = finalheight;
        transform.localPosition = pos;

        Vector3 rot = transform.localRotation.eulerAngles;
        rot.y += rotationSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(rot.x, rot.y, rot.z);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (pickupType == PickupType.ammo)
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().AddShotgunAmmo(5);
            if (pickupType == PickupType.neon)
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().AddNeonAmmo(5);
            if (pickupType == PickupType.health)
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().m_Health.Heal(15);

            Destroy(gameObject, .2f);
        }
    }

}
public enum PickupType
{
    health,ammo,neon
}
