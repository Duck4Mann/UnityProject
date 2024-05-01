using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStomp : MonoBehaviour
{
    public float bounce;
    public Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {                                   //Смотрит тег объекта и если совпадает то объект уничтожается и задаётся ускорение по Y равное bounce
        if (other.CompareTag("Enemy")) //Тег создаём сами и, соответсвенно, его и вписываем
        {
            Destroy(other.gameObject);
            rb2d.velocity = new Vector2(rb2d.velocity.x, bounce);
        }
        if (other.CompareTag("Push"))// В данном случае объект не уничтожается(пружина)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, bounce);
        }
    }
}
