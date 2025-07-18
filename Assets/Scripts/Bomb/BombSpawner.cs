using UnityEngine;

public class BombSpawner : BaseSpawner<BombInteraction>
{
    protected override void Spawn() { }

    public void SpawnBomb(Vector3 position)
    {
        if (_pool == null)
        {
            Debug.LogError("BombPool reference is missing!");
            return;
        }

        BombInteraction bomb = _pool.Get();

        if (bomb == null)
        {
            Debug.LogError("Failed to get bomb from pool");
            return;
        }

        bomb.Initialize(position, _pool);
        TotalSpawned++;
    }
}