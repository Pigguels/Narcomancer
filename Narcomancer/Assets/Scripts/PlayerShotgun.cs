using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShotgun : MonoBehaviour
{
    #region variables

    public float pelletRange = 20f;
    public float pelletDamage = 25f;
    [Space]
    public float timeBetweenShots = 0.35f;
    private float timeSinceLastShot = 0f;
    [Space]
    public int innerSpreadPelletAmount = 6;
    public int outerSpreadPelletAmount = 10;
    [Space]
    [Range(0, 180)]
    public float innerSpreadWidthAngle = 12f;
    [Range(0, 180)]
    public float innerSpreadHeightAngle = 12f;
    [Range(0, 180)]
    public float outerSpreadWidthAngle = 35f;
    [Range(0, 180)]
    public float outerSpreadHeightAngle = 35f;
    public bool drawPelletSpreads = false;

    [Space]
    [Header("References")]
    public Transform playerCamera;

    private PlayerController m_PlayerController;

    private LayerMask playerLayer;



    //Baz Junk
    [Header("On Hit Effects")]
    public GameObject bloodOnHit;
    public GameObject debrisOnHit;

    [Header("UI Animations")]
    public Animator ammoImage;
    bool hasLoaded;

    #endregion

    void Start()
    {
        m_PlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        playerLayer = LayerMask.GetMask("Player");
    }

    void Update()
    {
        if (timeSinceLastShot < timeBetweenShots)
            timeSinceLastShot += Time.deltaTime;

        if ((timeSinceLastShot > timeBetweenShots) && m_PlayerController.m_CurrentShotgunAmmo != 0 && !hasLoaded)
        {
            ammoImage.SetTrigger("UIAmmoLoad");
            hasLoaded = true;
        }
    }

    void Shoot()
    {
        ammoImage.SetTrigger("UIAmmoShot");
        hasLoaded = false;
        // a list of hit objects to apply damage to at the end
        List<RaycastHit> hits = new List<RaycastHit>();

        //make sure the outer spread angle is never smaller than the inn spread angle
        if (innerSpreadWidthAngle > outerSpreadWidthAngle)
            outerSpreadWidthAngle = innerSpreadWidthAngle;
        if (innerSpreadHeightAngle > outerSpreadHeightAngle)
            outerSpreadHeightAngle = innerSpreadHeightAngle;

        // cache the spread angles - i have no idea why height needs to be in x and width needs to be in y, what
        Vector2 innerSpreadAngle = new Vector2(innerSpreadHeightAngle, innerSpreadWidthAngle) * 0.5f;
        Vector2 outerSpreadAngle = new Vector2(outerSpreadHeightAngle, outerSpreadWidthAngle) * 0.5f;

        // do the inner spread of pellets
        for (int i = 0; i < innerSpreadPelletAmount; i++)
        {
            // creates a random direction based on the innerSpreadAngle
            Vector2 spreadAngle = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * new Vector2(Random.Range(-innerSpreadAngle.x, innerSpreadAngle.x), Random.Range(-innerSpreadAngle.y, innerSpreadAngle.y));
            Vector3 newRayDir = Quaternion.Euler(spreadAngle) * playerCamera.forward;

            // do the raycast for the pellet
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.position, newRayDir, out hit, pelletRange, ~playerLayer))
            {
                // collect the hit object
                hits.Add(hit);
            }
        }

        // do the outer spread of pellets
        for (int i = 0; i < outerSpreadPelletAmount; i++)
        {
            // creates a random direction based on the outerSpreadAngle
            Vector2 rotDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            Vector2 spreadAngle = (rotDir * (innerSpreadAngle + (outerSpreadAngle - innerSpreadAngle))) + (rotDir * new Vector2(Random.Range(-(outerSpreadAngle.x - innerSpreadAngle.x), outerSpreadAngle.x - innerSpreadAngle.x), Random.Range(-(outerSpreadAngle.y - innerSpreadAngle.y), outerSpreadAngle.y - innerSpreadAngle.y)));
            Vector3 newRayDir = Quaternion.Euler(spreadAngle) * playerCamera.forward;

            // do the raycast for the pellet
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.position, newRayDir, out hit, pelletRange, ~playerLayer))
            {
                // collect the hit object
                hits.Add(hit);
            }
        }

        // apply the pellets damage to the hit objects
        for (int i = 0; i < hits.Count; i++)
        {
            if (hits[i].transform.CompareTag("Enemy"))
            {
                if (hits[i].transform.GetComponent<EnemyDamagePoint>())
                    hits[i].transform.GetComponent<EnemyDamagePoint>().Damage(pelletDamage);

                Instantiate(bloodOnHit, hits[i].point, gameObject.transform.rotation);

            }
            else if (hits[i].transform.CompareTag("Interactable"))
            {
                if (hits[i].transform.GetComponent<Health>())
                    hits[i].transform.GetComponent<Health>().Damage(pelletDamage);
                // NEED TO ADD GENERIC INTERACTABLE OBJECT EVENT CALLING HERE
            }
            else if (hits[i].transform.CompareTag("Untagged"))
            {
                Instantiate(debrisOnHit, hits[i].point, gameObject.transform.rotation);
            }
        }
    }

    public void OnPrimaryFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (timeSinceLastShot >= timeBetweenShots && m_PlayerController.m_CurrentShotgunAmmo > 0)
            {
                Shoot();
                --m_PlayerController.m_CurrentShotgunAmmo;
                timeSinceLastShot = 0f;
            }
        }
    }

    private void OnDrawGizmos()
    {
        // draw the pellets spreads
        if (drawPelletSpreads)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(playerCamera.position, playerCamera.position + ((Quaternion.Euler(0, innerSpreadWidthAngle, 0) * playerCamera.forward) * pelletRange));
            Gizmos.DrawLine(playerCamera.position, playerCamera.position + ((Quaternion.Euler(0, -innerSpreadWidthAngle, 0) * playerCamera.forward) * pelletRange));
            Gizmos.DrawLine(playerCamera.position, playerCamera.position + ((Quaternion.Euler(innerSpreadHeightAngle, 0, 0) * playerCamera.forward) * pelletRange));
            Gizmos.DrawLine(playerCamera.position, playerCamera.position + ((Quaternion.Euler(-innerSpreadHeightAngle, 0, 0) * playerCamera.forward) * pelletRange));
            Gizmos.DrawLine(playerCamera.position + ((Quaternion.Euler(0, innerSpreadWidthAngle, 0) * playerCamera.forward) * pelletRange), playerCamera.position + ((Quaternion.Euler(innerSpreadHeightAngle, 0, 0) * playerCamera.forward) * pelletRange));
            Gizmos.DrawLine(playerCamera.position + ((Quaternion.Euler(0, innerSpreadWidthAngle, 0) * playerCamera.forward) * pelletRange), playerCamera.position + ((Quaternion.Euler(-innerSpreadHeightAngle, 0, 0) * playerCamera.forward) * pelletRange));
            Gizmos.DrawLine(playerCamera.position + ((Quaternion.Euler(0, -innerSpreadWidthAngle, 0) * playerCamera.forward) * pelletRange), playerCamera.position + ((Quaternion.Euler(innerSpreadHeightAngle, 0, 0) * playerCamera.forward) * pelletRange));
            Gizmos.DrawLine(playerCamera.position + ((Quaternion.Euler(0, -innerSpreadWidthAngle, 0) * playerCamera.forward) * pelletRange), playerCamera.position + ((Quaternion.Euler(-innerSpreadHeightAngle, 0, 0) * playerCamera.forward) * pelletRange));

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(playerCamera.position, playerCamera.position + ((Quaternion.Euler(0, outerSpreadWidthAngle, 0) * playerCamera.forward) * pelletRange));
            Gizmos.DrawLine(playerCamera.position, playerCamera.position + ((Quaternion.Euler(0, -outerSpreadWidthAngle, 0) * playerCamera.forward) * pelletRange));
            Gizmos.DrawLine(playerCamera.position, playerCamera.position + ((Quaternion.Euler(outerSpreadHeightAngle, 0, 0) * playerCamera.forward) * pelletRange));
            Gizmos.DrawLine(playerCamera.position, playerCamera.position + ((Quaternion.Euler(-outerSpreadHeightAngle, 0, 0) * playerCamera.forward) * pelletRange));
            Gizmos.DrawLine(playerCamera.position + ((Quaternion.Euler(0, outerSpreadWidthAngle, 0) * playerCamera.forward) * pelletRange), playerCamera.position + ((Quaternion.Euler(outerSpreadHeightAngle, 0, 0) * playerCamera.forward) * pelletRange));
            Gizmos.DrawLine(playerCamera.position + ((Quaternion.Euler(0, outerSpreadWidthAngle, 0) * playerCamera.forward) * pelletRange), playerCamera.position + ((Quaternion.Euler(-outerSpreadHeightAngle, 0, 0) * playerCamera.forward) * pelletRange));
            Gizmos.DrawLine(playerCamera.position + ((Quaternion.Euler(0, -outerSpreadWidthAngle, 0) * playerCamera.forward) * pelletRange), playerCamera.position + ((Quaternion.Euler(outerSpreadHeightAngle, 0, 0) * playerCamera.forward) * pelletRange));
            Gizmos.DrawLine(playerCamera.position + ((Quaternion.Euler(0, -outerSpreadWidthAngle, 0) * playerCamera.forward) * pelletRange), playerCamera.position + ((Quaternion.Euler(-outerSpreadHeightAngle, 0, 0) * playerCamera.forward) * pelletRange));
        }
    }
}
