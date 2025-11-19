using PlayerController;
using TimeKit;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerShootCooldownView : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private Slider fireCooldownSlider;
        [SerializeField] private Slider retrieveCooldownSlider;
        
        private CooldownObserver _fireObserver;
        private CooldownObserver _retrieveObserver;
        
        private void Awake()
        {
            var shooter = player.GetComponent<IPlayerShooter>();

            _fireObserver = shooter.FireCooldown.Observer();
            _retrieveObserver = shooter.RetrieveCooldown.Observer();
            
            _fireObserver.RatioChanged += OnFireCooldownRatioChanged;
            _retrieveObserver.RatioChanged += OnRetrieveCooldownRatioChanged;
        }

        private void OnDestroy()
        {
            _fireObserver.RatioChanged -= OnFireCooldownRatioChanged;
            _retrieveObserver.RatioChanged -= OnRetrieveCooldownRatioChanged;
            
            _fireObserver.Dispose();
            _retrieveObserver.Dispose();
        }

        private void OnFireCooldownRatioChanged(float ratio)
        {
            fireCooldownSlider.value = ratio;
        }

        private void OnRetrieveCooldownRatioChanged(float ratio)
        {
            retrieveCooldownSlider.value = ratio;
        }
    }
}