using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using System.Collections;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.XR;

public class CharacterController2D : MonoBehaviour
{
    public float coyoteTime = 0.2f; // Даёт время на прыжок если не нажали вовремя и падаете в пропасть 
    private float coyoteTimeCounter;
    private float jumpingPower = 16f; //Сила прыжка
    private float horizontal;

    private bool isJumping;// Находится ли персонаж в воздухе?

    

    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;
    
 
    private float jumpTimeCounter;
    public Animator animator;
    [SerializeField] private float m_JumpForce = 400f;                          // Количество добавляемой силы при прыжке персонажа
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f;   // "Мощность" сглаживания передвижения
    
    [SerializeField] private LayerMask m_WhatIsGround;                          // Маска определяющая землю для персонажа (под землёй имеется ввиду объект по которому можно ходить)
    [SerializeField] private Transform m_GroundCheck;                           // Определение места, относительно персонажа, которое будет проверять наличие земли.
    [SerializeField] private Transform m_CeilingCheck;                          // Определение места, относительно персонажа, которое будет проверять наличие потолка
    
    const float k_GroundedRadius = .2f; // Радиус окружности по которому будем определять "заземлённость" персонажа
    private bool m_Grounded;            // Находится ли персонаж на земле? (true/false) "Заземлённость"
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // Если смотрит вправо - true, иначе false. В нашем случае персонаж смотрит вправо
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
            OnLandEvent = new UnityEvent(); //Создаёт события в Ispector, в данном случае событие будет связываться с другим скриптом нашего персонажа

        

       
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // Игрок заземлён если окружность вокруг точки проверки касается чего-либо
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
        animator.SetBool("Jump", !m_Grounded);//Меняет истинность Jump в аниматоре на противоположное m_Grounded

        

    }


    public void Move(float move, bool jump)// Принимает значение скорости и истинность заземлённости
    {
        // Высчитывает новое ускорение персонажу
        Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
        // И затем передвигает его(учитывая сглаживание)
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        // Если персонаж двигается вправо, но лицо находится слева то используем Flip
        if (move > 0 && !m_FacingRight)
        {
            
            Flip();
        }
        // Если персонаж двигается влево, но лицо находится справа то используем Flip
        else if (move < 0 && m_FacingRight)
        {
            
            Flip();
        }
        
        // Если игрок заземлён и находится в прыжке
        if (m_Grounded && jump)
        {
            // то добавляем вертикальную силу
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));

        }

        //Эта секция отвечает за удобную функцию - игрок может прыгать в падении, если упал несколько миллисекунд назад. Видео на соответсвующую тему можно найти в txt файле
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
        //Эта секция отвечает за удобную функцию - игрок может прыгать в падении, если упал несколько миллисекунд назад. Видео на соответсвующую тему можно найти в txt файле

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
        // Изменяет направление лица персонажа на противоположное
        m_FacingRight = !m_FacingRight;

        //Умножаем маштаб  X на -1, тем самым разворачивая персонажа
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