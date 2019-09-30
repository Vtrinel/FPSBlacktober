using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;

public class FieldOfView : MonoBehaviour
{

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [ReadOnly] public List<Transform> visibleTargets = new List<Transform>();

    [SerializeField] Vector3 m_targetLocation;
    [SerializeField] float m_lastVisualTime;

    [ReadOnly, SerializeField] bool hasTargetOnSight;
    [ReadOnly, SerializeField] bool hasAlreadySawTarget;

    public bool HasTargetOnSight { get => hasTargetOnSight; private set => hasTargetOnSight = value; }
    public Vector3 TargetLocation { get => m_targetLocation; private set => m_targetLocation = value; }
    public float LastVisualTime { get => m_lastVisualTime; private set => m_lastVisualTime = value; }
    public bool HasAlreadySawTarget { get => hasAlreadySawTarget; private set => hasAlreadySawTarget = value; }

    public event Action OnTargetFound = delegate { };
    public event Action OnTargetLost = delegate { };

    void Start()
    {
       // StartCoroutine("FindTargetsWithDelay", .2f);
    }

    private void Update()
    {
        FindVisibleTargets();
    }


    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2) //if the angle is right
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                //check if there's some obtacle
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    //Here the target is visible
                    visibleTargets.Add(target);
                    TargetLocation = target.position;
                    LastVisualTime = Time.time;

                    if (!HasTargetOnSight)
                    {
                        HasAlreadySawTarget = true;
                        OnTargetFound();
                    }

                    Debug.Log("I SEE TARGET");
                    HasTargetOnSight = true;
                }
                else
                {
                    //here the target is behind an obstacle
                }
            }
        }

        if (visibleTargets.Count == 0)
        {
            if (HasTargetOnSight)
            {
                OnTargetLost();
            }
            HasTargetOnSight = false;
        }
    }


    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}