using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingLine : MonoBehaviour
{

    LineRenderer m_lineRend;
    public float animationDuration = 4.0f;

    private GameObject m_target;


    // Start is called before the first frame update
    void Start()
    {
        m_lineRend = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        LineAnimate(transform, m_target.transform);
    }


    private IEnumerator LineAnimate(Transform enemyTransform, Transform playerTransform)
    {
        float startTime = Time.time;

        Vector3 startPos = enemyTransform.position;
        Vector3 endPos = playerTransform.position;

        m_lineRend.SetPosition(0, endPos);
        m_lineRend.SetPosition(1, startPos);

        Vector3 pos = startPos;
        while (pos != endPos)
        {
            float t = (Time.time - startTime) / animationDuration;
            pos = Vector3.Lerp(startPos, endPos, t);
            m_lineRend.SetPosition(1, pos);
            yield return null;
        }

    }
}
