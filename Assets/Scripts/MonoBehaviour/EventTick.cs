using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class EventTick : MonoBehaviour
{
    [SerializeField,Min(0)] float tickTime = 0.5f;
    [SerializeField,ReadOnly] bool tickIsActive = false;
    [SerializeField] bool activeOnStart = true;
    [SerializeField] UnityEvent tickEvent;

    IEnumerator currentTick;

    private void Start()
    {
        if (activeOnStart)
        {
            StartTick();
        }
    }

    public void StartTick()
    {
        currentTick = Tick();
        StartCoroutine(currentTick);
        tickIsActive = true;
    }

    public void StopTick()
    {
        if(currentTick != null)
        {
            StopCoroutine(currentTick);
        }
        tickIsActive = false;
    }

    IEnumerator Tick()
    {
        yield return new WaitForSeconds(tickTime);
        tickEvent.Invoke();
        if (tickIsActive)
        {
            StartTick();
        }
    }
}
