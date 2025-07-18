using System;
using UnityEngine;
using System.Collections;

public class CubeLifetime : MonoBehaviour
{
    [SerializeField] private float _lifetimeAfterCollision = 2f;
    public event Action<CubeLifetime> LifetimeEnded;

    public void StartLifetimeCountdown()
    {
        StartCoroutine(LifetimeCoroutine());
    }

    private IEnumerator LifetimeCoroutine()
    {
        yield return new WaitForSeconds(_lifetimeAfterCollision);
        LifetimeEnded?.Invoke(this);
    }

    public void CancelCountdown()
    {
        StopAllCoroutines();
    }
}