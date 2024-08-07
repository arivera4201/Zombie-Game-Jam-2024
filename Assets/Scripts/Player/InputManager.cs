using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerControls playerControls;
    private PlayerController playerController;
    private PlayerCamera playerCamera;
    [SerializeField] public Player player;
    // Start is called before the first frame update
    void Awake()
    {
        playerControls = new PlayerControls();
        playerController = GetComponent<PlayerController>();
        playerCamera = GetComponent<PlayerCamera>();
        playerControls.Movement.Jump.performed += ctx => playerController.Jump();
        playerControls.Movement.Fire.performed += ctx => player.weapon.StartFiring();
        playerControls.Movement.Fire.canceled += ctx => player.weapon.StopFiring();
        playerControls.Movement.Reload.performed += ctx => player.weapon.StartReload();
        playerControls.Movement.Interact.performed += ctx => player.Interact();
        player.playerControls = playerControls;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerController.Move(playerControls.Movement.Move.ReadValue<Vector2>(), playerControls.Movement.Sprint.IsPressed());
    }

    private void LateUpdate()
    {
        playerCamera.CameraMove(playerControls.Movement.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        playerControls.Movement.Enable();
    }

    private void OnDisable()
    {
        playerControls.Movement.Disable();
    }
}
