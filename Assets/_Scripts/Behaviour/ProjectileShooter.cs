using UnityEngine;

public class ProjectileShooter: MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Player playerMovement;
    
    private void OnEnable()
    {
        if (playerMovement != null)
        {
            playerMovement.OnFire += OnPlayerFire;
        }
    }

    private void OnDisable()
    {
        if (playerMovement != null)
        {
            playerMovement.OnFire -= OnPlayerFire;
        }
    }

    private void OnPlayerFire(object sender, System.EventArgs e)
    {
        Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        // Add your projectile shooting logic here
    }
}
