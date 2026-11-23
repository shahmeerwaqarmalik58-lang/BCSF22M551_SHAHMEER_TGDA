using UnityEngine;
using TMPro;

public class FloatingDamage : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float duration = 1.0f;
    private TextMeshPro textMesh;
    private float timer;
    private Color startColor;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        startColor = textMesh.color;
    }

    public void Setup(int damageAmount)
    {
        textMesh.text = "-" + damageAmount.ToString();
    }

    void Update()
    {
        // Move upwards
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        // Fade out
        timer += Time.deltaTime;
        float alpha = Mathf.Lerp(1f, 0f, timer / duration);
        textMesh.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }
}