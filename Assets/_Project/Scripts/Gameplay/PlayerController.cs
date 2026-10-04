using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerController : Entity
{
    [Header("Player Movement Settings")]
    [SerializeField] private float jumpForce = 16f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    [SerializeField] private bool isGrounded; // Hiển thị ra Inspector để dễ theo dõi
    private float horizontalInput;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 1. Nhận phím điều khiển (Mũi tên hoặc A/D)
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // 2. Kiểm tra tiếp đất bằng vòng tròn quét
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.35f, groundLayer);
        }

        // 3. Phím Space để nhảy
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"Bấm Space! Tiếp đất (isGrounded): {isGrounded}");
            if (isGrounded)
            {
                Jump();
            }
        }

        // 4. Lật mặt nhân vật theo hướng di chuyển
        if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    public void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    protected override void Die()
    {
        Debug.Log("Chú khỉ đã hết máu!");
        gameObject.SetActive(false);
    }

    // Vẽ vòng tròn màu đỏ quanh bàn chân khỉ trong cửa sổ Scene
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.35f);
        }
    }
}