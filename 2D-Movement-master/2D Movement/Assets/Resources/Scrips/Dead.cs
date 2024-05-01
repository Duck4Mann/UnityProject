using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dead : MonoBehaviour
{
    // ������ �������� �� "������" ���������,  ��� ����� ��������������� � ������ ������������ � �������� � ��������������� �����
    public GameObject Player;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))//"Enemy" � "Dead" ��� � ���� ���� ����, �������� ����� ���� �����. ������� ������� �������������� ��� � ��������� (� ��������� ������� �������)
        {            
            SceneManager.LoadScene("Main");//���� �������� ����� � ����� Enemy ��� Dead �� ��������������� �����, "Main" - �������� ����� �����, ���������� ��� �������� ������� ����� ���������� "SampleScene"
        }
        if (collision.gameObject.CompareTag("Dead"))
        {
            SceneManager.LoadScene("Main");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
