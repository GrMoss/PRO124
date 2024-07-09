using UnityEngine;

public class BlurObjects : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] float spriteRendererColorA = 0.7f;

    void Start()
    {
        // Lấy SpriteRenderer từ GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Làm mờ đối tượng bằng cách giảm alpha của màu sắc
            Color color = spriteRenderer.color;
            color.a = spriteRendererColorA; // Đặt alpha xuống để làm mờ
            spriteRenderer.color = color;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Color color = spriteRenderer.color;
            color.a = 1f; // Đặt alpha về 100% để hiện thị đầy đủ
            spriteRenderer.color = color;
        }
    }
}
