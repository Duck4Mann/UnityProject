using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dead : MonoBehaviour
{
    // Скрипт отвечает за "смерть" персонажа,  вся сцена перезапускается в случае столкновения с объектом с соответствующим тегом
    public GameObject Player;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))//"Enemy" и "Dead" это и есть наши теги, название может быть любым. Главное создать соответсувющий тег в испекторе (и назначить нужному объекту)
        {            
            SceneManager.LoadScene("Main");//Если касаемся врага с тегом Enemy или Dead то перезапускается сцена, "Main" - название нашей сцены, изначально при создании проекта сцена называется "SampleScene"
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
