using UnityEngine;
using System.Collections;

public abstract class BaseSpawner<TSpawnable> : MonoBehaviour where TSpawnable : MonoBehaviour
{
    [SerializeField] protected Vector3 _spawnArea = new Vector3(10f, 0f, 10f);
    [SerializeField, Range(0.1f, 10f)] protected float _spawnRate = 1f;
    [SerializeField, Range(5f, 20f)] protected float _spawnHeight = 10f;
    [SerializeField, Range(1, 100)] private int _maxActiveObjects = 30;

    protected ObjectPool<TSpawnable> _pool;
    protected Coroutine _spawnRoutine;
    protected bool _isActive;

    public int TotalSpawned { get; protected set; }

    protected ObjectPool<TSpawnable> Pool => _pool;
    public int ActiveCount => _pool != null ? _pool.ActiveCount : 0;
    public int TotalCreated => _pool != null ? _pool.TotalCreated : 0;

    protected virtual void Start()
    {
        ValidateReferences();
        BeginSpawning();
    }

    protected virtual void OnValidate()
    {
        ValidateReferences();
    }

    protected virtual void ValidateReferences()
    {
        if (_pool == null)
            _pool = GetComponentInChildren<ObjectPool<TSpawnable>>(true);
    }

    public void BeginSpawning()
    {
        if (_isActive) 
            return;

        _isActive = true;
        _spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    public void StopSpawning()
    {
        if (_isActive == false) 
            return;

        _isActive = false;

        if (_spawnRoutine != null)
        {
            StopCoroutine(_spawnRoutine);
            _spawnRoutine = null;
        }
    }

    protected virtual IEnumerator SpawnRoutine()
    {
        var waitTime = new WaitForSeconds(1f / _spawnRate);

        while (_isActive)
        {
            if (_pool.ActiveCount < _maxActiveObjects)
            {
                Spawn();
            }

            yield return waitTime;
        }
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

    protected virtual void OnDestroy()
    {
        StopSpawning();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(
            new Vector3(0, _spawnHeight, 0),
            new Vector3(_spawnArea.x, 0.1f, _spawnArea.z)
        );
    }
}