using TimeKit;
using UnityEngine;

namespace PlayerController
{
    public interface IPlayerMover
    {
        void SetMoveInput(Vector2 moveInput);
    }
    
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMover : MonoBehaviour, IPlayerMover
    {
        [SerializeField] private float moveSpeed;

        private const float Bound = 8.5f;

        private Clock _clock;
        private Rigidbody2D _rb;
        private Vector2 _moveInput;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.gravityScale = 0;
            _rb.freezeRotation = true;
            
            _clock = TimeManager.GetClock(ClockType.GamePlay);
        }

        private void FixedUpdate()
        {
            // Wrap
            Vector2 delta = _moveInput * (moveSpeed * Time.fixedDeltaTime * _clock.TimeScale);
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

        public void SetMoveInput(Vector2 moveInput)
        {
            _moveInput = moveInput;
        }
    }
}