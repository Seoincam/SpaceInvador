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
        void Retrieve(IBullet bullet);
    }
    
    public class PlayerShooter : MonoBehaviour, IPlayerShooter, IDamageable
    {
        [SerializeField] private int maxBulletCount = 4;
        [SerializeField] private Transform bulletSafeArea;
        
        private Camera _cam;
        
        private Vector2 _aimInput;
        private Vector2 _aimDirection;
        private List<IBullet> _bullets;

        private void Awake()
        {
            _cam = Camera.main;
            
            _bullets = new List<IBullet>(maxBulletCount);
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
            if (_bullets.Count >= maxBulletCount)
                return;
            Shoot(_aimDirection);
        }

        public void TryRetrieve()
        {
            if (_bullets.Count <= 0) 
                return;

            foreach (var bullet in _bullets)
            {
                if (!bullet.CanRetrieve)
                {
                    bullet.SetRetrieve();
                    break;
                }
            }
        }

        public void Retrieve(IBullet bullet)
        {
            _bullets.Remove(bullet);
        }
        
        public void TakeDamage()
        {
            Debug.Log($"Player {gameObject.name} damaged!");
        }

        private void Shoot(Vector2 direction)
        {
            var bulletGo = GameServices.Spawner.Spawn("bullet", transform.position);
            if (!bulletGo.TryGetComponent(out IBullet bullet)) 
                return;
            
            _bullets.Add(bullet);
            bullet.Shoot(direction, bulletSafeArea);
        }
    }
}