using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
    private Rigidbody2D m_Rigidbody2D;
    public float runSpeed = 40f;// Скорость передвижения
	public Animator animator;
	float verticalMove = 0f;//Изначальное значение вертикального передвижения
	float horizontalMove = 0f;//Изначальное значение горизонтального передвижения
	bool jump = false;
	
	[Header("Events")]
	[Space]
	
    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }
    
   
    private void Start()
    {
		GetComponent<Rigidbody2D>();//Ищет RigidBody2D прикреплённое к нашуму персонажу
    }
    void Update () {
		verticalMove = Input.GetAxisRaw("Vertical");
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		if (Input.GetButtonDown("Jump"))//Если кнопка прыжка нажата, то меняем значение Jump в аниматоре на истинно
		{
			jump = true;
            animator.SetBool("Jump", true);
        }

		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));//Значение Speed в аниматоре равно модулю горизонтального передвижения

        if (Input.GetKey(KeyCode.LeftShift))//Если зажат левый шифт, то значение IsShift в аниматоре истинно
        {
            animator.SetBool("IsShift", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))//Если левый шифт был отжат, то значение IsShift в аниматоре ложно
        {
            animator.SetBool("IsShift", false);
        }


    }
	
	public void OnLanding()
	{
		animator.SetBool("Jump", false);//Ставит значение Jump в аниматоре на ложное
	}
	void FixedUpdate ()
	{
        //Отвечает за горизонтальное передвижение

        if (Input.GetKey(KeyCode.LeftShift)) //Если нажат левый шифт то используется этот код(Скорость персонажа увеличивается в 1.5 раза)
        {
            controller.Move(horizontalMove * Time.fixedDeltaTime * 1.5f,  jump);
            jump = false;
            
        }
        else//В иных случаях используются эти строчки 
		{
			
			controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
			jump = false;
		}
	}
}
