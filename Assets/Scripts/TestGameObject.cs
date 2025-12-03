using TimeKit;
using UnityEngine;

public class TestGameObject : MonoBehaviour
{
    void Start()
    {
        var clock = TimeManager.GetClock(ClockType.GamePlay)
            .SetLink(GetComponent<Animator>())
            .SetLink(GetComponent<ParticleSystem>())
            .SetLink(GetComponent<AudioSource>());
    }


}
