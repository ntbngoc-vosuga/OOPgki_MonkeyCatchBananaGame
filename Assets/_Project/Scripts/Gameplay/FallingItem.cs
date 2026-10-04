using UnityEngine;

// 1. Interface thể hiện chuẩn giao tiếp trừu tượng (Abstraction)
// Bất kỳ vật phẩm nào có thể thu thập được đều phải tuân thủ hợp đồng này
public interface ICollectible
{
    void OnCollected(GameObject player);
}

// 2. Lớp cơ sở trừu tượng (Abstract Class) vừa hiện thực Interface ICollectible,
// vừa chia sẻ hành vi vật lý rơi tự do và tự hủy ngoài màn hình cho các lớp con
[RequireComponent(typeof(Collider2D))]
public abstract class FallingItem : MonoBehaviour, ICollectible
{
    [Header("Falling Settings")]
    [SerializeField] protected float fallSpeed = 3.5f;
    [SerializeField] private float destroyY = -6f; // Vị trí rơi quá mép dưới màn hình thì tự hủy

    protected virtual void Update()
    {
        // Chuyển động rơi thẳng đứng từ trên xuống
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // Tự giải phóng bộ nhớ khi rơi ra khỏi màn hình
        if (transform.position.y < destroyY)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Gọi phương thức trừu tượng - Đa hình lúc chạy (Runtime Polymorphism)
            OnCollected(other.gameObject);
            Destroy(gameObject);
        }
    }

    // Phương thức trừu tượng: bắt buộc các lớp con (NormalBanana, RottenBanana, BombItem)
    // phải tự ghi đè (override) hành vi xử lý riêng của chúng
    public abstract void OnCollected(GameObject player);
}