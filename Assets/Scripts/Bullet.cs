using PlayerController;
using Services;
using TimeKit;
using UnityEngine;

namespace DefaultNamespace
{
    public interface IBullet
    {
        bool CanRetrieve { get; }
        GameObject GameObject { get; }
        void Shoot(Vector2 direction, Transform safeArea);
        void SetRetrieve();
    }
    
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class Bullet : MonoBehaviour, IBullet
    {
        [SerializeField] private float speed;
        [SerializeField] private LayerMask targetLayer;
        
        private const float Bound = 8.5f;

        private Rigidbody2D _rb;
        private SpriteRenderer _renderer;

        private bool _isActive;
        private Vector2 _direction;
        private Transform _safeArea;

        private Clock _clock;
        public bool CanRetrieve { get; private set; }
        public GameObject GameObject => gameObject;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.gravityScale = 0;
            _rb.freezeRotation = true;
            
            _renderer = GetComponent<SpriteRenderer>();

            _clock = TimeManager.GetClock(ClockType.GamePlay);
        }
        
        private void OnDisable()
        {
            _rb.linearVelocity = Vector2.zero; // movePosition() 기반이라 필요 없지만, 혹시 velocity를 쓸 수 있으니 초기화.
            _direction = Vector2.zero;
            _isActive = false;
        }
        
        private void FixedUpdate()
        {
            if (_clock.IsStopped)
                return;

            Vector2 delta = _direction * (speed * Time.fixedDeltaTime * _clock.TimeScale);
            Vector2 nextPos = _rb.position + delta;
            nextPos.x = nextPos.x switch
            {
                > Bound => -Bound,
                < -Bound => Bound,
                _ => nextPos.x
            };
            nextPos.y = nextPos.y switch
            {
                > Bound => -Bound,
                < -Bound => Bound,
                _ => nextPos.y
            };
            
            _rb.MovePosition(nextPos);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isActive)
                return;
            if ((targetLayer & (1 << other.gameObject.layer)) == 0)
                return;
            if (!other.TryGetComponent(out IDamageable damageable))
                return;

            if (CanRetrieve && other.TryGetComponent(out IPlayerShooter player))
            {
                player.Retrieve(this);
                Despawn();
                return;
            }
            
            damageable.TakeDamage();
            Despawn();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.transform == _safeArea)
            {
                _isActive = true;
            }
        }

        public void Shoot(Vector2 direction,Transform safeArea)
        {
            _direction = direction;
            _safeArea = safeArea;
            
            _isActive = false;
            CanRetrieve = false;
            _renderer.color = Color.yellow;
        }

        public void SetRetrieve()
        {
            _direction *= -1;
            CanRetrieve = true;
            _renderer.color = Color.white;
        }

        private void Despawn()
        {
            GameServices.Spawner.Despawn(gameObject);
        }
    }
}