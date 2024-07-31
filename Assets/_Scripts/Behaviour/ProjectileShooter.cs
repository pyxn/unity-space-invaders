using UnityEngine;

public class ProjectileShooter: MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Player player;
    [SerializeField] private float projectileSpeed = 13f;
    
    private void OnEnable()
    {
        if (player != null)
        {
            player.OnFire += Player_OnFire;
        }
    }

    private void OnDisable()
    {
        if (player != null)
        {
            player.OnFire -= Player_OnFire;
        }
    }

    private void Player_OnFire(object sender, FireEventArgs e)
    {
        FireProjectile(e.Direction, projectileSpeed);
    }

    private void FireProjectile(Vector2 direction, float speed)
{
    GameObject projectile = Instantiate(projectilePrefab, player.transform.position, Quaternion.identity);
    Projectile projectileComponent = projectile.GetComponent<Projectile>();
    if (projectileComponent != null)
    {
        projectileComponent.Initialize(Vector2.up, speed);
    }
}
}
