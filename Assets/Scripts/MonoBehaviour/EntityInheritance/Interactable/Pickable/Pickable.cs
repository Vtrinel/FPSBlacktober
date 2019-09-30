using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : Interactable, IPickable
{
    [SerializeField] AudioSource m_audio;
    public virtual void PickUpObject()
    {
        m_audio.Play();
        DestroyEntity();
    }
}
