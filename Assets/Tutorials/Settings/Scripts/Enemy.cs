using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType { Fast, Heavy, Progressive }

    [Header("Enemy Configuration")]
    public EnemyType enemyType;
    public float baseSpeed = 2f;
    public int damage = 10;

    [Header("Progressive Enemy Settings")]
    public float accelerationRate = 0.8f; // Speed added per second

    [Header("Feedback Prefab")]
    public GameObject floatingDamagePrefab;

    private float currentSpeed;
    private bool isDying = false;

    void Start()
    {
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        if (!GameManager.Instance.IsGameRunning() || isDying) return;

        // Progressive enemy speeds up over time
        if (enemyType == EnemyType.Progressive)
        {
            currentSpeed += accelerationRate * Time.deltaTime;
        }

        // Move left towards the castle (assuming Castle is on the Left)
        transform.Translate(Vector2.left * currentSpeed * Time.deltaTime);
    }

    // Handles both Mouse Click (Editor/PC) and Touch Tap (Android)
    void OnMouseDown()
    {
        if (!GameManager.Instance.IsGameRunning() || isDying) return;

        // Trigger Bonus Turret
        if (GameManager.Instance.turret != null)
        {
            GameManager.Instance.turret.AimAndShoot(transform.position);
        }

        GameManager.Instance.RegisterEnemyDefeated();
        Destroy(gameObject); // Destroyed immediately on tap, no castle damage
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDying) return;

        if (other.CompareTag("Castle"))
        {
            StartCoroutine(HitCastleRoutine(other));
        }
    }

    IEnumerator HitCastleRoutine(Collider2D castleCollider)
    {
        isDying = true;

        // 1. Exact collision point
        Vector2 hitPoint = castleCollider.ClosestPoint(transform.position);

        // 2. Spawn floating damage text
        if (floatingDamagePrefab != null)
        {
            GameObject dmgObj = Instantiate(floatingDamagePrefab, hitPoint, Quaternion.identity);
            dmgObj.GetComponent<FloatingDamage>().Setup(damage);
        }

        // 3. Inflict damage on castle
        GameManager.Instance.TakeCastleDamage(damage);

        // 4. Remain visible briefly while feedback plays, then destroy
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }
}