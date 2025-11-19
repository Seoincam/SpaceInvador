using System;
using System.Collections;
using DG.Tweening;
using Services;
using TimeKit;
using UnityEngine;

namespace DefaultNamespace
{
    public class TestGameManager : MonoBehaviour
    {
        private IDisposable _gamePlayClockPauseToken;
        private IDisposable _uiClockPauseToken;

        private IClock _clock;

        private void Awake()
        {
            _clock = TimeManager.Clocks[ClockType.GamePlay];
        }

        [ContextMenu("Start Coroutine With Clock")]
        private void StartCoroutineWithClock()
        {
            StartCoroutine(CoroutineWithClock());
            return;
            
            IEnumerator CoroutineWithClock()
            {
                Debug.Log("Starting clock");
                var seconds = 0f;
                while (seconds < 10f)
                {
                    yield return _clock.WaitForSeconds(1f);
                    seconds += 1f;
                    Debug.Log("Seconds: " + seconds);
                }
                Debug.Log("Ending clock");
            }
        }

        [ContextMenu("Create TestSquares With Tween")]
        private void CreateTestSquareWithTween()
        {
            var squareA = GameServices.Spawner.Spawn("Objects/TestSquare", new Vector3(10, 0));
            squareA.transform.DOMoveY(3f, 1f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetId(ClockType.GamePlay);
            squareA.name = "GamePlayClock Tween";
            
            var squareB = GameServices.Spawner.Spawn("Objects/TestSquare", new Vector3(12, 0));
            squareB.transform.DOMoveY(3f, 1f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetId(ClockType.UI);
            squareB.name = "UIClock Tween";
        }

        [ContextMenu("Toggle GamePlay Clock Pause")]
        private void ToggleGamePlayClock()
        {
            if (_gamePlayClockPauseToken == null)
            {
                _gamePlayClockPauseToken = TimeManager.Clocks[ClockType.GamePlay].PauseScope();
            }
            else
            {
                _gamePlayClockPauseToken.Dispose();
                _gamePlayClockPauseToken = null;
            }
        }

        [ContextMenu("Toggle UI Clock Pause")]
        private void ToggleUIClockPause()
        {
            if (_uiClockPauseToken == null)
            {
                _uiClockPauseToken = TimeManager.Clocks[ClockType.UI].PauseScope();
            }
            else
            {
                _uiClockPauseToken.Dispose();
                _uiClockPauseToken = null;
            }
        }
    }
}