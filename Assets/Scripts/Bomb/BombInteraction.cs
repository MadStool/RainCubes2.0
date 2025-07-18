using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BombVisuals), typeof(BombLifetime))]
public class BombInteraction : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private float _explosionForce = 300f;

    private Rigidbody _rigidbody;
    private BombVisuals _visuals;
    private BombLifetime _lifetime;
    private ObjectPool<BombInteraction> _pool;
    private bool _hasExploded;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _visuals = GetComponent<BombVisuals>();
        _lifetime = GetComponent<BombLifetime>();
    }

    public void Initialize(Vector3 position, ObjectPool<BombInteraction> pool)
    {
        _pool = pool;
        transform.position = position;
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _visuals.ResetVisuals();
        _hasExploded = false;
        _lifetime.StartCountdown();
    }

    public void Explode()
    {
        if (_hasExploded)
            return;

        _hasExploded = true;

        Collider[] colliders = new Collider[20];
        int count = Physics.OverlapSphereNonAlloc(
            transform.position,
            _explosionRadius,
            colliders
        );

        for (int i = 0; i < count; i++)
        {
            var rigidbody = colliders[i].attachedRigidbody;

            if (rigidbody != null && rigidbody != _rigidbody)
            {
                rigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
            }
        }

        if (_pool != null)
        {
            _pool.Return(this);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}