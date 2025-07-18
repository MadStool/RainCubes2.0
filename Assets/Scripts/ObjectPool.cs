using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<TComponent> : MonoBehaviour where TComponent : MonoBehaviour
{
    [SerializeField] private TComponent _prefab;
    [SerializeField] private int _initialPoolSize = 20;
    [SerializeField] private int _maxActiveObjects = 20;

    private Queue<TComponent> _inactiveObjects = new Queue<TComponent>();
    private List<TComponent> _activeObjects = new List<TComponent>();

    public int TotalCreated { get; private set; }
    public int ActiveCount => _activeObjects.Count;

    private void Start() => WarmPool();

    private void WarmPool()
    {
        for (int i = 0; i < _initialPoolSize; i++)
            CreateNewObject();
    }

    private TComponent CreateNewObject()
    {
        var pooledObject = Instantiate(_prefab);

        pooledObject.gameObject.SetActive(false);
        _inactiveObjects.Enqueue(pooledObject);
        TotalCreated++;

        return pooledObject;
    }

    public TComponent Get()
    {
        if (_inactiveObjects.Count == 0 && _activeObjects.Count < _maxActiveObjects)
        {
            CreateNewObject();
        }
        else if (_inactiveObjects.Count == 0)
        {
            return null;
        }

        var pooledObject = _inactiveObjects.Dequeue();

        _activeObjects.Add(pooledObject);
        pooledObject.gameObject.SetActive(true);

        return pooledObject;
    }

    public void Return(TComponent pooledObject)
    {
        if (pooledObject == null) 
            return;

        if (_activeObjects.Contains(pooledObject))
        {
            _activeObjects.Remove(pooledObject);
            _inactiveObjects.Enqueue(pooledObject);
            pooledObject.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Trying to return object not from active list");
        }
    }
}