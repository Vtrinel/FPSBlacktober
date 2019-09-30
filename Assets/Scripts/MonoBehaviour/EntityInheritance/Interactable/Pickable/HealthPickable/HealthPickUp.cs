using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : Pickable
{
    [Header("Health pick up settings")]
    [SerializeField] int m_healthPoints;

    public override void PickUpObject()
    {
        _gameManager.CallOnPlayerGetHealth(m_healthPoints);
        base.PickUpObject();
    }
}
