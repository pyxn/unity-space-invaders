using System;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public event EventHandler OnPlayerDeath;
    [SerializeField] private InputActionReference actionMove;
    [SerializeField] private InputActionReference actionFire;    
    // [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 actionMoveDirection;

    public event EventHandler OnFire;

    private void Start()
    {
        OnPlayerDeath += Player_OnPlayerDeath;
    }
    private void OnDestroy() {
        OnPlayerDeath -= Player_OnPlayerDeath;
    }
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
    Vector3 movement = new Vector3(actionMoveDirection.x * moveSpeed * Time.deltaTime, actionMoveDirection.y * moveSpeed * Time.deltaTime, 0);
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

    [SerializeField] private GameObject explosionPrefab;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boid")) 
        {
            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void Player_OnPlayerDeath(object sender, EventArgs e) 
    {
        GameManager.Instance.ChangeState(GameManager.GameState.GameOver);
    }


    private Vector2 GetScreenBounds()
{
    Camera mainCamera = Camera.main;
    Vector2 screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
    return screenBounds;
}
}
