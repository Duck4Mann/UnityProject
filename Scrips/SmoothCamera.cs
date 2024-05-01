using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    public Transform target; //������ �������� ������ (��������)
    public Vector3 offset; // ����������������� ��������� �� ������� �������� !!!!!(Z ���������� ����� �������� �� -10 )!!!!!!
    public float damping; //����������� �������� ������������ ������
   

    private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    private void Awake()
    {
      
    }
    private void Update()
    {
        
        
    }
    void LateUpdate()
    {
        
        Vector3 movePosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, movePosition, ref velocity, damping);
    }
}
