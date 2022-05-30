using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class ComboText : MonoBehaviour
{
    [SerializeField] private TMP_Text comboText;
    [SerializeField] private GameObject player;
    private float switchDur = 1;
    public Color color1;
    public Color color2;
    public Color color3;
    public Color color4;




    void Start()
    {
        comboText.enabled = false;
        comboText.color = color1;
    }

    void Update()
    {
        if (player.GetComponent<HeroKnight>().combo >= 1)
        {
            comboText.enabled = true;// Combo varsa texti gösterir.
            comboText.text = "COMBO:" + player.GetComponent<HeroKnight>().combo;//text

            //Combo'nun değerine göre renk verir.
            if (player.GetComponent<HeroKnight>().combo == 2)
            {
                comboText.color = Color.Lerp(color1, color2, 0.2f);
            }
            else if (player.GetComponent<HeroKnight>().combo == 3)
            {
                comboText.color = Color.Lerp(color1, color2, 0.4f);

            }
            else if (player.GetComponent<HeroKnight>().combo == 4)
            {
                comboText.color = Color.Lerp(color1, color2, 0.6f);
            }
            else if (player.GetComponent<HeroKnight>().combo == 5)
            {
                comboText.color = Color.Lerp(color1, color2, 0.8f);
            }
            else if (player.GetComponent<HeroKnight>().combo >= 6)
            {
                comboText.color = Color.Lerp(color1, color2, 1f);
            }
            //Renk yanıp sönerek sürenin azaldığını hatırlatır.
            if (player.GetComponent<HeroKnight>().combo <= 3 && player.GetComponent<HeroKnight>().timer >= 3)
            {
                comboText.color = Color.Lerp(color1, color4, Mathf.PingPong(Time.time / switchDur, 0.2f));
            }
            else if (player.GetComponent<HeroKnight>().combo >= 4 && player.GetComponent<HeroKnight>().timer >= 3)
            {
                comboText.color = Color.Lerp(color3, color4, Mathf.PingPong(Time.time / switchDur, 0.2f));
            }
            else if (player.GetComponent<HeroKnight>().combo >= 6 && player.GetComponent<HeroKnight>().timer >= 3)
            {
                comboText.color = Color.Lerp(color2, color3, Mathf.PingPong(Time.time / switchDur, 0.2f));
            }
        }
        else
        {
            comboText.enabled = false;// combo biterse text kapatılır.


        }
    }
}
