using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;
    public AudioSource textAudio;
    public Image dialogueImage;
    public Sprite sprite;
    [SerializeField] private GameObject player;

    public bool isOpen = false;
    public bool dialogueHappen;
    public string[] lines;
    public float textSpeed;
    private int index;
    private void Start()
    {
        gameObject.SetActive(false);
        dialogueHappen = false;
    }
    private void Update()
    {

        if (isOpen)
        {
            dialogueHappen = true;
            player.GetComponent<HeroKnight>().enabled = false; //Oyuncunun script'i kapatılır.
            player.GetComponent<HeroKnight>().m_animator.SetInteger("AnimState", 0);//Oyuncunun animasyonlarını durdurur.

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (textMeshProUGUI.text == lines[index])
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    textMeshProUGUI.text = lines[index];
                    gameObject.SetActive(false);

                }
            }
        }
        if (gameObject.activeInHierarchy == false)
        {
            player.GetComponent<HeroKnight>().enabled = true;//Dialogue bitince oyuncunun script'ini açar.
            isOpen = false;
        }

    }

    public void StartDialogue()
    {
        //Dialogue'u başlatır.
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        //Text'e yazma efekti ekler.
        foreach (char c in lines[index].ToCharArray())
        {
            textMeshProUGUI.text += c;
            yield return new WaitForSeconds(textSpeed);
            textAudio.Play();
        }
    }

    void NextLine()
    {
        //İlk dizi elemanı okutulduktan sonra ikincisine geçer.
        if (index < lines.Length - 1)
        {
            index++;
            textMeshProUGUI.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
