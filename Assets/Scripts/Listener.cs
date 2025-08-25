using System;
using System.Collections;
using UnityEngine;

public class Listener : MonoBehaviour
{
    [SerializeField] protected Interactable[] interactable;
    [SerializeField] private bool carryOutEventOnce = true;
    [SerializeField] private float startDelay;
    [SerializeField] private float endDelay;

    private void StartEvent(object sender, EventArgs e)
    {
        StartCoroutine(StartEventCoroutine());
    }

    private IEnumerator StartEventCoroutine()
    {
        yield return new WaitForSeconds(startDelay);
        CarryOutEvent(this, EventArgs.Empty);
        
    }

    protected virtual void CarryOutEvent(object sender, EventArgs e)
    {
        if (carryOutEventOnce)
        {
            OnDestroy();
        }
    }
    private void StartEndEvent(object sender, EventArgs e)
    {
        StartCoroutine(StartEndEventCoroutine());
    }

    private IEnumerator StartEndEventCoroutine()
    {
        yield return new WaitForSeconds(endDelay);
        CarryOutEndEvent(this, EventArgs.Empty);
        
    }

    protected virtual void CarryOutEndEvent(object sender, EventArgs e)
    {
        if (carryOutEventOnce)
        {
            OnDestroy();
        }
    }
    
    protected virtual void Start()
    {
        foreach (var inter in interactable)
        {
            if (inter == null) continue;
            inter.OnInteract += StartEvent;
            inter.OnEndInteract += StartEndEvent;
        }
    }

    protected virtual void OnDestroy()
    {
        foreach (var inter in interactable)
        {
            if (inter == null) continue;
            inter.OnInteract -= StartEvent;
            inter.OnEndInteract -= StartEndEvent;
        }
    }
}
