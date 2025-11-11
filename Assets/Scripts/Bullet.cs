using Services;
using UnityEngine;

namespace DefaultNamespace
{
    public interface IBullet
    {
        void Shoot(Vector2 direction);
    }
    
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour, IBullet
    {
        [SerializeField] private float speed;
        [SerializeField] private int wrapCount;
        
        private const float Bound = 8.5f;

        private Rigidbody2D _rb;
        private Vector2 _direction;
        private int _maxWrapCount;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.gravityScale = 0;
            _rb.freezeRotation = true;
            
            _maxWrapCount = wrapCount;
        }

        private void OnEnable()
        {
            wrapCount = _maxWrapCount;
        }
        private void OnDisable()
        {
            _rb.linearVelocity = Vector2.zero; // movePosition() 기반이라 필요 없지만, 혹시 velocity를 쓸 수 있으니 초기화.
            _direction = Vector2.zero;
        }
        
        private void FixedUpdate()
        {
            Vector2 nextPos = _rb.position + _direction * (speed * Time.fixedDeltaTime);
            bool wrapped = false;
            
            // X Wrap
            if (nextPos.x > Bound)
            {
                nextPos.x = -Bound;
                wrapped = true;
            }
            else if (nextPos.x < -Bound)
            {
                nextPos.x = Bound;
                wrapped = true;
            }

            // Y Wrap
            if (nextPos.y > Bound)
            {
                nextPos.y = -Bound;
                wrapped = true;
            }
            else if (nextPos.y < -Bound)
            {
                nextPos.y = Bound;
                wrapped = true;
            }

            if (wrapped)
            {
                wrapCount--;
                if (wrapCount < 0)
                {
                    GameServices.Spawner.Despawn(gameObject);
                    return;
                }
            }
            
            _rb.MovePosition(nextPos);
        }

        public void Shoot(Vector2 direction)
        {
            _direction = direction;
            
            // 회전
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}