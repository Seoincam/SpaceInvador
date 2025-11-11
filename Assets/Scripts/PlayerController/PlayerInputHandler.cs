using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerController
{
    [RequireComponent(typeof(IPlayerMover), typeof(IPlayerShooter))]
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputActions;

        private IPlayerMover _mover;
        private IPlayerShooter _shooter;
        
        private void OnEnable()
        {
            _mover = GetComponent<IPlayerMover>();
            _shooter = GetComponent<IPlayerShooter>();
            
            // Input Action 
            var map = inputActions.FindActionMap("PlayerCharacter");
            var move = inputActions.FindAction("Move");
            var look = inputActions.FindAction("Look");
            var fire = inputActions.FindAction("Fire");
            var retrieve =  inputActions.FindAction("Retrieve");
            
            map.Enable();
            
            move.performed += OnMove;
            move.canceled += OnMove;
            look.performed += OnLook;
            look.canceled += OnLook;
            fire.performed += OnFire;
            retrieve.performed += OnRetrieve;
        }

        private void OnMove(InputAction.CallbackContext ctx)
        {
            var moveInput = ctx.ReadValue<Vector2>();
            _mover.SetMoveInput(moveInput);
        }

        private void OnLook(InputAction.CallbackContext ctx)
        {
            var aimInput = ctx.ReadValue<Vector2>();
            _shooter.SetAimInput(aimInput);
        }

        private void OnFire(InputAction.CallbackContext ctx)
        {
            _shooter.TryFire();
        }

        private void OnRetrieve(InputAction.CallbackContext ctx)
        {
            _shooter.TryRetrieve();
        }
    }
}