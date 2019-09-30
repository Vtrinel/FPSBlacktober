using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GUIGrenade : GUIElement
{
    Player player;
    [SerializeField] TextMeshProUGUI m_currentGrenade;


    public override void OnEnable()
    {
        base.OnEnable();
        player = Player.instance;
    }

    public override void Update()
    {
        base.Update();
        m_currentGrenade.text = player.PlayerGun.Grenade.ToString();
    }
}
