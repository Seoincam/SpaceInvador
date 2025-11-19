using System;
using TimeKit;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MenuTest : MonoBehaviour
    {
        private InputAction _pause;
        private CanvasGroup _canvasGroup;
        private IDisposable _pauseToken;
        
        private void OnEnable()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0;

            _pause = new InputAction("Pause", InputActionType.Button);
            _pause.AddBinding("<Keyboard>/escape");
            _pause.performed += OnPause;
            _pause.Enable();
        }

        private void OnPause(InputAction.CallbackContext _)
        {
            Toggle();
        }

        private void Toggle()
        {
            if (_pauseToken == null)
                OpenMenu();
            else
                CloseMenu();
        }

        private void OpenMenu()
        {
            Debug.Log("Opening menu");

            _pauseToken ??= TimeManager.Clocks[ClockType.GamePlay].PauseScope();

            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1;
        }

        private void CloseMenu()
        {
            _pauseToken?.Dispose();
            _pauseToken = null;
            
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0;
        }
    }
}
