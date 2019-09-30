using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootInfo
{
    private Vector3 m_shootPosition;
    private float m_shootTime;

    public PlayerShootInfo(Vector3 p_shootPos,float p_shootTime)
    {
        ShootPosition = p_shootPos;
        ShootTime = p_shootTime;
    }

    public Vector3 ShootPosition { get => m_shootPosition; private set => m_shootPosition = value; }
    public float ShootTime { get => m_shootTime; private set => m_shootTime = value; }
}
