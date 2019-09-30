using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Health
{
    [SerializeField] float m_lifePoint = 1;

    public float LifePoint { get => m_lifePoint; private set => m_lifePoint = value; }
    public event Action OnDeath = delegate { };

    public void AddLifePoint(float p_pointToAdd)
    {
        LifePoint += p_pointToAdd;
    }

    public void RemoveLifePoint(float p_pointToRemove)
    {
        LifePoint -= p_pointToRemove;

        if(LifePoint <= 0)
        {
            OnDeath();
        }
    }
}
