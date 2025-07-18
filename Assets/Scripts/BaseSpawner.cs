using UnityEngine;
using System.Collections;

public abstract class BaseSpawner<T> : MonoBehaviour where T : MonoBehaviour
{
    public event System.Action OnCountChanged;

    [Header("Spawn Settings")]
    [SerializeField] protected Vector3 _spawnArea = new Vector3(10f, 0f, 10f);
    [SerializeField, Range(0.1f, 10f)] protected float _spawnRate = 1f;
    [SerializeField, Range(5f, 20f)] protected float _spawnHeight = 10f;
    [SerializeField, Range(1, 100)] protected int _maxActiveObjects = 30;

    protected ObjectPool<T> _pool;
    protected Coroutine _spawnRoutine;
    protected bool _isActive;

    public int TotalSpawned { get; protected set; }
    public int ActiveCount => _pool?.ActiveCount ?? 0;
    public int TotalCreated => _pool?.TotalCreated ?? 0;

    protected virtual void Start()
    {
        _pool = GetComponentInChildren<ObjectPool<T>>();

        if (_pool != null)
        {
            _pool.OnPoolUpdated += HandlePoolUpdate;
            UpdateCounts();
        }

        BeginSpawning();
    }

    private void HandlePoolUpdate()
    {
        UpdateCounts();
    }

    private void UpdateCounts()
    {
        OnCountChanged?.Invoke();
    }

    protected virtual void OnDestroy()
    {
        if (_pool != null)
        {
            _pool.OnPoolUpdated -= HandlePoolUpdate;
        }

        StopSpawning();
    }

    public void BeginSpawning()
    {
        if (_isActive || _spawnRoutine != null)
            return;

        _isActive = true;
        _spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    public void StopSpawning()
    {
        _isActive = false;
    }

    protected virtual IEnumerator SpawnRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(1f / _spawnRate);

        while (_isActive && (_pool == null || _pool.ActiveCount < _maxActiveObjects))
        {
            if (_pool != null && _pool.ActiveCount < _maxActiveObjects)
            {
                Spawn();
            }

            yield return wait;
        }

        _spawnRoutine = null;
    }

    protected abstract void Spawn();

    protected Vector3 GetRandomPosition()
    {
        return new Vector3(
            Random.Range(-_spawnArea.x / 2f, _spawnArea.x / 2f),
            _spawnHeight,
            Random.Range(-_spawnArea.z / 2f, _spawnArea.z / 2f)
        );
    }

    protected void NotifyCountChanged() => OnCountChanged?.Invoke();
}