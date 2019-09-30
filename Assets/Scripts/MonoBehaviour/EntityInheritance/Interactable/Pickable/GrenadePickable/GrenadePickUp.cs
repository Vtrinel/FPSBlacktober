using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadePickUp : Pickable
{
    [Header("Grenade pick up settings")]
    [SerializeField] int m_grenade;

    public override void PickUpObject()
    {
        _gameManager.CallOnPlayerGetGrenade(m_grenade);
        Debug.Log("PICK UP", this);
        base.PickUpObject();
    }
}
