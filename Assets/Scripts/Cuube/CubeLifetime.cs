using UnityEngine;
using System;

[RequireComponent(typeof(CubeInteraction))]
public class CubeLifetime : MonoBehaviour
{
    public event Action<CubeLifetime> LifetimeEnded;

    [SerializeField] private float _lifetimeAfterCollision = 2f;

    private float _currentLifetime;
    private bool _isCounting;

    public void StartLifetimeCountdown()
    {
        _currentLifetime = _lifetimeAfterCollision;
        _isCounting = true;
    }

    private void Update()
    {
        if (_isCounting == false) 
            return;

        _currentLifetime -= Time.deltaTime;

        if (_currentLifetime <= 0f)
        {
            _isCounting = false;
            LifetimeEnded?.Invoke(this);
        }
    }

    public void ResetLifetime()
    {
        _isCounting = false;
        _currentLifetime = 0f;
    }
}