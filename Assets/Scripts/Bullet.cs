using UnityEngine;

namespace DefaultNamespace
{
    public interface IBullet
    {
        GameObject GameObject { get; }
        void Shoot(Vector2 direction);
    }
    
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour, IBullet
    {
        [SerializeField] private float speed;
        
        private const float Bound = 8.5f;

        private Rigidbody2D _rb;
        private Vector2 _direction;

        public GameObject GameObject => gameObject;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.gravityScale = 0;
            _rb.freezeRotation = true;
        }
        
        private void OnDisable()
        {
            _rb.linearVelocity = Vector2.zero; // movePosition() 기반이라 필요 없지만, 혹시 velocity를 쓸 수 있으니 초기화.
            _direction = Vector2.zero;
        }
        
        private void FixedUpdate()
        {
            Vector2 nextPos = _rb.position + _direction * (speed * Time.fixedDeltaTime);
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

        public void Shoot(Vector2 direction)
        {
            _direction = direction;
            
            // 회전
            // float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            // transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}