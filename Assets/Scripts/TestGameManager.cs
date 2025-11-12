using Services;
using UnityEngine;

namespace DefaultNamespace
{
    public class TestGameManager : MonoBehaviour
    {
        private void Start()
        {
            GameServices.Spawner.Spawn("Objects/PlayerCharacter", Vector3.zero);
        }
    }
}