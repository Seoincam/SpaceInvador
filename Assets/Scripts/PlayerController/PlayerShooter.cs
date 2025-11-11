using System.Collections.Generic;
using DefaultNamespace;
using Services;
using UnityEngine;

namespace PlayerController
{
    public interface IPlayerShooter
    {
        void SetAimInput(Vector2 aimInput);
        void TryFire();
        void TryRetrieve();
    }
    
    public class PlayerShooter : MonoBehaviour, IPlayerShooter
    {
        [SerializeField] private int maxBulletCount = 4;
        
        private Camera _cam;
        
        private Vector2 _aimInput;
        private Vector2 _aimDirection;
        private Queue<IBullet> _bullets;

        private void Awake()
        {
            _cam = Camera.main;
            _bullets = new Queue<IBullet>(maxBulletCount);
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
            if (_bullets.Count < maxBulletCount)
            {
                Shoot(_aimDirection);
            }
        }

        public void TryRetrieve()
        {
            if (_bullets.TryDequeue(out var bullet))
            {
                GameServices.Spawner.Despawn(bullet.GameObject);
            }
        }

        private void Shoot(Vector2 direction)
        {
            var bulletGo = GameServices.Spawner.Spawn("bullet", transform.position);
            if (bulletGo.TryGetComponent(out IBullet bullet))
            {
                _bullets.Enqueue(bullet);
                bullet.Shoot(direction);
            }
        }
    }
}