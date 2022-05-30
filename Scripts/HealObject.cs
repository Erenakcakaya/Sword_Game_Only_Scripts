using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Oyuncunun canı maksimum candan azsa 20 arttırır.
            if (other.gameObject.GetComponent<HeroKnight>().currentHealth <= other.gameObject.GetComponent<HeroKnight>().maxHealth)
            {
                other.gameObject.GetComponent<HeroKnight>().currentHealth += 20;
                if (other.gameObject.GetComponent<HeroKnight>().currentHealth > other.gameObject.GetComponent<HeroKnight>().maxHealth)//İyileştikten sonra oyuncunun canı maksimum candan fazla olursa onu maksimum cana eşitler.
                {
                    other.gameObject.GetComponent<HeroKnight>().currentHealth = other.gameObject.GetComponent<HeroKnight>().maxHealth;
                }
                other.gameObject.GetComponent<HeroKnight>().healthBar.setHealth(other.gameObject.GetComponent<HeroKnight>().currentHealth, other.gameObject.GetComponent<HeroKnight>().maxHealth);//Can barını düzenler.
                gameObject.SetActive(false);//Heal objesini yok eder.
            }
        }
    }
}
