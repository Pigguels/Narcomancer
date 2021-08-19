using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Grunt_Pistol : MonoBehaviour
{
   

    NavMeshAgent m_navAgent;
    //CharacterController m_charCont;  //this character controller could be used for manual ai movement if needed
    
    //the enemy Firerate of guns
    public float m_fireRate = 0.5f;

    public Transform spawnpoint;
    public GameObject bulletPrefab;
    private float m_distance;
    public GameObject m_target;
    public float moveSpeed = 15;
    private bool hasFired;

    // Start is called before the first frame update
    void Start()
    {
        m_navAgent = GetComponent<NavMeshAgent>();

        m_navAgent.updatePosition = true;
    }

    // Update is called once per frame
    void Update()
    {

        m_distance = Vector3.Distance(m_target.transform.position, transform.position);
        transform.LookAt(m_target.transform, Vector3.up);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        if (m_distance > 4)
        {
            m_navAgent.updatePosition = true;
            m_navAgent.SetDestination(m_target.transform.position);

        }
        if (m_distance <= 4)
        {
            m_navAgent.updatePosition = false;
            if (!hasFired)
                StartCoroutine(Attack());

        }

    }
    private IEnumerator Attack()
    {
        hasFired = true;
        GameObject bullet;
        bullet = Instantiate(bulletPrefab, spawnpoint.position, Quaternion.identity);
        bullet.gameObject.GetComponent<Rigidbody>().AddForce(spawnpoint.forward * 1000f);
        Destroy(bullet, 1.5f);
        yield return new WaitForSeconds(m_fireRate);
        hasFired = false;
    }
}
