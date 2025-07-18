using UnityEngine;

public class BombSpawner : BaseSpawner<BombInteraction>
{
    protected override void Spawn() { }

    public void SpawnBomb(Vector3 position)
    {
        if (_pool == null) 
            return;

        BombInteraction bomb = _pool.Get();
        bomb.Initialize(position, _pool);
        TotalSpawned++;
        NotifyCountChanged();
    }
}