using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LastWalkDialogueSettings : MonoBehaviour
{
    public GameObject player;
    public GameObject npcFollow;
    public GameObject newNpc;
    public GameObject walkingDialogue;
    public GameObject dialogueBox;


    //Player balkabağı ile yürürken collision'a değerse hareketi kısıtlanır, döner ve konuşma başlar.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<HeroKnight>().newPumpkin == true)
            {
                walkingDialogue.GetComponent<WalkDialogue_House>().dialogueActive = false;
                walkingDialogue.GetComponent<WalkDialogue_House>().sec = 6f;
                other.GetComponent<HeroKnight>().enabled = false;
                StartCoroutine(Turn());
                StartCoroutine(StartDialogue());
                newNpc.GetComponent<NpcLine_House>().lineOfSite = 5f;
                StartCoroutine(WaitConversation());
                other.GetComponent<HeroKnight>().m_animator.SetInteger("AnimState", 0);
            }
        }

    }

    //Player 1.5 saniye sonra tersine döner
    IEnumerator Turn()
    {
        yield return new WaitForSeconds(1.5f);
        player.transform.localScale = new Vector2(1.5f, 1.5f);
    }
    IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(2.5f);
        walkingDialogue.GetComponent<WalkDialogue_House>().textMeshProUGUI.text = string.Empty;
        dialogueBox.SetActive(true);
        walkingDialogue.GetComponent<WalkDialogue_House>().StartDialogue();
        walkingDialogue.GetComponent<WalkDialogue_House>().dialogueImage.GetComponent<Image>().sprite = walkingDialogue.GetComponent<WalkDialogue_House>().sprite;
    }

    IEnumerator WaitConversation()
    {
        yield return new WaitForSeconds(6.5f);
        player.GetComponent<HeroKnight>().enabled = true;
    }
}
