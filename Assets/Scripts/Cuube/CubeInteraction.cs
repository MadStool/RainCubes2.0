using UnityEngine;

[RequireComponent(typeof(CubePhysics))]
[RequireComponent(typeof(CubeVisuals))]
[RequireComponent(typeof(CubeLifetime))]
public class CubeInteraction : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Color _collisionColor = Color.red;

    private BombSpawner _bombSpawner;
    private CubePhysics _physics;
    private CubeVisuals _visuals;
    private CubeLifetime _lifetime;
    private bool _hasCollided;
    private ObjectPool<CubeInteraction> _pool;

    private void Awake()
    {
        _physics = GetComponent<CubePhysics>();
        _visuals = GetComponent<CubeVisuals>();
        _lifetime = GetComponent<CubeLifetime>();

        _lifetime.LifetimeEnded += OnLifetimeEnded;
    }

    private void OnDestroy()
    {
        _lifetime.LifetimeEnded -= OnLifetimeEnded;
    }

    public void Initialize(Color color, Vector3 position, BombSpawner bombSpawner, ObjectPool<CubeInteraction> pool)
    {
        _bombSpawner = bombSpawner ?? throw new System.ArgumentNullException(nameof(bombSpawner));
        _pool = pool ?? throw new System.ArgumentNullException(nameof(pool));
        _visuals.SetColor(color);
        transform.SetPositionAndRotation(position, Quaternion.identity);
        _physics.ResetPhysics();
        _hasCollided = false;
    }

    private void OnEnable()
    {
        _hasCollided = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasCollided || !collision.collider.TryGetComponent<Platform>(out Platform platform))
            return;

        _hasCollided = true;
        _visuals.SetColor(_collisionColor);
        _lifetime.StartLifetimeCountdown();
    }

    private void OnLifetimeEnded(CubeLifetime cubeLifetime)
    {
        _bombSpawner.SpawnBomb(transform.position);

        if (_pool != null)
        {
            _pool.Return(this);
        }
        else
        {
            gameObject.SetActive(false);
            Debug.LogWarning("Pool reference is missing - object deactivated instead of returned to pool");
        }
    }
}