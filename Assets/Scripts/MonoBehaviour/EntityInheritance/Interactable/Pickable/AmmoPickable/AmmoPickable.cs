using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickable : Pickable
{
    [Header("Ammo pick up settings")]
    [SerializeField] int m_ammo;

    public override void PickUpObject()
    {
        _gameManager.CallOnPlayerGetAmmo(m_ammo);
        base.PickUpObject();
    }
}
