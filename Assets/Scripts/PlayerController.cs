using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private float moveSpeed;

    private const float Bound = 8.5f;
    
    private Rigidbody2D _rb;
    private InputAction _move;
    private Vector2 _moveInput;

    private void OnEnable()
    {
        // Rigidbody2D
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0;
        _rb.freezeRotation = true;
        
        // Input Action
        var map = inputActions.FindActionMap("PlayerCharacter");
        _move = map.FindAction("Move");
        
        map.Enable();
        _move.performed += OnMove;
        _move.canceled += OnMove;
    }

    private void FixedUpdate()
    {
        // Wrap
        Vector2 nextPos = _rb.position + _moveInput * (moveSpeed * Time.fixedDeltaTime);
        nextPos.x = nextPos.x switch
        {
            > Bound => -Bound,
            < -Bound => Bound,
            _ => nextPos.x
        };
        nextPos.y = nextPos.y switch
        {
            > Bound => -Bound,
            < -Bound => Bound,
            _ => nextPos.y
        };

        _rb.MovePosition(nextPos);
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();
    }
}
