using UnityEngine;

public class TurretController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;

    public void AimAndShoot(Vector3 targetPosition)
    {
        Vector2 direction = (targetPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (projectilePrefab != null)
        {
            Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
            GameObject proj = Instantiate(projectilePrefab, spawnPos, transform.rotation);
            proj.GetComponent<Projectile>().SetTarget(targetPosition);
        }
    }
}