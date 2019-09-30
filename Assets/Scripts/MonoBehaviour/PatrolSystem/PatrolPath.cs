using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PatrolPath : MonoBehaviour
{
    [SerializeField] List<PatrolPoint> m_patrolPoints;
    [SerializeField] Color pathColor = Color.blue;

    public List<PatrolPoint> PatrolPoints { get => m_patrolPoints; set => m_patrolPoints = value; }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < PatrolPoints.Count; i++)
        {
            Gizmos.color = pathColor;
            Gizmos.DrawLine(PatrolPoints[i].PatrolPointPosition, PatrolPoints[(i + 1) % PatrolPoints.Count].PatrolPointPosition);
            //Gizmos.color = Color.red;
            //Gizmos.DrawIcon(PatrolPoints[i].PatrolPointPosition, "patrolPoint_" + i);
        }
    }

    [Button]
    public void CreatePatrolPoint()
    {
        GameObject patrolPoint = new GameObject("patrolPoint_" + PatrolPoints.Count.ToString());
        patrolPoint.transform.parent = transform;
        patrolPoint.transform.localPosition = Vector3.zero;

        m_patrolPoints.Add(new PatrolPoint(patrolPoint.transform));
    }

    [Button]
    public void DeletePatrolPoint()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        m_patrolPoints = new List<PatrolPoint>();
    }
}
