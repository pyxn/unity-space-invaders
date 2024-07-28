using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference actionMove;
    [SerializeField] private InputActionReference actionFire;    
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 actionMoveDirection;

    public event EventHandler OnFire;

    private void Update()
    {
        Move();
    }

    private void OnEnable()
    {
        actionFire.action.started += OnFirePerformed;
    }

    private void OnDisable()
    {
        actionFire.action.started -= OnFirePerformed;
    }

    // Events

    private void Move()
{
    actionMoveDirection = actionMove.action.ReadValue<Vector2>();
    Vector3 movement = new Vector3(actionMoveDirection.x * moveSpeed * Time.deltaTime, 0, 0);
    Vector3 newPosition = transform.position + movement;

    Vector2 screenBounds = GetScreenBounds();

    newPosition.x = Mathf.Clamp(newPosition.x, -screenBounds.x, screenBounds.x);
    newPosition.y = Mathf.Clamp(newPosition.y, -screenBounds.y, screenBounds.y);

    transform.position = newPosition;
}
    
    private void OnFirePerformed(InputAction.CallbackContext obj)
    {
        OnFire?.Invoke(this, EventArgs.Empty);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Detected collision with a trigger");
    }

    private Vector2 GetScreenBounds()
{
    Camera mainCamera = Camera.main;
    Vector2 screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
    return screenBounds;
}
}
