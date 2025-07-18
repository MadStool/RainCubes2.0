using UnityEngine;
using TMPro;

public abstract class SpawnerStats<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected BaseSpawner<T> _spawner;
    [SerializeField] protected TMP_Text _statsText;

    protected virtual void Start()
    {
        UpdateUI();
    }

    private void OnEnable()
    {
        if (_spawner != null)
        {
            _spawner.OnCountChanged += UpdateUI;
            UpdateUI();
        }
    }

    private void OnDisable()
    {
        if (_spawner != null)
        {
            _spawner.OnCountChanged -= UpdateUI;
        }
    }

    protected abstract void UpdateUI();
}