using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public Transform player;
    public Transform spawn;

    private void Awake()//Player'ın başlangıçtaki konumu
    {
        player.transform.position = spawn.transform.position;
    }
}
