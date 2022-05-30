using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheshManager : MonoBehaviour
{
    private bool check;
    Animator anim;
    BoxCollider2D chestBox;



    private void Start()
    {
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        anim = GetComponent<Animator>();
        chestBox = GetComponent<BoxCollider2D>();

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && check)
        {
            anim.SetBool("isOpen", true);//açılma animasyonunu oynatırç
            chestBox.enabled = false;//Collider'i kapatır böylece açılan sandık bir daha açılamaz.
            gameObject.transform.GetChild(1).gameObject.SetActive(true);//içindeki alınabilir nesneyi aktif eder.



        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.GetChild(7).GetChild(0).gameObject.SetActive(true);//info textini açar
            check = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.GetChild(7).GetChild(0).gameObject.SetActive(false);//info textini kapatır
        }
    }
}
