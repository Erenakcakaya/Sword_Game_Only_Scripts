using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSettings : MonoBehaviour
{
    public GameObject player;
    void Start()
    {
        player.transform.localScale = new Vector2(1.5f, 1.5f); //player'ın scale'sin ayarlanması
    }


}
