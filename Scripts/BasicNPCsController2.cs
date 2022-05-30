using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicNPCsController2 : MonoBehaviour
{
    public Transform player;
    public Transform npc;

    void Update()
    {
        TurnToPlayer();
    }

    void TurnToPlayer()//NPC'yi sürekli oyuncuya döndürür.
    {
        if (player.transform.position.x > npc.transform.position.x)
        {
            npc.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            npc.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
}
