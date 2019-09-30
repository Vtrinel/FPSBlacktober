using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character, IPlayer
{
    [Header("Player settings")]
    [SerializeField] AutomaticGunScriptLPFP m_playerGun;
    [SerializeField] FPSControllerLPFP.FpsControllerLPFP m_controller;

    public static Player instance;

    public AutomaticGunScriptLPFP PlayerGun { get => m_playerGun; private set => m_playerGun = value; }

    [SerializeField] Animator m_postProcessAnim;

    public override void Awake()
    {
        base.Awake();
        instance = this;
    }

    public override void TakeDamage(DamageInfo p_damageInfo)
    {
        base.TakeDamage(p_damageInfo);
        Debug.Log("Player Took damage");
        m_postProcessAnim.SetTrigger("damage");
    }

    public override void Death()
    {
        if (isDead) return;
        PlayerGun.enabled = false;
        PlayerGun.GameOver();
        m_controller.enabled = false;
        _gameManager.CallOnGameOver();
        base.Death();
    }

    public override void PlayerGetAmmo(int p_ammo)
    {
        base.PlayerGetAmmo(p_ammo);
        PlayerGun.AddAmmo(p_ammo);
    }

    public override void PlayerGetGrenade(int p_grenade)
    {
        base.PlayerGetGrenade(p_grenade);
        Debug.Log("Add grenade");
        PlayerGun.AddGrenade(p_grenade);
    }

    public override void PlayerGetHealth(int p_healthPoint)
    {
        base.PlayerGetHealth(p_healthPoint);
        _characterHealth.AddLifePoint(p_healthPoint);
    }
}
