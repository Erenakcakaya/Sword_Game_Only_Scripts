using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinSceneSettings : MonoBehaviour
{
    public GameObject player;
    public GameObject dialogue;
    public GameObject barrier;


    void Start()
    {
        player.GetComponent<HeroKnight>().newPumpkin = true;
    }

    private void Update()
    {
        if (dialogue.GetComponent<Dialogue>().dialogueHappen == true)
        {
            player.GetComponent<HeroKnight>().newPumpkin = false;
            barrier.SetActive(false);

        }
    }
}
