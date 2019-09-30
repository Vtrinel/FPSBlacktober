using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Interactable
{
    [Header("Character settings")]
    [SerializeField] protected Health _characterHealth;
    [SerializeField] public bool isDead;

    [Header("Character damage settings")]
    [SerializeField] ParticleSystem m_damageFx;

    public Health Health { get => _characterHealth; }

    public override void Awake()
    {
        base.Awake();
        _characterHealth.OnDeath += Death;
    }


    public virtual void Death()
    {
        isDead = true;
    }

    public override void TakeDamage(DamageInfo p_damageInfo)
    {
        base.TakeDamage(p_damageInfo);

        //VFX
        if(m_damageFx != null)
        {
            m_damageFx.transform.position = p_damageInfo.DamageContactPosition;
            m_damageFx.transform.rotation = Quaternion.LookRotation(p_damageInfo.DamageContactNormal);
            m_damageFx.Play();

            //Instantiate(m_impactFx, l_hit.point, Quaternion.LookRotation(l_hit.normal));
        }

        _characterHealth.RemoveLifePoint(p_damageInfo.DamagePoints);
    }
}
