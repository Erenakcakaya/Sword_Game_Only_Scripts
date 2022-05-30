using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WalkDialogue_House : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;
    public GameObject dialogue;
    public string[] lines;
    private int index;
    public float textSpeed;
    public AudioSource textAudio;
    public Image dialogueImage;
    public Sprite sprite;
    public bool dialogueActive = true;
    public float sec = 3f;


    private void Start()
    {
        dialogue.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Player collider'e değerse ve Balkabağını taşıyorsa...
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<HeroKnight>().newPumpkin == true)
            {
                dialogue.SetActive(dialogueActive);
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                if (textMeshProUGUI.text == lines[index])
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    textMeshProUGUI.text = lines[index];
                    StartCoroutine(Close());

                }

            }

        }
    }

    IEnumerator Close()
    {
        yield return new WaitForSeconds(sec);
        dialogue.SetActive(false);//Konuşmayı bitirir

    }

    
    public void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textMeshProUGUI.text += c;
            yield return new WaitForSeconds(textSpeed);
            textAudio.Play();
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textMeshProUGUI.text = string.Empty;
            StartCoroutine(TypeLine());
        }
    }

}
