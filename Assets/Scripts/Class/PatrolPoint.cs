using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PatrolPoint
{
    [SerializeField] Transform m_pointTranform;

    public Vector3 PatrolPointPosition { get => m_pointTranform.position;}

    public PatrolPoint(Transform p_pointTransform)
    {
        m_pointTranform = p_pointTransform;
    }
}
