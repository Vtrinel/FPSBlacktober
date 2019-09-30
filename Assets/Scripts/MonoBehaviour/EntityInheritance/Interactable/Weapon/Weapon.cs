using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Weapon : Interactable
{
    [SerializeField] WeaponData m_weaponData;
    [SerializeField] LayerMask m_targetLayer;

    float m_lastfired;
    int m_currentAmmo;
    bool m_isReloading;

    [Header("VFX")]
    [SerializeField] ParticleSystem flashFx;
    [SerializeField] AudioSource shootSfx;
    

    public bool OutOfAmmo
    {
        get
        {
            return m_currentAmmo == 0;
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        m_currentAmmo = m_weaponData.Ammo;
    }

    public void ShootOnTarget(Vector3 p_targetPos)
    {
        if (Time.time - m_lastfired <= 1 / m_weaponData.FireRate) return;

        if (OutOfAmmo)
        {
            Reload();
            return;
        }

        m_currentAmmo--;
        m_lastfired = Time.time;
        Vector3 l_recoil = new Vector3(Recoil(), Recoil(), 0);
        Vector3 l_shotDirection = CustomMethod.GetNormalizedDirection(transform.position, (p_targetPos + l_recoil));
        Ray l_ray = new Ray(transform.position, l_shotDirection);
        Physics.Raycast(l_ray, out RaycastHit l_hit, m_weaponData.Range, m_targetLayer, QueryTriggerInteraction.Collide);
        Debug.DrawRay(l_ray.origin, l_ray.direction * m_weaponData.Range, Color.magenta, 3);

        if (l_hit.collider != null)
        {
            IDamagable damagable = l_hit.transform.GetComponent<IDamagable>();

            if (damagable != null)
            {
                damagable.TakeDamage(new DamageInfo(m_weaponData.Damage, l_hit.point,l_hit.normal));
            }
        }

        //FX
        shootSfx?.Play();
        flashFx?.Play();
    }

    void Reload()
    {
        if (m_isReloading) return;
        StartCoroutine(Reloading());
    }

    IEnumerator Reloading()
    {
        m_isReloading = true;
        yield return new WaitForSeconds(m_weaponData.ReloadTime);
        m_currentAmmo = m_weaponData.Ammo;
        m_isReloading = false;
    }

    float Recoil()
    {
        float l_accuracy = Random.Range(m_weaponData.Accuracy, 1);
        float l_recoilSide = Random.Range(-1, 1);
        int l_side = Mathf.RoundToInt(l_recoilSide);
        return l_side * (m_weaponData.Recoil * (1 - l_accuracy));
    }
}
