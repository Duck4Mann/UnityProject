using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using System.Collections;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.XR;

public class CharacterController2D : MonoBehaviour
{
    public float coyoteTime = 0.2f; // ��� ����� �� ������ ���� �� ������ ������� � ������� � �������� 
    private float coyoteTimeCounter;
    private float jumpingPower = 16f; //���� ������
    private float horizontal;

    private bool isJumping;// ��������� �� �������� � �������?

    

    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;
    
 
    private float jumpTimeCounter;
    public Animator animator;
    [SerializeField] private float m_JumpForce = 400f;                          // ���������� ����������� ���� ��� ������ ���������
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f;   // "��������" ����������� ������������
    
    [SerializeField] private LayerMask m_WhatIsGround;                          // ����� ������������ ����� ��� ��������� (��� ����� ������� ����� ������ �� �������� ����� ������)
    [SerializeField] private Transform m_GroundCheck;                           // ����������� �����, ������������ ���������, ������� ����� ��������� ������� �����.
    [SerializeField] private Transform m_CeilingCheck;                          // ����������� �����, ������������ ���������, ������� ����� ��������� ������� �������
    
    const float k_GroundedRadius = .2f; // ������ ���������� �� �������� ����� ���������� "������������" ���������
    private bool m_Grounded;            // ��������� �� �������� �� �����? (true/false) "������������"
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // ���� ������� ������ - true, ����� false. � ����� ������ �������� ������� ������
    private Vector3 m_Velocity = Vector3.zero;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }
    public BoolEvent OnShiftEvent;
    

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
       
        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent(); //������ ������� � Ispector, � ������ ������ ������� ����� ����������� � ������ �������� ������ ���������

        

       
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // ����� ������� ���� ���������� ������ ����� �������� �������� ����-����
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
        if (wasGrounded)
        {
            
        }
        animator.SetBool("Jump", !m_Grounded);//������ ���������� Jump � ��������� �� ��������������� m_Grounded

        

    }


    public void Move(float move, bool jump)// ��������� �������� �������� � ���������� ������������
    {
        // ����������� ����� ��������� ���������
        Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
        // � ����� ����������� ���(�������� �����������)
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        // ���� �������� ��������� ������, �� ���� ��������� ����� �� ���������� Flip
        if (move > 0 && !m_FacingRight)
        {
            
            Flip();
        }
        // ���� �������� ��������� �����, �� ���� ��������� ������ �� ���������� Flip
        else if (move < 0 && m_FacingRight)
        {
            
            Flip();
        }
        
        // ���� ����� ������� � ��������� � ������
        if (m_Grounded && jump)
        {
            // �� ��������� ������������ ����
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));

        }

        //��� ������ �������� �� ������� ������� - ����� ����� ������� � �������, ���� ���� ��������� ����������� �����. ����� �� �������������� ���� ����� ����� � txt �����
        if (m_Grounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && jump)
        {
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce);

            jumpBufferCounter = 0f;

            StartCoroutine(JumpCooldown());
        }

        if (Input.GetButtonUp("Jump") && m_Rigidbody2D.velocity.y > 0f)
        {
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_Rigidbody2D.velocity.y * 0.5f);

            coyoteTimeCounter = 0f;
        }
         IEnumerator JumpCooldown()
        {
            jump = true;
            yield return new WaitForSeconds(0.4f);
            jump = false;
        }
    }

    private void Update()
    {
        //��� ������ �������� �� ������� ������� - ����� ����� ������� � �������, ���� ���� ��������� ����������� �����. ����� �� �������������� ���� ����� ����� � txt �����

        if (m_Grounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && !isJumping)
        {
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, jumpingPower);

            jumpBufferCounter = 0f;

            StartCoroutine(JumpCooldown());
        }

        if (Input.GetButtonUp("Jump") && m_Rigidbody2D.velocity.y > 0f)
        {
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_Rigidbody2D.velocity.y * 0.5f);

            coyoteTimeCounter = 0f;
        }

        
    }

    private void Flip()
    {
        // �������� ����������� ���� ��������� �� ���������������
        m_FacingRight = !m_FacingRight;

        //�������� ������  X �� -1, ��� ����� ������������ ���������
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    private IEnumerator JumpCooldown()
    {
        isJumping = true;
        yield return new WaitForSeconds(0.4f);
        isJumping = false;
    }
}