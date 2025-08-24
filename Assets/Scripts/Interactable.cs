using System;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public event EventHandler OnInteract;
    public event EventHandler OnEndInteract;
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnInteract?.Invoke(this, EventArgs.Empty);
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnEndInteract?.Invoke(this, EventArgs.Empty);
        }
    }
}
