using UnityEngine;
using System.Collections.Generic;
using System;

public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T _prefab;
    [SerializeField] private int _initialSize = 20;

    public event Action OnPoolUpdated;

    private Queue<T> _inactive = new Queue<T>();
    private List<T> _active = new List<T>();

    public int ActiveCount => _active.Count;
    public int TotalCreated { get; private set; }

    private void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < _initialSize; i++)
        {
            CreateNewObject();
        }

        NotifyPoolUpdated();
    }

    private T CreateNewObject()
    {
        var pooledObject = Instantiate(_prefab, transform);
        pooledObject.gameObject.SetActive(false);
        _inactive.Enqueue(pooledObject);
        TotalCreated++;

        return pooledObject;
    }

    public T Get()
    {
        if (_inactive.Count == 0)
        {
            CreateNewObject();
        }

        var pooledObject = _inactive.Dequeue();
        _active.Add(pooledObject);
        pooledObject.gameObject.SetActive(true);
        NotifyPoolUpdated();

        return pooledObject;
    }

    public void Return(T pooledObject)
    {
        if (_active.Contains(pooledObject) == false) 
            return;

        _active.Remove(pooledObject);
        _inactive.Enqueue(pooledObject);
        pooledObject.gameObject.SetActive(false);
        NotifyPoolUpdated();
    }

    private void NotifyPoolUpdated()
    {
        OnPoolUpdated?.Invoke();
    }
}