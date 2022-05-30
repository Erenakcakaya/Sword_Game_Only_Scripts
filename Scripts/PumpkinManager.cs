using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinManager : MonoBehaviour
{
    public GameObject notPump;
    public GameObject withPump;
    public GameObject player;



    private void Start()
    {
        notPump.SetActive(true);
        withPump.SetActive(false);
    }

    private void Update()
    {
        if (player.GetComponent<HeroKnight>().newPumpkin == true)//Player balkabağını alırsa
        {
            withPump.SetActive(true);
            notPump.SetActive(false);
        }
    }
}
