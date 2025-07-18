using UnityEngine;

public class CubeSpawner : BaseSpawner<CubeInteraction>
{
    [Header("Settings")]
    [SerializeField] private Color _defaultColor = Color.white;
    [SerializeField] private BombSpawner _bombSpawner;

    protected override void Spawn()
    {
        if (_pool == null)
            return;

        CubeInteraction cube = _pool.Get();
        cube.Initialize(_defaultColor, GetRandomPosition(), _bombSpawner, _pool);
        TotalSpawned++;
    }
}