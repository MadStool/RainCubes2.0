using UnityEngine;
using TMPro;

public class CubeStatsUI : SpawnerStats<CubeInteraction>
{
    protected override void UpdateUI()
    {
        if (_statsText == null || _spawner == null)
            return;

        _statsText.text = $"Cubes:\nTotal: {_spawner.TotalSpawned}\n" +
                         $"Created: {_spawner.TotalCreated}\n" +
                         $"Active: {_spawner.ActiveCount}";
    }
}