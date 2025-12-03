using System.Collections.Generic;
using System.Net.Http;
using DefaultNamespace;
using Services;
using TimeKit;
using UnityEngine;

namespace PlayerController
{
    public interface IPlayerShooter
    {
        void SetAimInput(Vector2 aimInput);
        void TryFire();
        void TryRetrieve();
        void Retrieve(IBullet bullet);
        
        IReadOnlyCooldown FireCooldown { get; }
        IReadOnlyCooldown RetrieveCooldown { get; }
    }
    
    public class PlayerShooter : MonoBehaviour, IPlayerShooter, IDamageable
    {
        [SerializeField] private Transform bulletSafeArea;
        
        [SerializeField] private int maxBulletCount = 4;
        [SerializeField] private float cooldownDuration = .5f;

        private Clock _clock;
        private Camera _cam;
        
        private Vector2 _aimInput;
        private Vector2 _aimDirection;
        private List<IBullet> _bullets;
        
        private Cooldown _fireCooldown;
        private Cooldown _retrieveCooldown;
        
        public IReadOnlyCooldown FireCooldown => _fireCooldown;
        public IReadOnlyCooldown RetrieveCooldown => _retrieveCooldown;

        private void Awake()
        {
            _cam = Camera.main;
            
            _clock = TimeManager.GetClock(ClockType.GamePlay);

            if (_clock != null)
            {
                Debug.Log("Clock found");
            }

            if (_clock is IReadOnlyClock rc1)
            {
                Debug.Log("IReadOnly Clock found");
            }

            if (_clock is not IReadOnlyClock rc)
            {
                Debug.LogWarning("PlayerShooter: clock is not IReadOnlyClock");
            }
            
            _fireCooldown = _clock.Cooldown(cooldownDuration);
            _retrieveCooldown = _clock.Cooldown(cooldownDuration);
            
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
            if (!_fireCooldown.IsReady || _bullets.Count >= maxBulletCount)
                return;
            Shoot(_aimDirection);
        }

        public void TryRetrieve()
        {
            if (!_retrieveCooldown.IsReady || _bullets.Count <= 0) 
                return;

            foreach (var bullet in _bullets)
            {
                if (!bullet.CanRetrieve)
                {
                    bullet.SetRetrieve();
                    break;
                }
            }
            _retrieveCooldown.TryConsume();
        }
        
        public void TakeDamage()
        {
            Debug.Log($"Player {gameObject.name} damaged!");
            // TODO: 총알 회수 처리해야함.
        }
        
        public void Retrieve(IBullet bullet)
        {
            _bullets.Remove(bullet);
        }

        private void Shoot(Vector2 direction)
        {
            _fireCooldown.TryConsume();
            
            var bulletGo = GameServices.Spawner.Spawn("Objects/bullet", transform.position);
            if (!bulletGo.TryGetComponent(out IBullet bullet)) 
                return;
            
            _bullets.Add(bullet);
            bullet.Shoot(direction, bulletSafeArea);
        }
    }
}