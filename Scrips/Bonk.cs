using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonk : MonoBehaviour
{
    // ������ HitStommp, ���������� ������� � ����� "Block" ��� ���������������
    public Rigidbody2D rb2d;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Block"))
        {
            Destroy(other.gameObject);
        }
    }
}
