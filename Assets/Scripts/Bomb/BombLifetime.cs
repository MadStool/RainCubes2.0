using UnityEngine;

public class BombLifetime : MonoBehaviour
{
    [SerializeField] private Vector2 _lifetimeRange = new Vector2(2f, 5f);
    private BombInteraction _bomb;
    private BombVisuals _visuals;
    private float _endTime;
    private bool _countdownFinished;

    private void Awake()
    {
        _bomb = GetComponent<BombInteraction>();
        _visuals = GetComponent<BombVisuals>();
    }

    public void StartCountdown()
    {
        _endTime = Time.time + Random.Range(_lifetimeRange.x, _lifetimeRange.y);
        _countdownFinished = false;
        enabled = true;
    }

    private void Update()
    {
        if (_countdownFinished) 
            return;

        float progress = Mathf.Clamp01(1 - (_endTime - Time.time) / (_lifetimeRange.y));
        _visuals.UpdateFade(progress);

        if (Time.time >= _endTime)
        {
            _countdownFinished = true;
            enabled = false;
            _bomb.Explode();
        }
    }

    private void OnDisable()
    {
        if (_countdownFinished == false)
        {
            _bomb.Explode();
        }
    }
}