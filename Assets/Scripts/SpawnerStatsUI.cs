using UnityEngine;
using TMPro;

public class SpawnerStatsUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CubeSpawner _cubeSpawner;
    [SerializeField] private BombSpawner _bombSpawner;
    [SerializeField] private TMP_Text _cubeStatsText;
    [SerializeField] private TMP_Text _bombStatsText;

    [Header("Settings")]
    [SerializeField] private float _updateInterval = 0.3f;

    private float _nextUpdateTime;

    private void Update()
    {
        if (Time.time < _nextUpdateTime)
            return;

        _nextUpdateTime = Time.time + _updateInterval;

        UpdateCubeStats();
        UpdateBombStats();
    }

    private void UpdateCubeStats()
    {
        if (_cubeSpawner == null || _cubeStatsText == null)
            return;

        _cubeStatsText.text = $"Cubes:\n" +
                             $"Total: {_cubeSpawner.TotalSpawned}\n" +
                             $"Created: {_cubeSpawner.TotalCreated}\n" +
                             $"Active: {_cubeSpawner.ActiveCount}";
    }

    private void UpdateBombStats()
    {
        if (_bombSpawner == null || _bombStatsText == null) 
            return;

        _bombStatsText.text = $"Bombs:\n" +
                             $"Total: {_bombSpawner.TotalSpawned}\n" +
                             $"Created: {_bombSpawner.TotalCreated}\n" +
                             $"Active: {_bombSpawner.ActiveCount}";
    }
}