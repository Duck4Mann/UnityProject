using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    public Transform target; //Объект слежения камеры (персонаж)
    public Vector3 offset; // Предустановленное растояние от объекта слежения !!!!!(Z координату нужно поменять на -10 )!!!!!!
    public float damping; //Сглаживание скорости передвижения камеры
   

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
