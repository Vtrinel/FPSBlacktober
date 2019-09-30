using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData_0", menuName = "SciptableObject/Data/Weapon", order = 0)]
public class WeaponData : ScriptableObject
{
    [SerializeField] float m_damage = 1;
    [SerializeField] float m_range = 100;
    [SerializeField, Range(0.0f, 1.0f)] float m_accuracy = 100;
    [SerializeField] float m_recoilAmplitude;
    [SerializeField, Min(0.01f)] float m_fireRate = 5;
    [SerializeField, Min(1)] int m_ammo = 30;
    [SerializeField, Min(0.01f)] float m_reloadTime = 1;

    public float Range { get => m_range; }
    public float Damage { get => m_damage; }
    public float Accuracy { get => m_accuracy; }
    public float Recoil { get => m_recoilAmplitude; }
    public float FireRate { get => m_fireRate; }
    public int Ammo { get => m_ammo; }
    public float ReloadTime { get => m_reloadTime; }
}
