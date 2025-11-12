using UnityEngine;

namespace Services.Time
{
    public class TimeService : MonoBehaviour
    {
        public static TimeService Instance { get; private set; }

        [SerializeField] private GameClock gamePlayClock;
        [SerializeField] private GameClock uiClock;
        
        public IClock GamePlayClock => gamePlayClock;
        public IClock UIClock => uiClock;

        private void Awake()
        {
            if (Instance != null) 
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            gamePlayClock = new GameClock();
            uiClock = new GameClock();
        }

        private void Update()
        {
            // TimeScale 등에 영향 받지 않는 Unity의 실제 시간.
            float udt = UnityEngine.Time.unscaledDeltaTime;
            
            GamePlayClock?.Tick(udt);
            UIClock?.Tick(udt);
        }
    }
}