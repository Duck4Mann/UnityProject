using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
    private Rigidbody2D m_Rigidbody2D;
    public float runSpeed = 40f;// �������� ������������
	public Animator animator;
	float verticalMove = 0f;//����������� �������� ������������� ������������
	float horizontalMove = 0f;//����������� �������� ��������������� ������������
	bool jump = false;
	
	[Header("Events")]
	[Space]
	
    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }
    
   
    private void Start()
    {
		GetComponent<Rigidbody2D>();//���� RigidBody2D ������������ � ������ ���������
    }
    void Update () {
		verticalMove = Input.GetAxisRaw("Vertical");
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		if (Input.GetButtonDown("Jump"))//���� ������ ������ ������, �� ������ �������� Jump � ��������� �� �������
		{
			jump = true;
            animator.SetBool("Jump", true);
        }

		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));//�������� Speed � ��������� ����� ������ ��������������� ������������

        if (Input.GetKey(KeyCode.LeftShift))//���� ����� ����� ����, �� �������� IsShift � ��������� �������
        {
            animator.SetBool("IsShift", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))//���� ����� ���� ��� �����, �� �������� IsShift � ��������� �����
        {
            animator.SetBool("IsShift", false);
        }


    }
	
	public void OnLanding()
	{
		animator.SetBool("Jump", false);//������ �������� Jump � ��������� �� ������
	}
	void FixedUpdate ()
	{
        //�������� �� �������������� ������������

        if (Input.GetKey(KeyCode.LeftShift)) //���� ����� ����� ���� �� ������������ ���� ���(�������� ��������� ������������� � 1.5 ����)
        {
            controller.Move(horizontalMove * Time.fixedDeltaTime * 1.5f,  jump);
            jump = false;
            
        }
        else//� ���� ������� ������������ ��� ������� 
		{
			
			controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
			jump = false;
		}
	}
}
