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
    public float pickupDistance = 3f;
    public LayerMask playerLayer;

    Transform player;

    float startingHeight;
    // Start is called before the first frame update
    void Start()
    {
        startingHeight = transform.localPosition.y;
        timeOffset = Random.value * Mathf.PI * 2;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {

        float finalheight = startingHeight + Mathf.Sin(Time.time * bounceSpeed + timeOffset) * bounceAmplitude;
        var pos = transform.localPosition;
        pos.y = finalheight;
        transform.localPosition = pos;

        Vector3 rot = transform.localRotation.eulerAngles;
        rot.y += rotationSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(rot.x, rot.y, rot.z);

        Collider[] hit = (Physics.OverlapSphere(transform.position, pickupDistance));
        // Cast a sphere around the pickup to see if it is about to hit anything.
        foreach (var collider in hit)
        {
            if (collider.CompareTag("Player"))
            {
                if (pickupType == PickupType.ammo)
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().AddShotgunAmmo(5);
                if (pickupType == PickupType.neon)
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().AddNeonAmmo(5);
                if (pickupType == PickupType.health)
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().m_Health.Heal(15);

                transform.position = Vector3.Lerp(transform.position, player.position, .1f);
                Destroy(gameObject, .7f);
            }

        }

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        if (pickupType == PickupType.ammo)
    //            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().AddShotgunAmmo(5);
    //        if (pickupType == PickupType.neon)
    //            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().AddNeonAmmo(5);
    //        if (pickupType == PickupType.health)
    //            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().m_Health.Heal(15);
    //
    //        Destroy(gameObject, .2f);
    //    }
    //}

}
public enum PickupType
{
    health, ammo, neon
}
