using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUIAmmo : GUIElement
{
    Player player;
    [SerializeField] TextMeshProUGUI m_currentAmmoText;
    [SerializeField] TextMeshProUGUI m_stockAmmoText;
    [SerializeField] Image m_ammoCircle;

    public override void OnEnable()
    {
        base.OnEnable();
        player = Player.instance;
    }

    public override void Update()
    {
        base.Update();
        m_ammoCircle.fillAmount = (float)player.PlayerGun.CurrentAmmo / (float)player.PlayerGun.Ammo;
        m_currentAmmoText.text = player.PlayerGun.CurrentAmmo.ToString();
        m_stockAmmoText.text = player.PlayerGun.AmmoStock.ToString();
    }
}
