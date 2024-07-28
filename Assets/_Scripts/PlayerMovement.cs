using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 _moveDirection;

    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference fire;

    public event EventHandler OnFire;


    private void Update()
    {
        _moveDirection = move.action.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        rigidBody.linearVelocity = new Vector2(_moveDirection.x * moveSpeed, _moveDirection.y * moveSpeed);
    }

    /* FIRE METHOD */
    private void OnEnable()
    {
        fire.action.started += Fire;
    }

    private void OnDisable()
    {
        fire.action.started -= Fire;
    }

    private void Fire(InputAction.CallbackContext obj)
    {
        OnFire?.Invoke(this, EventArgs.Empty);
    }
}
