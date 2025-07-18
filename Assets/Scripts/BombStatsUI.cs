using UnityEngine;

public class BombStatsUI : SpawnerStats<BombInteraction>
{
    protected override void UpdateUI()
    {
        if (_statsText == null || _spawner == null)
            return;

        _statsText.text = $"Bombs:\nTotal: {_spawner.TotalSpawned}\n" +
                         $"Created: {_spawner.TotalCreated}\n" +
                         $"Active: {_spawner.ActiveCount}";
    }
}