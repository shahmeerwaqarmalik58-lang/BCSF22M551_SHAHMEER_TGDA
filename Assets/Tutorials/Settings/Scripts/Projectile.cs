using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 25f;
    private Vector3 targetPos;

    public void SetTarget(Vector3 target)
    {
        targetPos = target;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
