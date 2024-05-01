using UnityEngine;

public class Goomba : MonoBehaviour
{
    public float moveSpeed = 2f;// �������� �����
    public Rigidbody2D rb;
    public bool facingRight = true;// ������� ������(����������)

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
        rb = GetComponent<Rigidbody2D>();//���� RigidBody2D ������������ � ������ ���������
    }

    void Update()
    {
        // ������������ ������ �����
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
        // ���� �������� ������� � ����� Wall ��� Enemy, �� �������������� � ��������������� �������
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