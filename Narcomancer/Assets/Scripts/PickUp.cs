using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public float bounceSpeed;
    public float bounceAmplitude;
    public float rotationSpeed;
    private float timeOffset;

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
        float finalheight = startingHeight + Mathf.Sin(Time.time * bounceSpeed + timeOffset) * bounceAmplitude;
        var pos = transform.localPosition;
        pos.y = finalheight;
        transform.localPosition = pos;

        Vector3 rot = transform.localRotation.eulerAngles;
        rot.y += rotationSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(rot.x, rot.y, rot.z);

    }
}
