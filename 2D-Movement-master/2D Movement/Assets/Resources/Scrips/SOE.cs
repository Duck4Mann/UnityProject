using UnityEngine;

public class Goomba : MonoBehaviour
{
    public float moveSpeed = 2f;// Скорость врага
    public Rigidbody2D rb;
    public bool facingRight = true;// Смотрит вправо(истинность)

    private void OnBecameInvisible()
    {
        enabled = false;
    }
    private void OnBecameVisible()
    {
        enabled = true;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();//Ищет RigidBody2D прикреплённое к нашему персонажу
    }

    void Update()
    {
        // Передвижение нашего врага
        if (facingRight)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Если коснулся объекта с тегом Wall или Enemy, то поворачивается в противоположную сторону
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Enemy"))
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}