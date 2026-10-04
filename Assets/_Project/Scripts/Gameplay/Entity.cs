using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("Base Entity Stats")]
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected int maxHealth = 100;
    protected int currentHealth;

    public int CurrentHealth => currentHealth;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} nhận {damage} sát thương. Máu còn lại: {currentHealth}");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Phương thức trừu tượng: bắt buộc các lớp con (Player, Enemy) phải tự viết cách chết riêng
    protected abstract void Die();
}