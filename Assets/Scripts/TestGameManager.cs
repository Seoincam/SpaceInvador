using System;
using DG.Tweening;
using Services;
using TimeKit.Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class TestGameManager : MonoBehaviour
    {
        private IDisposable _gamePlayClockPauseToken;
        private IDisposable _uiClockPauseToken;

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