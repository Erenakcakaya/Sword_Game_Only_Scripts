using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcFollow_House : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] GameObject oldNPC;
    [SerializeField] GameObject newNPC;
    public bool startFollow = false;
    public float speed = 2.3f;


    private void Awake()
    {
        newNPC.SetActive(false);
        oldNPC.SetActive(true);
    }
    void Update()
    {
        if (player.GetComponent<HeroKnight>().newPumpkin == true)
        {
            oldNPC.SetActive(false);
            newNPC.SetActive(true);
        }
        if (startFollow)
        {
            newNPC.GetComponent<BoxCollider2D>().enabled = false;
            Debug.Log("startFollow true");
            float distanceFromPlayer = Vector2.Distance(player.position, newNPC.transform.position);
            newNPC.GetComponent<Animator>().SetBool("isWalking", false);

            if (distanceFromPlayer > newNPC.GetComponent<NpcLine_House>().lineOfSite)
            {
                Vector2 playerPos = new Vector2(player.position.x, newNPC.transform.position.y);
                newNPC.transform.position = Vector2.MoveTowards(newNPC.transform.position, playerPos, speed * Time.deltaTime);
                newNPC.GetComponent<Animator>().SetBool("isWalking", true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<HeroKnight>().newPumpkin == true)
            {
                Debug.Log("TAKİP BAŞLASIN");
                startFollow = true;
            }
        }
    }



}
