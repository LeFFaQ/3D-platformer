using System;
using Unity.AppUI.Redux.DevTools;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{

    [SerializeField] private InputActionAsset inputActions;

    private InputActionMap playerMap;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction interactAction;

    public Vector2 Move => moveAction.ReadValue<Vector2>();
    public Vector2 Look => lookAction.ReadValue<Vector2>();

    public event Action JumpPressed;
    public event Action InteractPressed;



    void Awake()
    {
        if (inputActions == null)
        {
            Debug.LogError($"{name}: Input actions are not assigned.", this);
            enabled = false;
            return;
        }

        inputActions = Instantiate(inputActions);

        playerMap = inputActions.FindActionMap("Player", true);
        moveAction = playerMap.FindAction("Move", true);
        jumpAction = playerMap.FindAction("Jump", true);
        lookAction = playerMap.FindAction("Look", true);
        interactAction = playerMap.FindAction("Interact", true);

    }

    void OnEnable()
    {
        if (playerMap == null) return;

        playerMap.Enable();

        jumpAction.performed += OnJumpPressed;
        interactAction.performed += OnInteractPressed;

    }

    void OnDisable()
    {
        if (playerMap == null) return;

        jumpAction.performed -= OnJumpPressed;
        interactAction.performed -= OnInteractPressed;
    }

    private void OnInteractPressed(InputAction.CallbackContext context) => InteractPressed?.Invoke();


    private void OnJumpPressed(InputAction.CallbackContext context) => JumpPressed?.Invoke();
}
