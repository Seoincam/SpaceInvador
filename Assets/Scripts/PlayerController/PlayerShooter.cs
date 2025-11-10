using Services;
using UnityEngine;

namespace PlayerController
{
    public interface IPlayerShooter
    {
        void SetAimInput(Vector2 aimInput);
        void TryFire();
    }
    
    public class PlayerShooter : MonoBehaviour, IPlayerShooter
    {
        private Camera _cam;
        
        private Vector2 _aimInput;
        private Vector2 _aimDirection;

        private void Awake()
        {
            _cam = Camera.main;
        }

        private void Update()
        {
            Vector2 newDirection = _aimInput - (Vector2)transform.position;
            if (newDirection.sqrMagnitude > 0.001f)
                _aimDirection = newDirection.normalized;
        }

        public void SetAimInput(Vector2 aimInput)
        {
            _aimInput = _cam.ScreenToWorldPoint(aimInput);
        }

        public void TryFire()
        {
            Debug.Log("Try Fire: " + _aimDirection);
            Shoot(_aimDirection);
        }

        private void Shoot(Vector2 direction)
        {
            var bulletGo = GameServices.Spawner.Spawn("bullet", transform.position);
            if (bulletGo.TryGetComponent(out Rigidbody2D rb))
            {
                rb.linearVelocity = direction;
            }
        }
    }
}